using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupChat.GUI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public delegate void changeStatus(bool Loginstatus);
        public changeStatus LoginStatus;

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            bool loggedin = Global.RabbitConnection.ConnectServer(textBoxServer.Text);

            if (!loggedin)
            {
                LoginStatus(false);
                Application.Exit();
            }
            else
            {
                LoginStatus(true);
            }

            Global.username = textBoxUsername.Text;
            //MessageBox.Show("Connected to Server: "+ textBoxServer.Text);
            this.Close();
        }
    }
}
