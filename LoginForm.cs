using System;
using System.Windows.Forms;
using Npgsql;
using Vavilon;

namespace Vavilon
{
    public partial class LoginForm : Form
    {
        // Строка подключения к вашей БД в pgAdmin
        // Замените параметры на свои: хост, порт, имя БД, пользователь, пароль
        public static string ConnectionString =
            "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123";

        public static string CurrentUserLogin;
        public static string CurrentUserRole;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT role FROM librarians WHERE login = @login AND password_hash = @password";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        CurrentUserLogin = login;
                        CurrentUserRole = result.ToString();

                        // Пишем в ЖУРНАЛ факт входа
                        LogAction("LOGIN", "librarians", 0, $"Вход в систему: {CurrentUserRole}");

                        MainForm main = new MainForm();
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }
            }
        }

        // Метод для записи в ЖУРНАЛ — будет доступен везде
        public static void LogAction(string actionType, string tableName, int recordId, string details)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO action_log (user_login, action_type, table_affected, record_id, details) 
                               VALUES (@user, @type, @table, @id, @details)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", CurrentUserLogin ?? "system");
                    cmd.Parameters.AddWithValue("@type", actionType);
                    cmd.Parameters.AddWithValue("@table", tableName);
                    cmd.Parameters.AddWithValue("@id", recordId);
                    cmd.Parameters.AddWithValue("@details", details);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}