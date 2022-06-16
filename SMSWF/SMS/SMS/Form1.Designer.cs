namespace SMS
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sdt = new System.Windows.Forms.Label();
            this.conten = new System.Windows.Forms.Label();
            this.txtSdt = new System.Windows.Forms.TextBox();
            this.txtConten = new System.Windows.Forms.TextBox();
            this.btnGui = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sdt
            // 
            this.sdt.AutoSize = true;
            this.sdt.Location = new System.Drawing.Point(50, 46);
            this.sdt.Name = "sdt";
            this.sdt.Size = new System.Drawing.Size(27, 15);
            this.sdt.TabIndex = 0;
            this.sdt.Text = "SĐT";
            // 
            // conten
            // 
            this.conten.AutoSize = true;
            this.conten.Location = new System.Drawing.Point(35, 138);
            this.conten.Name = "conten";
            this.conten.Size = new System.Drawing.Size(57, 15);
            this.conten.TabIndex = 1;
            this.conten.Text = "Nội dung";
            // 
            // txtSdt
            // 
            this.txtSdt.Location = new System.Drawing.Point(108, 38);
            this.txtSdt.Name = "txtSdt";
            this.txtSdt.Size = new System.Drawing.Size(204, 23);
            this.txtSdt.TabIndex = 2;
            // 
            // txtConten
            // 
            this.txtConten.Location = new System.Drawing.Point(108, 138);
            this.txtConten.Name = "txtConten";
            this.txtConten.Size = new System.Drawing.Size(204, 23);
            this.txtConten.TabIndex = 3;
            // 
            // btnGui
            // 
            this.btnGui.Location = new System.Drawing.Point(99, 209);
            this.btnGui.Name = "btnGui";
            this.btnGui.Size = new System.Drawing.Size(129, 34);
            this.btnGui.TabIndex = 4;
            this.btnGui.Text = "Gửi";
            this.btnGui.UseVisualStyleBackColor = true;
            this.btnGui.Click += new System.EventHandler(this.btnGui_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 401);
            this.Controls.Add(this.btnGui);
            this.Controls.Add(this.txtConten);
            this.Controls.Add(this.txtSdt);
            this.Controls.Add(this.conten);
            this.Controls.Add(this.sdt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label sdt;
        private Label conten;
        private TextBox txtSdt;
        private TextBox txtConten;
        private Button btnGui;
    }
}