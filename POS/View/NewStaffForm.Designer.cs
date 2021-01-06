namespace POS.View
{
    partial class NewStaffForm
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
            this.textBox_fullName = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.textBox_repeatPassword = new System.Windows.Forms.TextBox();
            this.comboBox_privelege = new System.Windows.Forms.ComboBox();
            this.button_addStaff = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelProgressBar1 = new labelProgressBarControl.LabelProgressBar();
            this.SuspendLayout();
            // 
            // textBox_fullName
            // 
            this.textBox_fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fullName.Location = new System.Drawing.Point(116, 29);
            this.textBox_fullName.Name = "textBox_fullName";
            this.textBox_fullName.Size = new System.Drawing.Size(672, 20);
            this.textBox_fullName.TabIndex = 0;
            // 
            // textBox_password
            // 
            this.textBox_password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_password.Location = new System.Drawing.Point(116, 55);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(672, 20);
            this.textBox_password.TabIndex = 1;
            // 
            // textBox_repeatPassword
            // 
            this.textBox_repeatPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_repeatPassword.Location = new System.Drawing.Point(116, 81);
            this.textBox_repeatPassword.Name = "textBox_repeatPassword";
            this.textBox_repeatPassword.Size = new System.Drawing.Size(672, 20);
            this.textBox_repeatPassword.TabIndex = 2;
            // 
            // comboBox_privelege
            // 
            this.comboBox_privelege.FormattingEnabled = true;
            this.comboBox_privelege.Location = new System.Drawing.Point(116, 107);
            this.comboBox_privelege.Name = "comboBox_privelege";
            this.comboBox_privelege.Size = new System.Drawing.Size(142, 21);
            this.comboBox_privelege.TabIndex = 3;
            // 
            // button_addStaff
            // 
            this.button_addStaff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_addStaff.Location = new System.Drawing.Point(116, 219);
            this.button_addStaff.Name = "button_addStaff";
            this.button_addStaff.Size = new System.Drawing.Size(142, 23);
            this.button_addStaff.TabIndex = 4;
            this.button_addStaff.Text = "Add";
            this.button_addStaff.UseVisualStyleBackColor = true;
            this.button_addStaff.Click += new System.EventHandler(this.button_addStaff_Click);
            // 
            // button_close
            // 
            this.button_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_close.Location = new System.Drawing.Point(646, 219);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(142, 23);
            this.button_close.TabIndex = 5;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Full name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Repeat password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Privelege:";
            // 
            // labelProgressBar1
            // 
            this.labelProgressBar1.customText = null;
            this.labelProgressBar1.displayStyle = labelProgressBarControl.LabelProgressBarText.PERCENTAGE;
            this.labelProgressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProgressBar1.Location = new System.Drawing.Point(0, 0);
            this.labelProgressBar1.Name = "labelProgressBar1";
            this.labelProgressBar1.Size = new System.Drawing.Size(800, 26);
            this.labelProgressBar1.TabIndex = 10;
            // 
            // NewStaffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 254);
            this.Controls.Add(this.labelProgressBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_addStaff);
            this.Controls.Add(this.comboBox_privelege);
            this.Controls.Add(this.textBox_repeatPassword);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_fullName);
            this.Name = "NewStaffForm";
            this.Text = "Add staff member";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_fullName;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_repeatPassword;
        private System.Windows.Forms.ComboBox comboBox_privelege;
        private System.Windows.Forms.Button button_addStaff;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private labelProgressBarControl.LabelProgressBar labelProgressBar1;
    }
}