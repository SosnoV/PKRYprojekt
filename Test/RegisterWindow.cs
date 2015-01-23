using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class RegisterWindow : Form
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            string login, pwd, pesel;
            login = LoginTextBox.Text;
            pesel = maskedTextBox1.Text;
            pwd = PwdTextBox.Text;
            if (LoginTextBox.Text == "" || maskedTextBox1.Text == "" ||
                PwdTextBox.Text == "" || ConfPwdTextBox.Text == "")
            {
                RegisterBtn.Enabled = false;
                StatusLabel.Text = "Provide more information";
                return;
            }
            else
            {
                RegisterBtn.Enabled = true;
            }
            if (pwd.Equals(ConfPwdTextBox.Text))
            {
                //wyslac ponizsze i poczekac na odpowiedz
                CryptoModule.HashMessage(pwd);
                CryptoModule.HashMessage(pesel);

                //odpowiedz
                bool response = true;
                if (response)
                {
                    StatusLabel.Text = "Registration completed succesfully";
                    LoginTextBox.ReadOnly = true;
                    maskedTextBox1.ReadOnly = true;
                    PwdTextBox.ReadOnly = true;
                    ConfPwdTextBox.ReadOnly = true;
                }
                else
                {
                    StatusLabel.Text = "Registration failed";
                    LoginTextBox.ResetText();
                    maskedTextBox1.ResetText();
                    PwdTextBox.ResetText();
                    ConfPwdTextBox.ResetText();
                }
            }
            else 
            {
                StatusLabel.Text = "Passwords don't match"; 
            }
        }


    }
}
