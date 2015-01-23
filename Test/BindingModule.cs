using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    /// <summary>
    /// Klasa pomocnicza przenosząca dane między obiektami różnych klas
    /// </summary>
    class BindingModule
    {
        ///// <summary>
        ///// Kolekcja aktywnych czatów
        ///// </summary>
        //private static Dictionary<string, ChatWindow> activeChats = 
        //    new Dictionary<string,ChatWindow>();

        public static Encoding enc = Encoding.UTF8;
        /// <summary>
        /// Login użytkownika
        /// </summary>
        public static string myLogin { get; private set; }
        /// <summary>
        /// Metoda sprawdzająca poprawność peselu
        /// </summary>
        /// <param name="pesel"></param>
        /// <returns></returns>
        public static bool CheckPesel(string pesel) 
        {
            if (pesel.Length != 10)
                return false;
            bool result = false;
            int[] parameters = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            char[] array = pesel.ToCharArray();
            int sum = 0;
            int controlNumber = -1;
            for (int i = 0; i < 10; i++)
            {
                sum += parameters[i] * int.Parse(array[i].ToString());
                if(i == 9)
                    controlNumber = int.Parse(array[i].ToString());
            }
            sum = sum % 10;
            if (sum != 0)
                sum = 10 - sum;
            if (sum == controlNumber)
                result = true;
            return result;
        }
        /// <summary>
        /// Metoda ustawiająca login
        /// </summary>
        /// <param name="login"></param>
        public static void setLogin(string login)
        {
            myLogin = login;
        }
        /// <summary>
        /// Metoda do wyciągnięcia czatu z kolekcji
        /// </summary>
        /// <param name="userName">
        /// Login użytkownika z którym prowadzona jest rozmowa w szukanym czacie
        /// </param>
        /// <returns></returns>
        //public static ChatWindow GetChat(string userName)
        //{
        //    ChatWindow ch;
        //    if (!activeChats.TryGetValue(userName, out ch))
        //        return null;
        //    return ch;
        //}

        ///// <summary>
        ///// Dodanie czatu do kolekcji
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="mw"></param>
        //public static void AddChat(string userName, MainWindow mw) 
        //{
        //    //na wypadek 2 rozmowy w czasie dzialania programu, jakby cos sie nie usunelo
        //    try
        //    {
        //        activeChats.Remove(userName);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    finally
        //    {
        //        ChatWindow chat = new ChatWindow(userName, mw);
        //        activeChats.Add(userName, chat);
        //        chat.Show();
        //    }
        //}
        /// <summary>
        /// Usunięcie klucza publicznego z pamięci programu
        /// </summary>
        /// <param name="userName"></param>
        public static void RemoveUserKey(string userName) 
        {
            //activeChats.Remove(userName);
            CryptoModule.RemoveUserKey(userName);
        }



        //public static void CloseChats()
        //{
        //    foreach (var chat in activeChats)
        //    {
        //        chat.Value.Close();
        //    }
        //}
    }
}
