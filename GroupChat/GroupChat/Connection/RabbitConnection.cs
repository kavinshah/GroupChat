using GroupChat.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupChat.Connection
{
    public class RabbitConnection
    {

        RabbitMQ.Client.ConnectionFactory Factory;
        RabbitMQ.Client.IConnection Connection;

        public bool ConnectServer(string hostname)
        {
            try
            {
                Factory = new RabbitMQ.Client.ConnectionFactory() { HostName = hostname };
                Connection = Factory.CreateConnection();

                return true;
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
                return false;
            }
        }

        public bool DisconnectServer()
        {
            try
            {
                if (Connection.IsOpen)
                    Connection.Close();
                return true;
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
                return false;
            }
        }

        public RabbitMQ.Client.IModel CreateGroup(string groupname)
        {
            try
            {
                RabbitMQ.Client.IModel channel = Connection.CreateModel();
                channel.ExchangeDeclare(exchange: "GroupChat", type: "direct");
                channel.QueueBind(queue: channel.QueueDeclare().QueueName, exchange: "GroupChat", routingKey: groupname);
                return channel;
            }
            catch(Exception ex)
            {
                Logwriter.WriteToErrorLogs(DateTime.Now.ToString("hh:mm:ss tt") + " : " + ex.TargetSite.ReflectedType.Name + " - " + ex.TargetSite.Name + " - " + ex.Message);
                return null;
            }
        }


    }
}
