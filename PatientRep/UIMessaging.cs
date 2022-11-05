using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PatientRep
{
    public static class UIMessaging
    {
        public static MessageBoxResult CreateMessageBox(string message, string title, MessageBoxButton button, MessageBoxImage img)
        {
            return MessageBox.Show(message, title, button, img);
        }
    }
}
