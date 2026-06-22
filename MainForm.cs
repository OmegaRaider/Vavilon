using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace Vavilon
{
    public partial class MainForm : Form
    {
        private int lastIssuedLoanId = -1;

        public MainForm()
        {
            InitializeComponent();

            tabPage4.Enter += tabLog_Enter;

            if (LoginForm.CurrentUserRole != "admin")
            {
                TabPage logTab = tabControl1.TabPages["tabPage4"];
                if (logTab != null)
                    tabControl1.TabPages.Remove(logTab);
            }

            tabPageReadersList.Enter += TabPageReadersList_Enter;
            tabPageBooks.Enter += TabPageBooks_Enter;

            this.Text = $"Библиотека — {LoginForm.CurrentUserRole} ({LoginForm.CurrentUserLogin})";

            LoadReaders();
            LoadLoans();
            LoadReadersTable();
            LoadBooks();
        }

        // ================================================================
        // 1. ВЫДАЧА КНИГИ
        // ================================================================
        private void LoadReaders()
        {
            try
            {
                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    string sql = @"SELECT reader_id, 
                                  COALESCE(last_name, '') || ' ' || COALESCE(first_name, '') AS full_name 
                           FROM readers ORDER BY last_name";
                    var da = new NpgsqlDataAdapter(sql, conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    cmbReaders.DisplayMember = "full_name";
                    cmbReaders.ValueMember = "reader_id";
                    cmbReaders.DataSource = dt;
                    if (dt.Rows.Count == 0)
                        cmbReaders.Text = "Нет зарегистрированных читателей";
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки читателей: " + ex.Message); }
        }

        private void btnIssueBook_Click(object sender, EventArgs e)
        {
            if (btnIssueBook.Enabled == false) return;
            btnIssueBook.Enabled = false;
            try
            {
                string invNumber = txtInvNumber.Text.Trim();
                if (cmbReaders.SelectedValue == null) { MessageBox.Show("Выберите читателя"); return; }
                int readerId = (int)cmbReaders.SelectedValue;
                int days = (int)numDays.Value;
                if (string.IsNullOrEmpty(invNumber)) { MessageBox.Show("Введите инвентарный номер"); return; }

                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        string findCopy = @"
                            SELECT copy_id FROM book_copies 
                            WHERE inventory_number = @inv 
                            AND copy_id NOT IN (SELECT copy_id FROM book_loans WHERE return_date IS NULL)";
                        using (var cmd = new NpgsqlCommand(findCopy, conn))
                        {
                            cmd.Parameters.AddWithValue("@inv", invNumber);
                            object obj = cmd.ExecuteScalar();
                            if (obj == null) { MessageBox.Show("Книга не найдена или уже выдана!"); return; }
                            int copyId = Convert.ToInt32(obj);

                            string getLib = "SELECT lib_id FROM librarians WHERE login = @login";
                            using (var cmdLib = new NpgsqlCommand(getLib, conn))
                            {
                                cmdLib.Parameters.AddWithValue("@login", LoginForm.CurrentUserLogin);
                                int libId = Convert.ToInt32(cmdLib.ExecuteScalar());

                                string insert = @"INSERT INTO book_loans (copy_id, reader_id, lib_id, issue_date, due_date) 
                                                  VALUES (@copy, @reader, @lib, CURRENT_DATE, CURRENT_DATE + @days) 
                                                  RETURNING loan_id";
                                using (var cmdLoan = new NpgsqlCommand(insert, conn))
                                {
                                    cmdLoan.Parameters.AddWithValue("@copy", copyId);
                                    cmdLoan.Parameters.AddWithValue("@reader", readerId);
                                    cmdLoan.Parameters.AddWithValue("@lib", libId);
                                    cmdLoan.Parameters.AddWithValue("@days", days);
                                    int loanId = Convert.ToInt32(cmdLoan.ExecuteScalar());
                                    LoginForm.LogAction("INSERT", "book_loans", loanId,
                                        $"Выдача: инв.{invNumber} → читатель ID:{readerId}, на {days} дн.");
                                    tx.Commit();
                                    lastIssuedLoanId = loanId;
                                    txtInvNumber.Text = "";
                                    LoadLoans();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка выдачи: " + ex.Message); }
            finally { btnIssueBook.Enabled = true; }
        }

        // ================================================================
        // 2. ВОЗВРАТ КНИГИ
        // ================================================================
        private void LoadLoans()
        {
            try
            {
                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    string sql = @"
                SELECT bl.loan_id, 
                       r.last_name || ' ' || r.first_name AS читатель,
                       cb.title || ' (' || cb.author || ')' AS книга,
                       bc.inventory_number AS инвентарный_номер, 
                       bl.issue_date AS дата_выдачи, 
                       bl.due_date AS срок_возврата,
                       CASE 
                           WHEN bl.return_date IS NOT NULL THEN 'Возвращена'
                           WHEN bl.due_date < CURRENT_DATE THEN 'Просрочено'
                           ELSE 'На руках'
                       END AS статус
                FROM book_loans bl
                JOIN book_copies bc ON bl.copy_id = bc.copy_id
                JOIN catalog_books cb ON bc.book_id = cb.book_id
                JOIN readers r ON bl.reader_id = r.reader_id
                ORDER BY bl.issue_date DESC";

                    // Проверка на просрочку:
                    /* CASE 
                           WHEN bl.return_date IS NOT NULL THEN 'Возвращена'
                           WHEN bl.due_date < CURRENT_DATE THEN 'Просрочено'
                           ELSE 'На руках'
                       END AS статус*/
                    var da = new NpgsqlDataAdapter(sql, conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvLoans.DataSource = null;
                    dgvLoans.DataSource = dt;
                    if (dgvLoans.Columns.Count > 0)
                    {
                        dgvLoans.Columns["loan_id"].Visible = false;
                        dgvLoans.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                    if (lastIssuedLoanId != -1) SelectLoanById(lastIssuedLoanId);
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки выдач: " + ex.Message); }
        }

        private void SelectLoanById(int loanId)
        {
            foreach (DataGridViewRow row in dgvLoans.Rows)
            {
                if (row.Cells[0].Value != null && Convert.ToInt32(row.Cells[0].Value) == loanId)
                {
                    row.Selected = true;
                    dgvLoans.FirstDisplayedScrollingRowIndex = row.Index;
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    string bookTitle = row.Cells["книга"].Value?.ToString();
                    MessageBox.Show($"Книга \"{bookTitle}\" успешно выдана!",
                        "Книга выдана", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tabControl1.SelectedTab = tabPage2;
                    break;
                }
            }
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (btnReturnBook.Enabled == false) return;
            btnReturnBook.Enabled = false;
            try
            {
                if (dgvLoans.SelectedRows.Count == 0) { MessageBox.Show("Выберите строку выдачи"); return; }
                string status = dgvLoans.SelectedRows[0].Cells["статус"].Value?.ToString();
                if (status == "Возвращена") { MessageBox.Show("Эта книга уже возвращена!"); return; }
                int loanId = Convert.ToInt32(dgvLoans.SelectedRows[0].Cells[0].Value);
                string bookTitle = dgvLoans.SelectedRows[0].Cells["книга"].Value?.ToString();
                if (MessageBox.Show($"Вернуть книгу \"{bookTitle}\"?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        string sql = "UPDATE book_loans SET return_date = CURRENT_DATE WHERE loan_id = @id AND return_date IS NULL";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", loanId);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                LoginForm.LogAction("UPDATE", "book_loans", loanId, $"Возврат: {bookTitle}");
                                tx.Commit();
                                MessageBox.Show($"Книга \"{bookTitle}\" возвращена!", "Успех");
                                lastIssuedLoanId = -1;
                                LoadLoans();
                            }
                            else MessageBox.Show("Ошибка: книга уже возвращена или не найдена");
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка возврата: " + ex.Message); }
            finally { btnReturnBook.Enabled = true; }
        }

        // ================================================================
        // 3. ДОБАВИТЬ ЧИТАТЕЛЯ
        // ================================================================
        private void btnAddReader_Click(object sender, EventArgs e)
        {
            if (btnAddReader.Enabled == false) return;
            btnAddReader.Enabled = false;
            try
            {
                string lastName = txtLastName.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                string middleName = txtMiddleName.Text.Trim();
                string address = txtAddress.Text.Trim();
                string phone = txtPhone.Text.Trim();
                if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
                { MessageBox.Show("Фамилия и имя обязательны"); return; }

                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        string sql = @"INSERT INTO readers (last_name, first_name, middle_name, address, phone) 
                                       VALUES (@l, @f, @m, @a, @p) RETURNING reader_id";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@l", lastName);
                            cmd.Parameters.AddWithValue("@f", firstName);
                            cmd.Parameters.AddWithValue("@m", (object)middleName ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@a", (object)address ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@p", (object)phone ?? DBNull.Value);
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            LoginForm.LogAction("INSERT", "readers", newId, $"Добавлен: {lastName} {firstName}");
                            tx.Commit();
                            MessageBox.Show($"Читатель \"{lastName} {firstName}\" добавлен!");
                            txtLastName.Clear(); txtFirstName.Clear(); txtMiddleName.Clear();
                            txtAddress.Clear(); txtPhone.Clear();
                            LoadReaders();
                            LoadReadersTable();
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
            finally { btnAddReader.Enabled = true; }
        }

        // ================================================================
        // 4. СПИСОК ЧИТАТЕЛЕЙ (фильтры: ФИО, телефон, адрес, дата регистрации)
        // ================================================================
        private void TabPageReadersList_Enter(object sender, EventArgs e)
        {
            ResetReaderFilters();
            LoadReadersTable();
        }

        private void ResetReaderFilters()
        {
            txtFilterFIO.Text = "";
            txtFilterPhone.Text = "";
            txtFilterAddress.Text = "";
            chkRegDateEnabled.Checked = false;
            dtpFilterRegFrom.Value = DateTime.Today;
            dtpFilterRegTo.Value = DateTime.Today;
        }

        private void LoadReadersTable()
        {
            try
            {
                string fio = txtFilterFIO.Text.Trim();
                string phone = txtFilterPhone.Text.Trim();
                string address = txtFilterAddress.Text.Trim();
                bool dateEnabled = chkRegDateEnabled.Checked;
                DateTime regFrom = dtpFilterRegFrom.Value.Date;
                DateTime regTo = dtpFilterRegTo.Value.Date;

                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT reader_id, last_name, first_name, middle_name, address, phone, registration_date FROM readers WHERE 1=1";
                    var cmd = new NpgsqlCommand();
                    cmd.Connection = conn;

                    if (!string.IsNullOrEmpty(fio))
                    {
                        sql += " AND (last_name || ' ' || first_name || ' ' || COALESCE(middle_name, '')) ILIKE '%' || @fio || '%'";
                        cmd.Parameters.AddWithValue("@fio", fio);
                    }
                    if (!string.IsNullOrEmpty(phone))
                    {
                        sql += " AND phone ILIKE '%' || @phone || '%'";
                        cmd.Parameters.AddWithValue("@phone", phone);
                    }
                    if (!string.IsNullOrEmpty(address))
                    {
                        sql += " AND address ILIKE '%' || @address || '%'";
                        cmd.Parameters.AddWithValue("@address", address);
                    }
                    if (dateEnabled)
                    {
                        sql += " AND registration_date >= @regFrom AND registration_date <= @regTo";
                        cmd.Parameters.AddWithValue("@regFrom", regFrom);
                        cmd.Parameters.AddWithValue("@regTo", regTo);
                    }

                    sql += " ORDER BY last_name";
                    cmd.CommandText = sql;

                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    dgvReaders.DataSource = dt;
                    if (dgvReaders.Columns.Count > 0)
                    {
                        dgvReaders.Columns[0].Visible = false;
                        dgvReaders.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки списка: " + ex.Message); }
        }

        private void btnApplyReaderFilters_Click(object sender, EventArgs e)
        {
            LoadReadersTable();
            LoginForm.LogAction("SEARCH", "readers", 0, "Применены фильтры читателей");
        }

        private void btnResetReaderFilters_Click(object sender, EventArgs e)
        {
            ResetReaderFilters();
            LoadReadersTable();
            LoginForm.LogAction("SEARCH", "readers", 0, "Сброс фильтров читателей");
        }

        // ================================================================
        // 5. КАТАЛОГ КНИГ (фильтры: автор, название, издательство, год)
        // ================================================================
        private void TabPageBooks_Enter(object sender, EventArgs e)
        {
            ResetBookFilters();
            LoadBooks();
        }

        private void ResetBookFilters()
        {
            txtFilterAuthor.Text = "";
            txtFilterTitle.Text = "";
            txtFilterPublisher.Text = "";
            numFilterYearFrom.Value = 0;
            numFilterYearTo.Value = 0;
        }

        private void LoadBooks()
        {
            try
            {
                string author = txtFilterAuthor.Text.Trim();
                string title = txtFilterTitle.Text.Trim();
                string publisher = txtFilterPublisher.Text.Trim();
                int yearFrom = (int)numFilterYearFrom.Value;
                int yearTo = (int)numFilterYearTo.Value;

                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    string sql = @"
                SELECT bc.copy_id, cb.author, cb.title, cb.publisher, cb.year_pub,
                       bc.inventory_number, cb.total_quantity,
                       CASE WHEN EXISTS (
                           SELECT 1 FROM book_loans bl2 
                           WHERE bl2.copy_id = bc.copy_id AND bl2.return_date IS NULL
                       ) THEN 'Выдан' ELSE 'В наличии' END AS статус_экземпляра
                FROM catalog_books cb
                JOIN book_copies bc ON cb.book_id = bc.book_id
                WHERE 1=1";
                    var cmd = new NpgsqlCommand();
                    cmd.Connection = conn;

                    if (!string.IsNullOrEmpty(author))
                    {
                        sql += " AND cb.author ILIKE '%' || @author || '%'";
                        cmd.Parameters.AddWithValue("@author", author);
                    }
                    if (!string.IsNullOrEmpty(title))
                    {
                        sql += " AND cb.title ILIKE '%' || @title || '%'";
                        cmd.Parameters.AddWithValue("@title", title);
                    }
                    if (!string.IsNullOrEmpty(publisher))
                    {
                        sql += " AND cb.publisher ILIKE '%' || @publisher || '%'";
                        cmd.Parameters.AddWithValue("@publisher", publisher);
                    }
                    if (yearFrom > 0)
                    {
                        sql += " AND cb.year_pub >= @yearFrom";
                        cmd.Parameters.AddWithValue("@yearFrom", yearFrom);
                    }
                    if (yearTo > 0)
                    {
                        sql += " AND cb.year_pub <= @yearTo";
                        cmd.Parameters.AddWithValue("@yearTo", yearTo);
                    }

                    sql += " ORDER BY cb.author, cb.title, bc.inventory_number";
                    cmd.CommandText = sql;

                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    dgvBooks.DataSource = dt;

                    if (dgvBooks.Columns.Count > 0)
                    {
                        dgvBooks.Columns["copy_id"].Visible = false;
                        dgvBooks.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки каталога: " + ex.Message); }
        }

        private void btnApplyBookFilters_Click(object sender, EventArgs e)
        {
            LoadBooks();
            LoginForm.LogAction("SEARCH", "catalog_books", 0, "Применены фильтры книг");
        }

        private void btnResetBookFilters_Click(object sender, EventArgs e)
        {
            ResetBookFilters();
            LoadBooks();
            LoginForm.LogAction("SEARCH", "catalog_books", 0, "Сброс фильтров книг");
        }

        // ================================================================
        // 6. ЖУРНАЛ ДЕЙСТВИЙ
        // ================================================================
        private void tabLog_Enter(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM action_log ORDER BY action_time DESC LIMIT 200";
                    var da = new NpgsqlDataAdapter(sql, conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvLog.DataSource = dt;
                    if (dgvLog.Columns.Count > 0)
                        dgvLog.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка журнала: " + ex.Message); }
        }

        private void dgvLoans_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvLog_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void txtFilterFIO_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void tabPageBooks_Click(object sender, EventArgs e)
        {

        }
    }
}
