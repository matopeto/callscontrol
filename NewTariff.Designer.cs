namespace MinutesCounter
{
    partial class NewTariff
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
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.lblName = new System.Windows.Forms.Label();
            this.tariffName = new System.Windows.Forms.TextBox();
            this.tariffMonthlyPay = new System.Windows.Forms.TextBox();
            this.lblMonthlyPay = new System.Windows.Forms.Label();
            this.tariffCurrency = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "done";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Cancel";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(3, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(157, 18);
            this.lblName.Text = "Tariff Name";
            // 
            // tariffName
            // 
            this.tariffName.Location = new System.Drawing.Point(3, 33);
            this.tariffName.Name = "tariffName";
            this.tariffName.Size = new System.Drawing.Size(157, 22);
            this.tariffName.TabIndex = 1;
            // 
            // tariffMonthlyPay
            // 
            this.tariffMonthlyPay.Location = new System.Drawing.Point(3, 90);
            this.tariffMonthlyPay.Name = "tariffMonthlyPay";
            this.tariffMonthlyPay.Size = new System.Drawing.Size(157, 22);
            this.tariffMonthlyPay.TabIndex = 3;
            // 
            // lblMonthlyPay
            // 
            this.lblMonthlyPay.Location = new System.Drawing.Point(3, 69);
            this.lblMonthlyPay.Name = "lblMonthlyPay";
            this.lblMonthlyPay.Size = new System.Drawing.Size(170, 18);
            this.lblMonthlyPay.Text = "Monthly pay";
            // 
            // tariffCurrency
            // 
            this.tariffCurrency.Location = new System.Drawing.Point(3, 144);
            this.tariffCurrency.Name = "tariffCurrency";
            this.tariffCurrency.Size = new System.Drawing.Size(157, 22);
            this.tariffCurrency.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 18);
            this.label2.Text = "Currency";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 298);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(157, 22);
            this.textBox1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 18);
            this.label1.Text = "Currency";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(3, 244);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(157, 22);
            this.textBox2.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 18);
            this.label3.Text = "Monthly pay";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(3, 190);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(157, 22);
            this.textBox3.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 18);
            this.label4.Text = "Tariff Name";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.tariffName);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(170, 63);
            // 
            // NewTariff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(0, 50);
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(176, 180);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tariffCurrency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tariffMonthlyPay);
            this.Controls.Add(this.lblMonthlyPay);
            this.Menu = this.mainMenu1;
            this.Name = "NewTariff";
            this.Text = "New Tariff";
            this.Load += new System.EventHandler(this.NewTariff_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tariffName;
        private System.Windows.Forms.TextBox tariffMonthlyPay;
        private System.Windows.Forms.Label lblMonthlyPay;
        private System.Windows.Forms.TextBox tariffCurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.Panel panel1;
    }
}