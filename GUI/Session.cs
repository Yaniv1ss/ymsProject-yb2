using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace GUI
{
    public static class Session
    {
        public static People CurrentUser { get; set; }
        public static bool IsAdmin { get; set; }

        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        public static void Logout()
        {
            CurrentUser = null;
            IsAdmin = false;
        }
    }
}
