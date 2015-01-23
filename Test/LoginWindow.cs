using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    /// <summary>
    /// Klasa definiująca okno logowania
    /// </summary>
    public partial class LoginWindow : Form
    {
        /// <summary>
        /// Obiekt odpowiedzialny za komunikację między modułami
        /// </summary>
        public CommunicationModule cm;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public LoginWindow()
        {
            //odwołanie w ChatWindow: mw.lw.cm
            cm = new CommunicationModule();
            InitializeComponent();
        }

        private void LoginMethod()
        {
            if (PwdTextBox.Text != "" && LoginTextBox.Text != "")
            {//start if
                string login = LoginTextBox.Text;
                string pwd = PwdTextBox.Text;
                byte[] pwdArray = BindingModule.enc.GetBytes(pwd);
                StatusLabel.Text = "Login in progress";
                pwd = "";
                PwdTextBox.ResetText();
                //send login and wait for n
                int n = 1;
                if (n == -1)//Błędny login
                {
                    StatusLabel.Text = "Signing in failed";
                }
                else //else1
                {
                    pwdArray = CryptoModule.HashNTimes(pwdArray, n);
                    //send pwdArray
                    //Poczekaj na odpowiedz

                    bool response = true;
                    if (response)
                    {
                        BindingModule.setLogin(login);
                        //Udane logowanie, czekam na certyfikat
                        StatusLabel.Text = "Waiting for certificate";
                        //Otrzymany certyfikat
                        MainWindow mw = new MainWindow(this);
                        this.Hide();
                        mw.Show();
                    }
                    else
                    {
                        StatusLabel.Text = "Signing in failed";
                    }
                }//end else1

            }//end if
            else
            {
                StatusLabel.Text = "Need more data to proceed";
            }

        }

        private void RegisterLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new RegisterWindow().Show();
        }

        private void LoginTextBox_Enter(object sender, EventArgs e)
        {
            LoginTextBox.Text = "";
        }

        private void PwdTextBox_Enter(object sender, EventArgs e)
        {
            PwdTextBox.Text = "";
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            LoginMethod();
        }

        private void LoginWindow_Load(object sender, EventArgs e)
        {
            this.RegistrationToolTip.SetToolTip(this.RegisterLabel, "Click to register");
            this.LoginBtnToolTip.SetToolTip(this.LoginBtn, "Click to log in");
        }

        private void PwdTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                LoginMethod();
            }
        }

    }
}
