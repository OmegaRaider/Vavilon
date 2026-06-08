using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Vavilon
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Прячем вкладку "Журнал действий" от обычных библиотекарей
            if (LoginForm.CurrentUserRole != "admin")
                tabControl1.TabPages.RemoveAt(3); // индекс 3 = 4-я вкладка (Журнал)

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
                        int libId = Convert.ToInt32(cmdLib.ExecuteScalar());

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
                        }
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

            // Находим loan_id в выбранной строке (индекс 0 = первый столбец)
            int loanId = Convert.ToInt32(dgvLoans.SelectedRows[0].Cells[0].Value);

            using (var conn = new NpgsqlConnection(LoginForm.ConnectionString))
            {
                conn.Open();
                string sql = "UPDATE book_loans SET return_date = CURRENT_DATE WHERE loan_id = @id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", loanId);
                    cmd.ExecuteNonQuery();

                    LoginForm.LogAction("UPDATE", "book_loans", loanId, "Возврат книги");
                    MessageBox.Show("Книга возвращена!");

                    // Обновляем таблицу
                    LoadLoans();
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

        private void btnIssueBook_Click_1(object sender, EventArgs e)
        {

        }
    }
}