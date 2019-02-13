using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroupChat.Utilities;

namespace GroupChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Utilities.Logwriter.InitialiseBuffers(Application.StartupPath);
            InitializeComponent();
        }

        private void openNewChatBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Global.groupcreation = new GUI.GroupCreation();
                Global.groupcreation.MdiParent = Global.main;               
                Global.groupcreation.Show();
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " +  ex.Message);
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.login == null)
                {
                    Global.login = new GUI.Login();
                    Global.login.MdiParent = Global.main;
                    Global.login.Show();
                    Global.login.LoginStatus += ChangeLoginStatus;
                }
                else
                {
                    MessageBox.Show("Already Logged in");
                }
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }
            
        }

        private void ChangeLoginStatus(bool status)
        {
            try
            {
                if (status)
                {
                    labelStatus.BackColor = Color.LawnGreen;
                    labelStatus.Text = "LoggedIn";
                }
                else
                {
                    labelStatus.Text = "LoggedOut";
                    labelStatus.BackColor = Color.Crimson;
                    Global.login = null;
                }
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Global.main = this;
                labelStatus.BackColor = Color.Red;
                this.IsMdiContainer = true;
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (labelStatus.Text == "LoggedIn")
                {
                    ChangeLoginStatus(!Global.RabbitConnection.DisconnectServer());

                }
                else
                {
                    MessageBox.Show("Please Login first");

                }
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);

            }
        }

    }
}
