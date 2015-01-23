using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    /// <summary>
    /// Klasa odpowiedzialna za główne okno aplikacji
    /// </summary>
    public partial class MainWindow : Form
    {
        private StringBuilder sb;
        public CommunicationModule cm;
        public bool isChatOpen = false;
        //public LoginWindow lw;

        /// <summary>
        /// Konstrukto klasy MainWindow
        /// </summary>
        public MainWindow()
        {
            //this.lw = lw;
            cm = new CommunicationModule();
            cm.msgSignal += new MsgSignal(MsgService);
            sb = new StringBuilder();
            InitializeComponent();
            EnableDisableControls(false);
        }
        private void MsgService(object sender, MsgEvent e)
        {
            
        }
        /// <summary>
        /// Metoda odpowiedzialna za procedurę połączenia z innym użytkownikiem
        /// </summary>
        private void ConnectMethod() 
        {
            if (isChatOpen == true)
            {
                WriteInLog("Can't open another session");
                return;
            }
            if (ConInputTextBox.Text.Equals("")) //Pusty TextBox
                return;
            string login = ConInputTextBox.Text;
            if (login.Equals(BindingModule.myLogin))
            {
                WriteInLog("Can't talk with yourself");
                ConInputTextBox.ResetText();
                return;
            }
            sb.Clear();
            sb.Append("Trying to connect with: ").Append(login);
            string msg = sb.ToString();
            WriteInLog(msg);
            ConInputTextBox.Clear();
            //Metoda send i listen czekajaca na odpowiedz,czy user isOnline -> 
            //zawieszenie dzialania programu, głownego okna
            //Put Code Here, przeslanie potwierdzenia 
            bool isOnline = true;
            if (!isOnline)
            {
                sb.Append("Connection attempt with: ").Append(login).
                    Append(" failed, User is offline");
                msg = sb.ToString();
                WriteInLog(msg);
                return;
            }
            else
            {
                sb.Append("Connection attempt with: ").Append(login).
                   Append(" succeed");
                msg = sb.ToString();
                WriteInLog(msg);
                sb.Append("Waiting for certificate for ").Append(login);
                msg = sb.ToString();
                WriteInLog(msg);

                //Odebranie certyfikatu i utworzenie certyfikatu:
                //Odkomentowac:
                //byte[] certRawData;
                //X509Certificate2 cert = CryptoModule.CreatePublicCertFromRawData(certRawData);
                //CryptoModule.ImportKey(cert, false, false);

                sb.Append("Received certicate for ").Append(login).
                    Append(". Starting chat now");
                msg = sb.ToString();
                WriteInLog(msg);
                //Wyswietlenie okna czatu
                ChatWindow chat = new ChatWindow(login, this);
                chat.Show();
                //BindingModule.AddChat(login, this);
            }
        }
        /// <summary>
        /// Metoda obsługująca zdarzenie kliknięcia w przycisk ConnectBtn
        /// W szczególności wywoływana jest metoda ConnectMethod()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            ConnectMethod();            
        }

        /// <summary>
        /// Metoda wyświetlająca komunikat w Log
        /// </summary>
        /// <param name="msg">
        /// Komunikat do wyświetlenia
        /// </param>
        public void WriteInLog(string msg)
        {
            sb.Clear();
            sb.Append(DateTime.Now.ToString()).Append(" ").Append(msg);
            Log.AppendText(sb.ToString());
            sb.Clear();
            Log.AppendText("\n");
        }

        /// <summary>
        /// Metoda włączająca/wyłączajaca kontrolki (przyciski, textbox)
        /// </summary>
        /// <param name="enabled">
        /// True - włączone
        /// False - wyłączone
        /// </param>
        internal void EnableDisableControls(bool enabled)
        {
            this.ConnectBtn.Enabled = enabled;
            this.ConInputTextBox.ReadOnly = (!enabled);
            this.OnlineBtn.Enabled = enabled;
        
        }
        
        /// <summary>
        /// Metoda obsługująca zdarzenie ładowania komponentu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            ConnectBtnToolTip.SetToolTip(this.ConnectBtn, "Press to connect");
            OnlineBtnToolTip.SetToolTip(this.OnlineBtn, "Check who's online");
            WriteInLog("Program running and ready");
        }

        private void button1_Click(object sender, EventArgs e)//onlinebutton
        {
            
            //send request, wait for response
            string onlinelist = "User1 User2 User3";
            onlinelist = onlinelist.Replace(' ', '\n');
            WriteInLog("Online:\n" + onlinelist);
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie wciśnięcia klawisza w ConInputTextBox
        /// W szczególności wciśnięcie klawisza Enter powoduje wywołanie metody ConnectMethod()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                ConnectMethod();
            }
        }

        /// <summary>
        /// Metoda wyłączająca przycisk LoginBtn
        /// </summary>
        internal void DisableLogBtn() 
        {
            this.LogBtn.Enabled = false;
        }

        /// <summary>
        /// Metoda obsługująca zdarzenie kliknięcia w przycisk LogBtn
        /// W szczególności wywołuje ona LoginWindow co pozwala na przeprowadzenie procedury logowania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogBtn_Click(object sender, EventArgs e)
        {
            //EnableDisableControls(false);
            new LoginWindow(this).Show();
        }
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            cm.StopListening();
            cm.Stop();
        }

    }
}
