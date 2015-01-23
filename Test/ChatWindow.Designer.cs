namespace Test
{
    partial class ChatWindow
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
            this.ChatLog = new System.Windows.Forms.RichTextBox();
            this.MsgTextBox = new System.Windows.Forms.TextBox();
            this.SendBtn = new System.Windows.Forms.Button();
            this.CharCount = new System.Windows.Forms.Label();
            this.DisconnectBtn = new System.Windows.Forms.Button();
            this.SendBtnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.DiscBtnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // ChatLog
            // 
            this.ChatLog.BackColor = System.Drawing.Color.Black;
            this.ChatLog.ForeColor = System.Drawing.Color.Lime;
            this.ChatLog.Location = new System.Drawing.Point(2, 1);
            this.ChatLog.Name = "ChatLog";
            this.ChatLog.ReadOnly = true;
            this.ChatLog.Size = new System.Drawing.Size(281, 177);
            this.ChatLog.TabIndex = 1;
            this.ChatLog.Text = "";
            // 
            // MsgTextBox
            // 
            this.MsgTextBox.Location = new System.Drawing.Point(2, 184);
            this.MsgTextBox.Multiline = true;
            this.MsgTextBox.Name = "MsgTextBox";
            this.MsgTextBox.Size = new System.Drawing.Size(200, 57);
            this.MsgTextBox.TabIndex = 0;
            this.MsgTextBox.TextChanged += new System.EventHandler(this.MsgTextBox_TextChanged);
            this.MsgTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MsgTextBox_KeyPress);
            // 
            // SendBtn
            // 
            this.SendBtn.BackColor = System.Drawing.Color.Red;
            this.SendBtn.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SendBtn.Location = new System.Drawing.Point(208, 184);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(75, 57);
            this.SendBtn.TabIndex = 2;
            this.SendBtn.Text = "SEND";
            this.SendBtn.UseVisualStyleBackColor = false;
            this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
            // 
            // CharCount
            // 
            this.CharCount.AutoSize = true;
            this.CharCount.Location = new System.Drawing.Point(12, 244);
            this.CharCount.Name = "CharCount";
            this.CharCount.Size = new System.Drawing.Size(0, 13);
            this.CharCount.TabIndex = 3;
            // 
            // DisconnectBtn
            // 
            this.DisconnectBtn.Location = new System.Drawing.Point(208, 239);
            this.DisconnectBtn.Name = "DisconnectBtn";
            this.DisconnectBtn.Size = new System.Drawing.Size(75, 23);
            this.DisconnectBtn.TabIndex = 4;
            this.DisconnectBtn.Text = "END CHAT";
            this.DisconnectBtn.UseVisualStyleBackColor = true;
            this.DisconnectBtn.Click += new System.EventHandler(this.DisconnectBtn_Click);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.DisconnectBtn);
            this.Controls.Add(this.CharCount);
            this.Controls.Add(this.SendBtn);
            this.Controls.Add(this.MsgTextBox);
            this.Controls.Add(this.ChatLog);
            this.DoubleBuffered = true;
            this.Name = "ChatWindow";
            this.Text = "ChatWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatWindow_FormClosing);
            this.Load += new System.EventHandler(this.ChatWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ChatLog;
        private System.Windows.Forms.TextBox MsgTextBox;
        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.Label CharCount;
        private System.Windows.Forms.Button DisconnectBtn;
        private System.Windows.Forms.ToolTip SendBtnToolTip;
        private System.Windows.Forms.ToolTip DiscBtnToolTip;
    }
}