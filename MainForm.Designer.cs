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
            tabPageReadersList = new TabPage();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            btnResetReaderFilters = new Button();
            btnApplyReaderFilters = new Button();
            dtpFilterRegTo = new DateTimePicker();
            dtpFilterRegFrom = new DateTimePicker();
            chkRegDateEnabled = new CheckBox();
            txtFilterAddress = new TextBox();
            txtFilterPhone = new TextBox();
            txtFilterFIO = new TextBox();
            dgvReaders = new DataGridView();
            tabPageBooks = new TabPage();
            label13 = new Label();
            label12 = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            numFilterYearTo = new NumericUpDown();
            numFilterYearFrom = new NumericUpDown();
            txtFilterPublisher = new TextBox();
            txtFilterTitle = new TextBox();
            txtFilterAuthor = new TextBox();
            btnResetBookFilters = new Button();
            btnApplyBookFilters = new Button();
            dgvBooks = new DataGridView();
            tabPage4 = new TabPage();
            dgvLog = new DataGridView();
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
            tabPage2 = new TabPage();
            btnReturnBook = new Button();
            dgvLoans = new DataGridView();
            tabPage1 = new TabPage();
            btnIssueBook = new Button();
            numDays = new NumericUpDown();
            cmbReaders = new ComboBox();
            txtInvNumber = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabControl1 = new TabControl();
            tabPageReadersList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReaders).BeginInit();
            tabPageBooks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFilterYearTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFilterYearFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLog).BeginInit();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLoans).BeginInit();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDays).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabPageReadersList
            // 
            tabPageReadersList.Controls.Add(label18);
            tabPageReadersList.Controls.Add(label17);
            tabPageReadersList.Controls.Add(label16);
            tabPageReadersList.Controls.Add(label15);
            tabPageReadersList.Controls.Add(label14);
            tabPageReadersList.Controls.Add(btnResetReaderFilters);
            tabPageReadersList.Controls.Add(btnApplyReaderFilters);
            tabPageReadersList.Controls.Add(dtpFilterRegTo);
            tabPageReadersList.Controls.Add(dtpFilterRegFrom);
            tabPageReadersList.Controls.Add(chkRegDateEnabled);
            tabPageReadersList.Controls.Add(txtFilterAddress);
            tabPageReadersList.Controls.Add(txtFilterPhone);
            tabPageReadersList.Controls.Add(txtFilterFIO);
            tabPageReadersList.Controls.Add(dgvReaders);
            tabPageReadersList.Location = new Point(4, 24);
            tabPageReadersList.Name = "tabPageReadersList";
            tabPageReadersList.Padding = new Padding(3);
            tabPageReadersList.Size = new Size(776, 533);
            tabPageReadersList.TabIndex = 6;
            tabPageReadersList.Text = "Список читателей";
            tabPageReadersList.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(30, 234);
            label18.Name = "label18";
            label18.Size = new Size(52, 15);
            label18.TabIndex = 17;
            label18.Text = "Дата по:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(30, 195);
            label17.Name = "label17";
            label17.Size = new Size(44, 15);
            label17.TabIndex = 16;
            label17.Text = "Дата c:";
            label17.Click += label17_Click;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(30, 123);
            label16.Name = "label16";
            label16.Size = new Size(43, 15);
            label16.TabIndex = 15;
            label16.Text = "Адрес:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(30, 78);
            label15.Name = "label15";
            label15.Size = new Size(104, 15);
            label15.TabIndex = 14;
            label15.Text = "Номер телефона:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(30, 33);
            label14.Name = "label14";
            label14.Size = new Size(89, 15);
            label14.TabIndex = 13;
            label14.Text = "ФИО читателя:";
            // 
            // btnResetReaderFilters
            // 
            btnResetReaderFilters.Location = new Point(566, 161);
            btnResetReaderFilters.Name = "btnResetReaderFilters";
            btnResetReaderFilters.Size = new Size(150, 23);
            btnResetReaderFilters.TabIndex = 12;
            btnResetReaderFilters.Text = "Сбросить фильтры";
            btnResetReaderFilters.UseVisualStyleBackColor = true;
            btnResetReaderFilters.Click += btnResetReaderFilters_Click;
            // 
            // btnApplyReaderFilters
            // 
            btnApplyReaderFilters.Location = new Point(566, 111);
            btnApplyReaderFilters.Name = "btnApplyReaderFilters";
            btnApplyReaderFilters.Size = new Size(150, 23);
            btnApplyReaderFilters.TabIndex = 11;
            btnApplyReaderFilters.Text = "Применить фильтры";
            btnApplyReaderFilters.UseVisualStyleBackColor = true;
            btnApplyReaderFilters.Click += btnApplyReaderFilters_Click;
            // 
            // dtpFilterRegTo
            // 
            dtpFilterRegTo.Location = new Point(155, 228);
            dtpFilterRegTo.Name = "dtpFilterRegTo";
            dtpFilterRegTo.Size = new Size(335, 23);
            dtpFilterRegTo.TabIndex = 10;
            // 
            // dtpFilterRegFrom
            // 
            dtpFilterRegFrom.Location = new Point(155, 189);
            dtpFilterRegFrom.Name = "dtpFilterRegFrom";
            dtpFilterRegFrom.Size = new Size(335, 23);
            dtpFilterRegFrom.TabIndex = 9;
            // 
            // chkRegDateEnabled
            // 
            chkRegDateEnabled.AutoSize = true;
            chkRegDateEnabled.Location = new Point(155, 164);
            chkRegDateEnabled.Name = "chkRegDateEnabled";
            chkRegDateEnabled.Size = new Size(241, 19);
            chkRegDateEnabled.TabIndex = 8;
            chkRegDateEnabled.Text = "Включить фильтр по дате регистрации";
            chkRegDateEnabled.UseVisualStyleBackColor = true;
            // 
            // txtFilterAddress
            // 
            txtFilterAddress.Location = new Point(155, 120);
            txtFilterAddress.Name = "txtFilterAddress";
            txtFilterAddress.Size = new Size(335, 23);
            txtFilterAddress.TabIndex = 7;
            // 
            // txtFilterPhone
            // 
            txtFilterPhone.Location = new Point(155, 75);
            txtFilterPhone.Name = "txtFilterPhone";
            txtFilterPhone.Size = new Size(335, 23);
            txtFilterPhone.TabIndex = 6;
            // 
            // txtFilterFIO
            // 
            txtFilterFIO.Location = new Point(155, 30);
            txtFilterFIO.Name = "txtFilterFIO";
            txtFilterFIO.Size = new Size(335, 23);
            txtFilterFIO.TabIndex = 5;
            txtFilterFIO.TextChanged += txtFilterFIO_TextChanged;
            // 
            // dgvReaders
            // 
            dgvReaders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvReaders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReaders.Location = new Point(0, 273);
            dgvReaders.Name = "dgvReaders";
            dgvReaders.ReadOnly = true;
            dgvReaders.Size = new Size(777, 260);
            dgvReaders.TabIndex = 3;
            // 
            // tabPageBooks
            // 
            tabPageBooks.Controls.Add(label13);
            tabPageBooks.Controls.Add(label12);
            tabPageBooks.Controls.Add(label11);
            tabPageBooks.Controls.Add(label10);
            tabPageBooks.Controls.Add(label9);
            tabPageBooks.Controls.Add(numFilterYearTo);
            tabPageBooks.Controls.Add(numFilterYearFrom);
            tabPageBooks.Controls.Add(txtFilterPublisher);
            tabPageBooks.Controls.Add(txtFilterTitle);
            tabPageBooks.Controls.Add(txtFilterAuthor);
            tabPageBooks.Controls.Add(btnResetBookFilters);
            tabPageBooks.Controls.Add(btnApplyBookFilters);
            tabPageBooks.Controls.Add(dgvBooks);
            tabPageBooks.Location = new Point(4, 24);
            tabPageBooks.Name = "tabPageBooks";
            tabPageBooks.Padding = new Padding(3);
            tabPageBooks.Size = new Size(776, 533);
            tabPageBooks.TabIndex = 4;
            tabPageBooks.Text = "Каталог книг";
            tabPageBooks.UseVisualStyleBackColor = true;
            tabPageBooks.Click += tabPageBooks_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(31, 212);
            label13.Name = "label13";
            label13.Size = new Size(45, 15);
            label13.TabIndex = 12;
            label13.Text = "Год до:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(31, 167);
            label12.Name = "label12";
            label12.Size = new Size(44, 15);
            label12.TabIndex = 11;
            label12.Text = "Год от:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(30, 123);
            label11.Name = "label11";
            label11.Size = new Size(84, 15);
            label11.TabIndex = 10;
            label11.Text = "Издательство:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(30, 78);
            label10.Name = "label10";
            label10.Size = new Size(62, 15);
            label10.TabIndex = 9;
            label10.Text = "Название:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(31, 33);
            label9.Name = "label9";
            label9.Size = new Size(43, 15);
            label9.TabIndex = 8;
            label9.Text = "\tАвтор:";
            // 
            // numFilterYearTo
            // 
            numFilterYearTo.Location = new Point(155, 210);
            numFilterYearTo.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            numFilterYearTo.Name = "numFilterYearTo";
            numFilterYearTo.Size = new Size(335, 23);
            numFilterYearTo.TabIndex = 7;
            // 
            // numFilterYearFrom
            // 
            numFilterYearFrom.Location = new Point(155, 165);
            numFilterYearFrom.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            numFilterYearFrom.Name = "numFilterYearFrom";
            numFilterYearFrom.Size = new Size(335, 23);
            numFilterYearFrom.TabIndex = 6;
            // 
            // txtFilterPublisher
            // 
            txtFilterPublisher.Location = new Point(155, 120);
            txtFilterPublisher.Name = "txtFilterPublisher";
            txtFilterPublisher.Size = new Size(335, 23);
            txtFilterPublisher.TabIndex = 5;
            // 
            // txtFilterTitle
            // 
            txtFilterTitle.Location = new Point(155, 75);
            txtFilterTitle.Name = "txtFilterTitle";
            txtFilterTitle.Size = new Size(335, 23);
            txtFilterTitle.TabIndex = 4;
            // 
            // txtFilterAuthor
            // 
            txtFilterAuthor.Location = new Point(155, 30);
            txtFilterAuthor.Name = "txtFilterAuthor";
            txtFilterAuthor.Size = new Size(335, 23);
            txtFilterAuthor.TabIndex = 3;
            // 
            // btnResetBookFilters
            // 
            btnResetBookFilters.Location = new Point(567, 177);
            btnResetBookFilters.Name = "btnResetBookFilters";
            btnResetBookFilters.Size = new Size(150, 23);
            btnResetBookFilters.TabIndex = 2;
            btnResetBookFilters.Text = "Сбросить фильтры";
            btnResetBookFilters.UseVisualStyleBackColor = true;
            btnResetBookFilters.Click += btnResetBookFilters_Click;
            // 
            // btnApplyBookFilters
            // 
            btnApplyBookFilters.Location = new Point(567, 121);
            btnApplyBookFilters.Name = "btnApplyBookFilters";
            btnApplyBookFilters.Size = new Size(150, 23);
            btnApplyBookFilters.TabIndex = 1;
            btnApplyBookFilters.Text = "Применить фильтры";
            btnApplyBookFilters.UseVisualStyleBackColor = true;
            btnApplyBookFilters.Click += btnApplyBookFilters_Click;
            // 
            // dgvBooks
            // 
            dgvBooks.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBooks.Location = new Point(3, 281);
            dgvBooks.Name = "dgvBooks";
            dgvBooks.ReadOnly = true;
            dgvBooks.Size = new Size(777, 256);
            dgvBooks.TabIndex = 0;
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
            tabPage3.Text = "Добавить Читателя";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnAddReader
            // 
            btnAddReader.Location = new Point(323, 425);
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
            btnReturnBook.Location = new Point(301, 502);
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
            btnIssueBook.Location = new Point(340, 400);
            btnIssueBook.Name = "btnIssueBook";
            btnIssueBook.Size = new Size(109, 23);
            btnIssueBook.TabIndex = 6;
            btnIssueBook.Text = "Выдать книгу";
            btnIssueBook.UseVisualStyleBackColor = true;
            btnIssueBook.Click += btnIssueBook_Click;
            // 
            // numDays
            // 
            numDays.Location = new Point(221, 308);
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
            cmbReaders.Location = new Point(221, 236);
            cmbReaders.Name = "cmbReaders";
            cmbReaders.Size = new Size(358, 23);
            cmbReaders.TabIndex = 4;
            // 
            // txtInvNumber
            // 
            txtInvNumber.Location = new Point(221, 165);
            txtInvNumber.Name = "txtInvNumber";
            txtInvNumber.Size = new Size(358, 23);
            txtInvNumber.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(76, 316);
            label3.Name = "label3";
            label3.Size = new Size(75, 15);
            label3.TabIndex = 2;
            label3.Text = "Срок (дней):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(91, 244);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 1;
            label2.Text = "Читатель:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(54, 173);
            label1.Name = "label1";
            label1.Size = new Size(125, 15);
            label1.TabIndex = 0;
            label1.Text = "Инвентарный номер:";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPageBooks);
            tabControl1.Controls.Add(tabPageReadersList);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(784, 561);
            tabControl1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(tabControl1);
            Name = "MainForm";
            Text = "Vavilon";
            tabPageReadersList.ResumeLayout(false);
            tabPageReadersList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReaders).EndInit();
            tabPageBooks.ResumeLayout(false);
            tabPageBooks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFilterYearTo).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFilterYearFrom).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLog).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLoans).EndInit();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDays).EndInit();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabPage tabPageReadersList;
        private TextBox txtFilterAuthor;
        private TabPage tabPageBooks;
        private DataGridView dgvBooks;
        private TabPage tabPage4;
        private DataGridView dgvLog;
        private TabPage tabPage3;
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
        private TabPage tabPage2;
        private Button btnReturnBook;
        private DataGridView dgvLoans;
        private TabPage tabPage1;
        private Button btnIssueBook;
        private NumericUpDown numDays;
        private ComboBox cmbReaders;
        private TextBox txtInvNumber;
        private Label label3;
        private Label label2;
        private Label label1;
        private TabControl tabControl1;
        private TextBox txtFilterFIO;
        private DataGridView dgvReaders;
        private Button btnResetReaderFilters;
        private Button btnApplyReaderFilters;
        private DateTimePicker dtpFilterRegTo;
        private DateTimePicker dtpFilterRegFrom;
        private CheckBox chkRegDateEnabled;
        private TextBox txtFilterAddress;
        private TextBox txtFilterPhone;
        private NumericUpDown numFilterYearTo;
        private NumericUpDown numFilterYearFrom;
        private TextBox txtFilterPublisher;
        private TextBox txtFilterTitle;
        private Button btnResetBookFilters;
        private Button btnApplyBookFilters;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label18;
    }
}