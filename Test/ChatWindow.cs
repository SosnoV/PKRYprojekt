using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    /// <summary>
    /// Klasa definiująca okno czat
    /// </summary>
    public partial class ChatWindow : Form
    {
        //odwołanie do CommunicationModule: mw.lw.cm
        Encoding enc = Encoding.UTF8;
        StringBuilder sb;
        private bool areCharsOver; //czy wiadomosc ma ponad 160 znakow
        /// <summary>
        /// Login użtykownika z którym prowadzona jest rozmowa
        /// </summary>
        private string otherUser;
        private MainWindow mw;
        private bool isDisconnected;

        /// <summary>
        /// Konstruktor klasy ChatWindow
        /// 
        /// </summary>
        /// <param name="login"> Login użytkownika, z którym trwa sesja komunikacyjna</param>
        /// <param name="mw"></param>
        public ChatWindow(string login, MainWindow mw)
        {
            sb = new StringBuilder();
            this.mw = mw;
            otherUser = login;
            InitializeComponent();
            areCharsOver = true;
            SendBtn.Enabled = false;
            isDisconnected = false;
        }

        /// <summary>
        /// Metoda wyświetlająca wiadomość napisaną przez użytkownika
        /// </summary>
        /// <param name="msg"></param>
        public void WriteInLogAsThisUser(string msg)
        {
            sb.Clear();
            sb.Append(DateTime.Now.ToLongTimeString()).Append(" ").
                Append(BindingModule.myLogin).Append("\n").
                Append(msg);
            ChatLog.AppendText(sb.ToString());
            sb.Clear();
            ChatLog.AppendText("\n");
        }
        /// <summary>
        /// Metoda wyświetlająca tekst w oknie czatu
        /// </summary>
        /// <param name="msg"></param>
        public void WriteInLog(string msg)
        {
            sb.Clear();
            sb.Append(DateTime.Now.ToLongTimeString()).Append(" ").Append(msg);
            ChatLog.AppendText(sb.ToString());
            sb.Clear();
            ChatLog.AppendText("\n");
        }

        /// <summary>
        /// Metoda wyświetlająca wiadomość od innego użytkownika
        /// </summary>
        /// <param name="msg"></param>
        public void WriteInLogAsOtherUser(string msg)
        {
            sb.Clear();
            sb.Append(DateTime.Now.ToLongTimeString()).Append(" ").
                Append(otherUser).Append(" ").
                Append(msg);
            ChatLog.AppendText(sb.ToString());
            sb.Clear();
            ChatLog.AppendText("\n");
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie Click wygenerowane przez przycisk SendBtn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendBtn_Click(object sender, EventArgs e)
        {
            TextToSendMethod();
        }

        /// <summary>
        /// Metoda wysyłająca wiadomość użytkownika
        /// </summary>
        private void TextToSendMethod() 
        {
            if (!areCharsOver)
            {
                string msg = MsgTextBox.Text;
                //byte[] encryptedMsg = CryptoModule.EncryptMsg(enc.GetBytes(msg), otherUser);
                //byte[] signedMsg = CryptoModule.Sign(msg);
                WriteInLogAsThisUser(msg);
                MsgTextBox.ResetText();
            }
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MsgTextBox_TextChanged(object sender, EventArgs e)
        {
            if (MsgTextBox.Text.Length > 160 || MsgTextBox.Text.Length == 0)
            {
                areCharsOver = true;
                SendBtn.Enabled = false;
                SendBtn.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                areCharsOver = false;
                SendBtn.Enabled = true;
                SendBtn.BackColor = System.Drawing.Color.Green;
            }

            sb.Append(MsgTextBox.Text.Length).Append("/160");
            CharCount.Text = sb.ToString();
            sb.Clear();
        }

        //Uzupelnic o kod do wysylania
        private void send()
        {
            string msg = MsgTextBox.Text;
            byte[] encryptedMsg = CryptoModule.EncryptMsg(enc.GetBytes(msg), otherUser);
            byte[] signedMsg = CryptoModule.Sign(msg);
        }
        /// <summary>
        /// Metoda obsługująca zdarzenie zamykania okna czatu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            BindingModule.RemoveUserChatAndKey(otherUser);
            mw.WriteInLog("Ended chat session with: " + otherUser);
        }
        /// <summary>
        /// Metoda obsługująca zdarzenie Click wygenerowane przez przycisk DisconnectBtn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            if (!isDisconnected)
            {
                //Disconnect
                SendBtn.Enabled = false;
                MsgTextBox.Enabled = false;
                isDisconnected = true;
                WriteInLog("Disconnected. Chat terminated");
            }
        }
        /// <summary>
        /// Metoda obsługująca zdarzenie ładowania się okna czatu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatWindow_Load(object sender, EventArgs e)
        {
            this.SendBtnToolTip.SetToolTip(this.SendBtn, "Click to send message");
            this.DiscBtnToolTip.SetToolTip(this.DisconnectBtn, "Click to disconnect");
            WriteInLog("You are now talking with " + otherUser);
            
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie KeyPress wygenerowane przez MsgTextBox
        /// Powoduje wysłanie wiadomości w przypadku kliknięcia Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MsgTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                TextToSendMethod();
            } 
        }

       
    }
}
