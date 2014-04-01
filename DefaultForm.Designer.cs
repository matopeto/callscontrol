namespace MinutesCounter
{
    partial class DefaultForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuCount = new System.Windows.Forms.MenuItem();
            this.menuMenu = new System.Windows.Forms.MenuItem();
            this.menuDetailed = new System.Windows.Forms.MenuItem();
            this.menuTariffInfo = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.chckLastMonth = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panelBillDay = new System.Windows.Forms.Panel();
            this.cmbBillDay = new System.Windows.Forms.ComboBox();
            this.panelBillDay.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuCount);
            this.mainMenu1.MenuItems.Add(this.menuMenu);
            // 
            // menuCount
            // 
            this.menuCount.Text = "Count";
            this.menuCount.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuMenu
            // 
            this.menuMenu.MenuItems.Add(this.menuDetailed);
            this.menuMenu.MenuItems.Add(this.menuTariffInfo);
            this.menuMenu.MenuItems.Add(this.menuAbout);
            this.menuMenu.MenuItems.Add(this.menuExit);
            this.menuMenu.Text = "Menu";
            this.menuMenu.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuDetailed
            // 
            this.menuDetailed.Text = "Detailed?";
            this.menuDetailed.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuTariffInfo
            // 
            this.menuTariffInfo.Text = "Tariff info";
            this.menuTariffInfo.Click += new System.EventHandler(this.menuItem1_Click_1);
            // 
            // menuAbout
            // 
            this.menuAbout.Text = "About";
            this.menuAbout.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuExit
            // 
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 19);
            this.label1.Text = "BillDay";
            // 
            // txtResult
            // 
            this.txtResult.AcceptsReturn = true;
            this.txtResult.AcceptsTab = true;
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(3, 70);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(234, 193);
            this.txtResult.TabIndex = 3;
            this.txtResult.WordWrap = false;
            this.txtResult.TextChanged += new System.EventHandler(this.txtResult_TextChanged);
            // 
            // chckLastMonth
            // 
            this.chckLastMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chckLastMonth.Location = new System.Drawing.Point(3, 36);
            this.chckLastMonth.Name = "chckLastMonth";
            this.chckLastMonth.Size = new System.Drawing.Size(225, 22);
            this.chckLastMonth.TabIndex = 2;
            this.chckLastMonth.Text = "Last month?";
            this.chckLastMonth.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(9, 218);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(213, 22);
            this.progressBar1.Visible = false;
            // 
            // panelBillDay
            // 
            this.panelBillDay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBillDay.Controls.Add(this.cmbBillDay);
            this.panelBillDay.Controls.Add(this.chckLastMonth);
            this.panelBillDay.Controls.Add(this.label1);
            this.panelBillDay.Location = new System.Drawing.Point(5, 3);
            this.panelBillDay.Name = "panelBillDay";
            this.panelBillDay.Size = new System.Drawing.Size(232, 61);
            this.panelBillDay.GotFocus += new System.EventHandler(this.panelBillDay_GotFocus);
            // 
            // cmbBillDay
            // 
            this.cmbBillDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBillDay.Items.Add("Please select");
            this.cmbBillDay.Items.Add("01");
            this.cmbBillDay.Items.Add("02");
            this.cmbBillDay.Items.Add("03");
            this.cmbBillDay.Items.Add("04");
            this.cmbBillDay.Items.Add("05");
            this.cmbBillDay.Items.Add("06");
            this.cmbBillDay.Items.Add("07");
            this.cmbBillDay.Items.Add("08");
            this.cmbBillDay.Items.Add("09");
            this.cmbBillDay.Items.Add("10");
            this.cmbBillDay.Items.Add("11");
            this.cmbBillDay.Items.Add("12");
            this.cmbBillDay.Items.Add("13");
            this.cmbBillDay.Items.Add("14");
            this.cmbBillDay.Items.Add("15");
            this.cmbBillDay.Items.Add("16");
            this.cmbBillDay.Items.Add("17");
            this.cmbBillDay.Items.Add("18");
            this.cmbBillDay.Items.Add("19");
            this.cmbBillDay.Items.Add("20");
            this.cmbBillDay.Items.Add("21");
            this.cmbBillDay.Items.Add("22");
            this.cmbBillDay.Items.Add("23");
            this.cmbBillDay.Items.Add("24");
            this.cmbBillDay.Items.Add("25");
            this.cmbBillDay.Items.Add("26");
            this.cmbBillDay.Items.Add("27");
            this.cmbBillDay.Items.Add("28");
            this.cmbBillDay.Items.Add("29");
            this.cmbBillDay.Items.Add("30");
            this.cmbBillDay.Items.Add("31");
            this.cmbBillDay.Items.Add("Interval");
            this.cmbBillDay.Location = new System.Drawing.Point(139, 6);
            this.cmbBillDay.Name = "cmbBillDay";
            this.cmbBillDay.Size = new System.Drawing.Size(89, 30);
            this.cmbBillDay.TabIndex = 1;
            this.cmbBillDay.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
            // 
            // DefaultForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 266);
            this.Controls.Add(this.panelBillDay);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtResult);
            this.Menu = this.mainMenu1;
            this.Name = "DefaultForm";
            this.Text = "Calls Control";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DefaultForm_Closing);
            this.panelBillDay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuCount;
        private System.Windows.Forms.MenuItem menuMenu;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuItem menuAbout;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.CheckBox chckLastMonth;
        private System.Windows.Forms.MenuItem menuDetailed;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panelBillDay;
		private System.Windows.Forms.ComboBox cmbBillDay;
		private System.Windows.Forms.MenuItem menuTariffInfo;
    }
}

