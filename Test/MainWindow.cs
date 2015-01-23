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
    public partial class MainWindow : Form
    {
        private StringBuilder sb;
        public LoginWindow lw;

        public MainWindow(LoginWindow lw)
        {
            this.lw = lw;
            sb = new StringBuilder();
            InitializeComponent();
        }

        private void ConnectMethod() 
        {
            if (ConInputTextBox.Text.Equals("")) //Pusty TextBox
                return;
            string login = ConInputTextBox.Text;
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
                BindingModule.AddChat(login, this);
            }
        }
        //Obsluga eventu klikniecia w Connect Button
        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            ConnectMethod();            
        }

        //Metoda do wyswietlania stringow w RichTextBox Log
        public void WriteInLog(string msg)
        {
            sb.Clear();
            sb.Append(DateTime.Now.ToString()).Append(" ").Append(msg);
            Log.AppendText(sb.ToString());
            sb.Clear();
            Log.AppendText("\n");
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            lw.Close();
        }

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

        private void ConInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                ConnectMethod();
            }
        }

    }
}
