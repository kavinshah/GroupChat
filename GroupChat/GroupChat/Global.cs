using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupChat
{
    public static class Global
    {
        public static Connection.RabbitConnection RabbitConnection = new Connection.RabbitConnection();
        public static string username = "";
        public static Dictionary<string, GroupChat.GUI.ChatWindow> ChatWindows = new Dictionary<string, GUI.ChatWindow>();
        public static GroupChat.GUI.GroupCreation groupcreation = new GUI.GroupCreation();
        public static GroupChat.GUI.Login login;
        public static Form1 main;
    }
}
