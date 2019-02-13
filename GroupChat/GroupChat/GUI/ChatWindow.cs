using GroupChat.Utilities;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace GroupChat.GUI
{
    public partial class ChatWindow : Form
    {
        public ChatWindow()
        {
            InitializeComponent();
        }

        //public ChatWindow(string groupname)
        //{
        //    try
        //    {
        //        GroupName = groupname;
        //        this.Text = GroupName;

        //        this.MdiParent = Global.main;

        //        //create channel
        //        channel = Global.RabbitConnection.CreateGroup(GroupName);

        //        if (channel != null && channel.IsOpen == true)
        //        {
        //            //create callbacks
        //            CreateConsumer();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Cannot join group " + groupname);
        //            this.Close();
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}
        private void ChatWindow_Load(object sender, EventArgs e)
        {
            try
            {

                this.Text = GroupName;

                this.MdiParent = Global.main;

                //create channel
                channel = Global.RabbitConnection.CreateGroup(GroupName);

                if (channel != null && channel.IsOpen == true)
                {
                    //create callbacks
                    CreateConsumer();
                }
                else
                {
                    MessageBox.Show("Cannot join group " + GroupName);
                    this.Close();
                }

            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message); 
            }
        }

        public bool CreateConsumer()
        {
            try
            {

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    ReceivedMessage(message);
                };
                channel.BasicConsume(queue: Qname,
                                 noAck: false,
                                 consumer: consumer);

                return true;
            }
            catch (Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
                return false;
            }
        }


        public void ReceivedMessage(string messageD)
        {
            try
            {
                string message = GroupChat.Utilities.RSACryptography.CryptographyHelper.Decrypt(messageD);
                WriteToFile(message);
                richTextBoxReceived.Invoke(new MethodInvoker(delegate
                {
                    richTextBoxReceived.Text = richTextBoxReceived.Text + message;
                }));

            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);                
            }
        }


        public void WriteToFile(string text)
        {
            try
            {
                using(var sr = System.IO.File.AppendText(Application.StartupPath + "\\Chats\\" + GroupName + ".txt"))
                {
                    sr.WriteLine(text);
                }
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        public DateTime getNisttime()
        {
            DateTime time = new DateTime();
            try
            {
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    time = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
            }
            catch(Exception ex)
            {
                time = System.DateTime.Now;
            }

            return time;
        }

        public bool sendMessage(string message)
        {
            try
            {
                byte[] body = Encoding.UTF8.GetBytes(GroupChat.Utilities.RSACryptography.CryptographyHelper.Encrypt(Global.username + ": " + message + "  (Sent at "+ getNisttime().ToString("hh:mm:ss tt") + " )" + "\n\n"));

                channel.BasicPublish(exchange: "GroupChat",
                                 routingKey: GroupName,
                                 basicProperties: null,
                                 body: body);
                return true;
            }
            catch (Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
                return false;
            }
        }

        private void ChatWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Global.ChatWindows.ContainsKey(this.Text))
                Global.ChatWindows.Remove(this.Text);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                richTextBoxSend.Invoke(new MethodInvoker(delegate
                {
                    message = richTextBoxSend.Text;
                    richTextBoxSend.Text = "";
                }));

                bool status = sendMessage(message);

                if (!status)
                {
                    labelStatus.Text = "Message Not Sent";
                    labelStatus.BackColor = Color.Crimson;
                    //System.Timers.Timer timer = new System.Timers.Timer(1000*10);
                    //timer.Elapsed += new ElapsedEventHandler(timerHandler);
                }
                else
                {
                    labelStatus.Text = "Message Sent";
                    labelStatus.BackColor = Color.LawnGreen;
                }
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        //private void timerHandler(object source, ElapsedEventArgs e)
        //{

        //}

        public RabbitMQ.Client.IModel channel;
        public string GroupName = "";
        public string Qname = "";

        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Global.ChatWindows.ContainsKey(this.Text))
                    Global.ChatWindows.Remove(this.Text);
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBoxReceived.Invoke(new MethodInvoker(delegate
                {
                    richTextBoxReceived.Text = "";
                }));
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
            }
        }
    }
}
