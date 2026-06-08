using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Vavilon
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Подключаем обработчики событий
            btnIssueBook.Click += btnIssueBook_Click;
            btnReturnBook.Click += btnReturnBook_Click;
            btnAddReader.Click += btnAddReader_Click;
            tabPage4.Enter += tabLog_Enter;

            // Прячем вкладку "Журнал действий" от обычных библиотекарей
            if (LoginForm.CurrentUserRole != "admin")
                tabControl1.TabPages.RemoveAt(3);

            this.Text = $"Библиотека — {LoginForm.CurrentUserRole} ({LoginForm.CurrentUserLogin})";

            // Загружаем данные
            LoadReaders();
            LoadLoans();
        }

        // Загрузка списка читателей в выпадающий список
        private void LoadReaders()
        {
            using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT reader_id, last_name || ' ' || first_name AS full_name FROM readers ORDER BY last_name";
                var da = new NpgsqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);

                cmbReaders.DisplayMember = "full_name";
                cmbReaders.ValueMember = "reader_id";
                cmbReaders.DataSource = dt;
            }
        }

        // Загрузка списка выданных книг в таблицу
        private void LoadLoans()
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
                           CASE WHEN bl.return_date IS NULL THEN 'На руках' ELSE 'Возвращена' END AS статус
                    FROM book_loans bl
                    JOIN book_copies bc ON bl.copy_id = bc.copy_id
                    JOIN catalog_books cb ON bc.book_id = cb.book_id
                    JOIN readers r ON bl.reader_id = r.reader_id
                    ORDER BY bl.issue_date DESC";

                var da = new NpgsqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgvLoans.DataSource = dt;
            }
        }

        // Выдача книги
        private void btnIssueBook_Click(object sender, EventArgs e)
        {
            string invNumber = txtInvNumber.Text.Trim();

            if (cmbReaders.SelectedValue == null)
            {
                MessageBox.Show("Выберите читателя");
                return;
            }

            int readerId = (int)cmbReaders.SelectedValue;
            int days = (int)numDays.Value;

            if (string.IsNullOrEmpty(invNumber))
            {
                MessageBox.Show("Введите инвентарный номер книги");
                return;
            }

            using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
            {
                conn.Open();

                // Начинаем транзакцию для атомарности операции
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Ищем свободный экземпляр
                        string findCopy = @"
                            SELECT copy_id FROM book_copies 
                            WHERE inventory_number = @inv 
                            AND copy_id NOT IN (SELECT copy_id FROM book_loans WHERE return_date IS NULL)";

                        using (var cmd = new NpgsqlCommand(findCopy, conn))
                        {
                            cmd.Parameters.AddWithValue("@inv", invNumber);
                            object copyObj = cmd.ExecuteScalar();

                            if (copyObj == null)
                            {
                                MessageBox.Show("Книга не найдена или уже выдана!");
                                return;
                            }

                            int copyId = Convert.ToInt32(copyObj);

                            // Находим библиотекаря по логину
                            string getLib = "SELECT lib_id FROM librarians WHERE login = @login";
                            using (var cmdLib = new NpgsqlCommand(getLib, conn))
                            {
                                cmdLib.Parameters.AddWithValue("@login", LoginForm.CurrentUserLogin);
                                object libObj = cmdLib.ExecuteScalar();

                                if (libObj == null)
                                {
                                    MessageBox.Show("Библиотекарь не найден!");
                                    return;
                                }

                                int libId = Convert.ToInt32(libObj);

                                // Создаём запись выдачи
                                string insertLoan = @"
                                    INSERT INTO book_loans (copy_id, reader_id, lib_id, issue_date, due_date) 
                                    VALUES (@copy, @reader, @lib, CURRENT_DATE, CURRENT_DATE + @days) 
                                    RETURNING loan_id";

                                using (var cmdLoan = new NpgsqlCommand(insertLoan, conn))
                                {
                                    cmdLoan.Parameters.AddWithValue("@copy", copyId);
                                    cmdLoan.Parameters.AddWithValue("@reader", readerId);
                                    cmdLoan.Parameters.AddWithValue("@lib", libId);
                                    cmdLoan.Parameters.AddWithValue("@days", days);

                                    int loanId = Convert.ToInt32(cmdLoan.ExecuteScalar());

                                    // Запись в журнал
                                    LoginForm.LogAction("INSERT", "book_loans", loanId,
                                        $"Выдача: инв.{invNumber} → читатель ID:{readerId}, на {days} дн.");

                                    MessageBox.Show($"Книга выдана! ID записи: {loanId}");

                                    // Обновляем таблицу
                                    LoadLoans();

                                    // Очищаем поле инвентарного номера
                                    txtInvNumber.Text = "";

                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при выдаче книги: {ex.Message}");
                    }
                }
            }
        }

        // Возврат книги
        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (dgvLoans.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку выдачи в таблице ниже");
                return;
            }

            // Проверяем, что книга еще не возвращена
            string status = dgvLoans.SelectedRows[0].Cells["статус"].Value?.ToString();
            if (status == "Возвращена")
            {
                MessageBox.Show("Эта книга уже возвращена!");
                return;
            }

            // Находим loan_id в выбранной строке
            int loanId = Convert.ToInt32(dgvLoans.SelectedRows[0].Cells[0].Value);

            using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = "UPDATE book_loans SET return_date = CURRENT_DATE WHERE loan_id = @id AND return_date IS NULL";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", loanId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                LoginForm.LogAction("UPDATE", "book_loans", loanId, "Возврат книги");
                                MessageBox.Show("Книга возвращена!");

                                // Обновляем таблицу
                                LoadLoans();
                                transaction.Commit();
                            }
                            else
                            {
                                MessageBox.Show("Ошибка: книга не найдена или уже возвращена!");
                                transaction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при возврате книги: {ex.Message}");
                    }
                }
            }
        }

        // Добавление читателя
        private void btnAddReader_Click(object sender, EventArgs e)
        {
            string lastName = txtLastName.Text.Trim();
            string firstName = txtFirstName.Text.Trim();
            string middleName = txtMiddleName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                MessageBox.Show("Фамилия и имя обязательны для заполнения");
                return;
            }

            using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"
                            INSERT INTO readers (last_name, first_name, middle_name, address, phone) 
                            VALUES (@l, @f, @m, @a, @p) RETURNING reader_id";

                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@l", lastName);
                            cmd.Parameters.AddWithValue("@f", firstName);
                            cmd.Parameters.AddWithValue("@m", string.IsNullOrEmpty(middleName) ? (object)DBNull.Value : middleName);
                            cmd.Parameters.AddWithValue("@a", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);
                            cmd.Parameters.AddWithValue("@p", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);

                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            LoginForm.LogAction("INSERT", "readers", newId, $"Добавлен: {lastName} {firstName}");
                            MessageBox.Show("Читатель добавлен!");

                            // Очищаем поля
                            txtLastName.Text = "";
                            txtFirstName.Text = "";
                            txtMiddleName.Text = "";
                            txtAddress.Text = "";
                            txtPhone.Text = "";

                            // Обновляем список читателей
                            LoadReaders();

                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при добавлении читателя: {ex.Message}");
                    }
                }
            }
        }

        // Загрузка журнала действий (когда открываем вкладку)
        private void tabLog_Enter(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM action_log ORDER BY action_time DESC LIMIT 200";
                var da = new NpgsqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgvLog.DataSource = dt;
            }
        }
    }
}