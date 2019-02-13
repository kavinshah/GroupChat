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
    public partial class GroupCreation : Form
    {
        public GroupCreation()
        {
            InitializeComponent();
        }

        private void buttonJoin_Click(object sender, EventArgs e)
        {
            if (textBoxGroupName != null && !Global.ChatWindows.ContainsKey(textBoxGroupName.Text))
            {
                Global.ChatWindows.Add(textBoxGroupName.Text, new ChatWindow());
                Global.ChatWindows[textBoxGroupName.Text].GroupName = textBoxGroupName.Text;
                Global.ChatWindows[textBoxGroupName.Text].Show();
            }
            else if(Global.ChatWindows.ContainsKey(textBoxGroupName.Text))
            {
                MessageBox.Show("Chat Window " + textBoxGroupName.Text + "is already open!");
            }

            this.Close();
        }
    }
}
