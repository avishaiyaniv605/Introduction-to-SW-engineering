using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ISE172_project.Logic;
using ISE172_project.Pressistence;
using ISE172_project;

namespace GUI_MS2
{
    /// <summary>
    /// A class representing the main window of the chat
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ChatRoom _chatRoom;

        public MainWindow() //constructor
        {
            if (_chatRoom==null) _chatRoom = new ChatRoom();
            log.Info("create new chat room");
            InitializeComponent();
            log.Info("main window is open");
        }

       /*
        * When the user clicks the Login Button 
        * The following button will close the "Main" window and open a login window
        */
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            Login_Window wLog = new Login_Window();
            log.Info("open login window");
            wLog.Show();
            this.Close();
        }

        /*
         * When the user clicks the Registration Button 
         * The following button will close the "Main" window and opens a Registration window
         */
        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            Registration_Window wReg = new Registration_Window();
            log.Info("open register window");
            wReg.Show();
            this.Close();
        }
        
        /*
         * Closing the app
         */
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            log.Info("Close the application");
            MessageBox.Show("Good bye");
            System.Windows.Application.Current.Shutdown();
           
        }
    }
}
