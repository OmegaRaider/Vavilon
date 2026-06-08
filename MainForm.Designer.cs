namespace Vavilon
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnIssueBook = new Button();
            numDays = new NumericUpDown();
            cmbReaders = new ComboBox();
            txtInvNumber = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabPage2 = new TabPage();
            btnReturnBook = new Button();
            dgvLoans = new DataGridView();
            tabPage3 = new TabPage();
            btnAddReader = new Button();
            txtPhone = new TextBox();
            txtAddress = new TextBox();
            txtMiddleName = new TextBox();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            tabPage4 = new TabPage();
            dgvLog = new DataGridView();
            tabPage5 = new TabPage();
            dataGridView1 = new DataGridView();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDays).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLoans).BeginInit();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLog).BeginInit();
            tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(784, 561);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnIssueBook);
            tabPage1.Controls.Add(numDays);
            tabPage1.Controls.Add(cmbReaders);
            tabPage1.Controls.Add(txtInvNumber);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(776, 533);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Выдача книги";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnIssueBook
            // 
            btnIssueBook.Location = new Point(340, 412);
            btnIssueBook.Name = "btnIssueBook";
            btnIssueBook.Size = new Size(109, 23);
            btnIssueBook.TabIndex = 6;
            btnIssueBook.Text = "Выдать книгу";
            btnIssueBook.UseVisualStyleBackColor = true;
            btnIssueBook.Click += btnIssueBook_Click;
            // 
            // numDays
            // 
            numDays.Location = new Point(221, 320);
            numDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numDays.Minimum = new decimal(new int[] { 14, 0, 0, 0 });
            numDays.Name = "numDays";
            numDays.Size = new Size(357, 23);
            numDays.TabIndex = 5;
            numDays.Value = new decimal(new int[] { 14, 0, 0, 0 });
            // 
            // cmbReaders
            // 
            cmbReaders.FormattingEnabled = true;
            cmbReaders.Location = new Point(221, 248);
            cmbReaders.Name = "cmbReaders";
            cmbReaders.Size = new Size(358, 23);
            cmbReaders.TabIndex = 4;
            // 
            // txtInvNumber
            // 
            txtInvNumber.Location = new Point(221, 177);
            txtInvNumber.Name = "txtInvNumber";
            txtInvNumber.Size = new Size(358, 23);
            txtInvNumber.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(76, 328);
            label3.Name = "label3";
            label3.Size = new Size(75, 15);
            label3.TabIndex = 2;
            label3.Text = "Срок (дней):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(91, 256);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 1;
            label2.Text = "Читатель:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(54, 185);
            label1.Name = "label1";
            label1.Size = new Size(125, 15);
            label1.TabIndex = 0;
            label1.Text = "Инвентарный номер:";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(btnReturnBook);
            tabPage2.Controls.Add(dgvLoans);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(776, 533);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Возврат книги";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnReturnBook
            // 
            btnReturnBook.Location = new Point(280, 503);
            btnReturnBook.Name = "btnReturnBook";
            btnReturnBook.Size = new Size(191, 23);
            btnReturnBook.TabIndex = 1;
            btnReturnBook.Text = "Вернуть выбранную книгу";
            btnReturnBook.UseVisualStyleBackColor = true;
            btnReturnBook.Click += btnReturnBook_Click;
            // 
            // dgvLoans
            // 
            dgvLoans.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvLoans.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLoans.Location = new Point(-4, 0);
            dgvLoans.Name = "dgvLoans";
            dgvLoans.Size = new Size(784, 497);
            dgvLoans.TabIndex = 0;
            dgvLoans.CellContentClick += dgvLoans_CellContentClick;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(btnAddReader);
            tabPage3.Controls.Add(txtPhone);
            tabPage3.Controls.Add(txtAddress);
            tabPage3.Controls.Add(txtMiddleName);
            tabPage3.Controls.Add(txtFirstName);
            tabPage3.Controls.Add(txtLastName);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(label7);
            tabPage3.Controls.Add(label6);
            tabPage3.Controls.Add(label5);
            tabPage3.Controls.Add(label4);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(776, 533);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Читатели";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnAddReader
            // 
            btnAddReader.Location = new Point(296, 423);
            btnAddReader.Name = "btnAddReader";
            btnAddReader.Size = new Size(127, 23);
            btnAddReader.TabIndex = 10;
            btnAddReader.Text = " Добавить читателя";
            btnAddReader.UseVisualStyleBackColor = true;
            btnAddReader.Click += btnAddReader_Click;
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(130, 307);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(517, 23);
            txtPhone.TabIndex = 9;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(130, 252);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(517, 23);
            txtAddress.TabIndex = 8;
            // 
            // txtMiddleName
            // 
            txtMiddleName.Location = new Point(130, 197);
            txtMiddleName.Name = "txtMiddleName";
            txtMiddleName.Size = new Size(517, 23);
            txtMiddleName.TabIndex = 7;
            // 
            // txtFirstName
            // 
            txtFirstName.Location = new Point(130, 142);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(517, 23);
            txtFirstName.TabIndex = 6;
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(130, 87);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(517, 23);
            txtLastName.TabIndex = 5;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(45, 310);
            label8.Name = "label8";
            label8.Size = new Size(59, 15);
            label8.TabIndex = 4;
            label8.Text = "Телефон:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(45, 255);
            label7.Name = "label7";
            label7.Size = new Size(43, 15);
            label7.TabIndex = 3;
            label7.Text = "Адрес:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(45, 200);
            label6.Name = "label6";
            label6.Size = new Size(61, 15);
            label6.TabIndex = 2;
            label6.Text = "Отчество:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(45, 145);
            label5.Name = "label5";
            label5.Size = new Size(34, 15);
            label5.TabIndex = 1;
            label5.Text = "Имя:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(45, 90);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 0;
            label4.Text = "Фамилия:";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dgvLog);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(776, 533);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Журнал действий";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvLog
            // 
            dgvLog.AllowUserToDeleteRows = false;
            dgvLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLog.Dock = DockStyle.Fill;
            dgvLog.Location = new Point(3, 3);
            dgvLog.Name = "dgvLog";
            dgvLog.ReadOnly = true;
            dgvLog.Size = new Size(770, 527);
            dgvLog.TabIndex = 0;
            dgvLog.CellContentClick += dgvLog_CellContentClick;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(dataGridView1);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(776, 533);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Каталог книг";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(770, 527);
            dataGridView1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(tabControl1);
            Name = "MainForm";
            Text = "Vavilon";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDays).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLoans).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLog).EndInit();
            tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private NumericUpDown numDays;
        private ComboBox cmbReaders;
        private TextBox txtInvNumber;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnIssueBook;
        private Button btnReturnBook;
        private DataGridView dgvLoans;
        private Button btnAddReader;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private TextBox txtMiddleName;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private DataGridView dgvLog;
        private TabPage tabPage5;
        private DataGridView dataGridView1;
    }
}