using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    
    class BindingModule
    {
        private static Dictionary<string, ChatWindow> activeChats = 
            new Dictionary<string,ChatWindow>();

        public static Encoding enc = Encoding.UTF8;
        public static string myLogin { get; private set; }

        public static void setLogin(string login)
        {
            myLogin = login;
        }

        public static ChatWindow GetChat(string userName)
        {
            ChatWindow ch;
            if (!activeChats.TryGetValue(userName, out ch))
                return null;
            return ch;
        }

        public static void AddChat(string userName, MainWindow mw) 
        {
            //na wypadek 2 rozmowy w czasie dzialania programu, jakby cos sie nie usunelo
            try
            {
                activeChats.Remove(userName);
            }
            catch (Exception)
            {

            }
            finally
            {
                ChatWindow chat = new ChatWindow(userName, mw);
                activeChats.Add(userName, chat);
                chat.Show();
            }
        }

        public static void RemoveUserChatAndKey(string userName) 
        {
            activeChats.Remove(userName);
            CryptoModule.RemoveUserKey(userName);
        }



        public static void CloseChats()
        {
            foreach (var chat in activeChats)
            {
                chat.Value.Close();
            }
        }
    }
}
