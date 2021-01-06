namespace POS.View
{
    partial class DiscountForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_specifyPrice = new System.Windows.Forms.TextBox();
            this.textBox_amountDiscount = new System.Windows.Forms.TextBox();
            this.trackBar_percentDiscount = new System.Windows.Forms.TrackBar();
            this.radioButton_specifyPrice = new System.Windows.Forms.RadioButton();
            this.radioButton_dollarAmount = new System.Windows.Forms.RadioButton();
            this.radioButton_percentage = new System.Windows.Forms.RadioButton();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_apply = new System.Windows.Forms.Button();
            this.label_percentage = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_percentDiscount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_percentage);
            this.groupBox1.Controls.Add(this.textBox_specifyPrice);
            this.groupBox1.Controls.Add(this.textBox_amountDiscount);
            this.groupBox1.Controls.Add(this.trackBar_percentDiscount);
            this.groupBox1.Controls.Add(this.radioButton_specifyPrice);
            this.groupBox1.Controls.Add(this.radioButton_dollarAmount);
            this.groupBox1.Controls.Add(this.radioButton_percentage);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 205);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Discount";
            // 
            // textBox_specifyPrice
            // 
            this.textBox_specifyPrice.Location = new System.Drawing.Point(97, 169);
            this.textBox_specifyPrice.Name = "textBox_specifyPrice";
            this.textBox_specifyPrice.Size = new System.Drawing.Size(273, 20);
            this.textBox_specifyPrice.TabIndex = 5;
            this.textBox_specifyPrice.TextChanged += new System.EventHandler(this.textBox_specifyPrice_TextChanged);
            // 
            // textBox_amountDiscount
            // 
            this.textBox_amountDiscount.Location = new System.Drawing.Point(97, 80);
            this.textBox_amountDiscount.Name = "textBox_amountDiscount";
            this.textBox_amountDiscount.Size = new System.Drawing.Size(273, 20);
            this.textBox_amountDiscount.TabIndex = 4;
            this.textBox_amountDiscount.TextChanged += new System.EventHandler(this.textBox_amountDiscount_TextChanged);
            // 
            // trackBar_percentDiscount
            // 
            this.trackBar_percentDiscount.Location = new System.Drawing.Point(97, 29);
            this.trackBar_percentDiscount.Name = "trackBar_percentDiscount";
            this.trackBar_percentDiscount.Size = new System.Drawing.Size(223, 45);
            this.trackBar_percentDiscount.TabIndex = 3;
            this.trackBar_percentDiscount.ValueChanged += new System.EventHandler(this.trackBar_percentDiscount_ValueChanged);
            // 
            // radioButton_specifyPrice
            // 
            this.radioButton_specifyPrice.AutoSize = true;
            this.radioButton_specifyPrice.Location = new System.Drawing.Point(6, 169);
            this.radioButton_specifyPrice.Name = "radioButton_specifyPrice";
            this.radioButton_specifyPrice.Size = new System.Drawing.Size(86, 17);
            this.radioButton_specifyPrice.TabIndex = 2;
            this.radioButton_specifyPrice.TabStop = true;
            this.radioButton_specifyPrice.Text = "Specify price";
            this.radioButton_specifyPrice.UseVisualStyleBackColor = true;
            this.radioButton_specifyPrice.CheckedChanged += new System.EventHandler(this.radioButton_specifyPrice_CheckedChanged);
            // 
            // radioButton_dollarAmount
            // 
            this.radioButton_dollarAmount.AutoSize = true;
            this.radioButton_dollarAmount.Location = new System.Drawing.Point(6, 81);
            this.radioButton_dollarAmount.Name = "radioButton_dollarAmount";
            this.radioButton_dollarAmount.Size = new System.Drawing.Size(90, 17);
            this.radioButton_dollarAmount.TabIndex = 1;
            this.radioButton_dollarAmount.TabStop = true;
            this.radioButton_dollarAmount.Text = "Dollar amount";
            this.radioButton_dollarAmount.UseVisualStyleBackColor = true;
            this.radioButton_dollarAmount.CheckedChanged += new System.EventHandler(this.radioButton_dollarAmount_CheckedChanged);
            // 
            // radioButton_percentage
            // 
            this.radioButton_percentage.AutoSize = true;
            this.radioButton_percentage.Location = new System.Drawing.Point(6, 29);
            this.radioButton_percentage.Name = "radioButton_percentage";
            this.radioButton_percentage.Size = new System.Drawing.Size(80, 17);
            this.radioButton_percentage.TabIndex = 0;
            this.radioButton_percentage.TabStop = true;
            this.radioButton_percentage.Text = "Percentage";
            this.radioButton_percentage.UseVisualStyleBackColor = true;
            this.radioButton_percentage.CheckedChanged += new System.EventHandler(this.radioButton_percentage_CheckedChanged);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(151, 223);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(232, 223);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(313, 223);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 3;
            this.button_apply.Text = "Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // label_percentage
            // 
            this.label_percentage.AutoSize = true;
            this.label_percentage.Location = new System.Drawing.Point(335, 33);
            this.label_percentage.Name = "label_percentage";
            this.label_percentage.Size = new System.Drawing.Size(21, 13);
            this.label_percentage.TabIndex = 6;
            this.label_percentage.Text = "0%";
            // 
            // DiscountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 254);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiscountForm";
            this.Text = "Discount";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_percentDiscount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.RadioButton radioButton_specifyPrice;
        private System.Windows.Forms.RadioButton radioButton_dollarAmount;
        private System.Windows.Forms.RadioButton radioButton_percentage;
        private System.Windows.Forms.TextBox textBox_specifyPrice;
        private System.Windows.Forms.TextBox textBox_amountDiscount;
        private System.Windows.Forms.TrackBar trackBar_percentDiscount;
        private System.Windows.Forms.Label label_percentage;
    }
}