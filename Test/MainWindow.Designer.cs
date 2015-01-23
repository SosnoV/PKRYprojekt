namespace Test
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.ConInputTextBox = new System.Windows.Forms.TextBox();
            this.Log = new System.Windows.Forms.RichTextBox();
            this.ConnectBtnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.OnlineBtn = new System.Windows.Forms.Button();
            this.OnlineBtnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LogBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Location = new System.Drawing.Point(81, 38);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ConnectBtn.Size = new System.Drawing.Size(79, 23);
            this.ConnectBtn.TabIndex = 1;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // ConInputTextBox
            // 
            this.ConInputTextBox.Location = new System.Drawing.Point(12, 12);
            this.ConInputTextBox.Name = "ConInputTextBox";
            this.ConInputTextBox.Size = new System.Drawing.Size(148, 20);
            this.ConInputTextBox.TabIndex = 0;
            this.ConInputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ConInputTextBox_KeyPress);
            // 
            // Log
            // 
            this.Log.BackColor = System.Drawing.Color.Black;
            this.Log.ForeColor = System.Drawing.Color.Lime;
            this.Log.Location = new System.Drawing.Point(12, 67);
            this.Log.Name = "Log";
            this.Log.ReadOnly = true;
            this.Log.Size = new System.Drawing.Size(333, 223);
            this.Log.TabIndex = 2;
            this.Log.Text = "";
            // 
            // OnlineBtn
            // 
            this.OnlineBtn.Location = new System.Drawing.Point(12, 38);
            this.OnlineBtn.Name = "OnlineBtn";
            this.OnlineBtn.Size = new System.Drawing.Size(59, 23);
            this.OnlineBtn.TabIndex = 3;
            this.OnlineBtn.Text = "Online";
            this.OnlineBtn.UseVisualStyleBackColor = true;
            this.OnlineBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // LogBtn
            // 
            this.LogBtn.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LogBtn.Location = new System.Drawing.Point(237, 12);
            this.LogBtn.Name = "LogBtn";
            this.LogBtn.Size = new System.Drawing.Size(108, 49);
            this.LogBtn.TabIndex = 4;
            this.LogBtn.Text = "LOGIN";
            this.LogBtn.UseVisualStyleBackColor = true;
            this.LogBtn.Click += new System.EventHandler(this.LogBtn_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(355, 306);
            this.Controls.Add(this.LogBtn);
            this.Controls.Add(this.OnlineBtn);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.ConInputTextBox);
            this.Controls.Add(this.ConnectBtn);
            this.Name = "MainWindow";
            this.Text = "Test";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.TextBox ConInputTextBox;
        private System.Windows.Forms.RichTextBox Log;
        private System.Windows.Forms.ToolTip ConnectBtnToolTip;
        private System.Windows.Forms.Button OnlineBtn;
        private System.Windows.Forms.ToolTip OnlineBtnToolTip;
        private System.Windows.Forms.Button LogBtn;
    }
}

