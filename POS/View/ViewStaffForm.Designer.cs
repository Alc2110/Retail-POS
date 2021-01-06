namespace POS.View
{
    partial class ViewStaffForm
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
            this.listView_staff = new System.Windows.Forms.ListView();
            this.button_deleteSelectedStaff = new System.Windows.Forms.Button();
            this.button_addNewStaff = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.circularProgressBar1 = new CircularProgressBar.CircularProgressBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_staff
            // 
            this.listView_staff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_staff.HideSelection = false;
            this.listView_staff.Location = new System.Drawing.Point(3, 16);
            this.listView_staff.Name = "listView_staff";
            this.listView_staff.Size = new System.Drawing.Size(673, 381);
            this.listView_staff.TabIndex = 0;
            this.listView_staff.UseCompatibleStateImageBehavior = false;
            this.listView_staff.SelectedIndexChanged += new System.EventHandler(this.listView_staff_SelectedIndexChanged);
            // 
            // button_deleteSelectedStaff
            // 
            this.button_deleteSelectedStaff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_deleteSelectedStaff.Location = new System.Drawing.Point(116, 415);
            this.button_deleteSelectedStaff.Name = "button_deleteSelectedStaff";
            this.button_deleteSelectedStaff.Size = new System.Drawing.Size(98, 23);
            this.button_deleteSelectedStaff.TabIndex = 1;
            this.button_deleteSelectedStaff.Text = "Delete selected";
            this.button_deleteSelectedStaff.UseVisualStyleBackColor = true;
            this.button_deleteSelectedStaff.Click += new System.EventHandler(this.button_deleteSelectedStaff_Click);
            // 
            // button_addNewStaff
            // 
            this.button_addNewStaff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_addNewStaff.Location = new System.Drawing.Point(12, 415);
            this.button_addNewStaff.Name = "button_addNewStaff";
            this.button_addNewStaff.Size = new System.Drawing.Size(98, 23);
            this.button_addNewStaff.TabIndex = 2;
            this.button_addNewStaff.Text = "Add new";
            this.button_addNewStaff.UseVisualStyleBackColor = true;
            this.button_addNewStaff.Click += new System.EventHandler(this.button_addNewStaff_Click);
            // 
            // button_close
            // 
            this.button_close.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_close.Location = new System.Drawing.Point(593, 415);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(98, 23);
            this.button_close.TabIndex = 3;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.circularProgressBar1);
            this.groupBox1.Controls.Add(this.listView_staff);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(679, 400);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Staff records";
            // 
            // circularProgressBar1
            // 
            this.circularProgressBar1.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.circularProgressBar1.AnimationSpeed = 500;
            this.circularProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar1.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.circularProgressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.circularProgressBar1.InnerColor = System.Drawing.Color.Transparent;
            this.circularProgressBar1.InnerMargin = 2;
            this.circularProgressBar1.InnerWidth = -1;
            this.circularProgressBar1.Location = new System.Drawing.Point(222, 92);
            this.circularProgressBar1.MarqueeAnimationSpeed = 1000;
            this.circularProgressBar1.Name = "circularProgressBar1";
            this.circularProgressBar1.OuterColor = System.Drawing.Color.Gray;
            this.circularProgressBar1.OuterMargin = -25;
            this.circularProgressBar1.OuterWidth = 26;
            this.circularProgressBar1.ProgressColor = System.Drawing.Color.Lime;
            this.circularProgressBar1.ProgressWidth = 25;
            this.circularProgressBar1.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.circularProgressBar1.Size = new System.Drawing.Size(235, 217);
            this.circularProgressBar1.StartAngle = 270;
            this.circularProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.circularProgressBar1.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.circularProgressBar1.SubscriptText = "";
            this.circularProgressBar1.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.circularProgressBar1.SuperscriptText = "";
            this.circularProgressBar1.TabIndex = 2;
            this.circularProgressBar1.Text = "Loading";
            this.circularProgressBar1.TextMargin = new System.Windows.Forms.Padding(0);
            this.circularProgressBar1.Value = 68;
            // 
            // ViewStaffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_addNewStaff);
            this.Controls.Add(this.button_deleteSelectedStaff);
            this.Name = "ViewStaffForm";
            this.Text = "Staff";
            this.Load += new System.EventHandler(this.ViewStaffForm_Load);
            this.Resize += new System.EventHandler(this.ViewStaffForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_staff;
        private System.Windows.Forms.Button button_deleteSelectedStaff;
        private System.Windows.Forms.Button button_addNewStaff;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.GroupBox groupBox1;
        private CircularProgressBar.CircularProgressBar circularProgressBar1;
    }
}