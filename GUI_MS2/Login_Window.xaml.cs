using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace GUI_MS2
{
    /// <summary>
    /// A class representing the login window of the chat. (code behind of xmal)
    /// </summary>
    public partial class Login_Window : Window
    {
        #region Atributes
        ObservableModel _main = new ObservableModel();
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _password;
        private bool _isPwValid;

        public string _Password { get => _password; set => _password = value; }
        public bool IsPwValid { get => _isPwValid; set => _isPwValid = value; }
        #endregion

        public Login_Window() //cunstructor

        {
            this.DataContext = _main;
            InitializeComponent();
        }
        
        /*
         * The Logic_Click is initiated when the user is pressing the login button 
         * after pressnig the login button we will pass the proccess foroward and try to login with the values entered thourgh this function
         * the process is happening in the BuisnessLogic layer with the _chatRoom.login (line 104)
         * 
         */
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NotesTextBox.Focus();
            Boolean canLogin = true;
            try
            {
                if (IsPwValid)
                {
                    MainWindow._chatRoom.login(_main.Entered_NickName, _main.Entered_GrID, _Password);
                }
                else{
                    canLogin = false;
                    _main.LoginNotes = "invalid Password";
                    return;
                }
            }
            catch (Exception i)
            {
                _main.LoginNotes = i.Message;
                loggerOfReg(i.Message);
                canLogin = false;
            }
            if (canLogin)
            {
                ChatRoom_Window wChat = new ChatRoom_Window();
                wChat.Show();
                wChat.startTimer();
                this.Close();
            }
        }

        /*
         * collecting logs to the log file throughout the logging in process
         */
        private void loggerOfReg(string e)
        {
            if (e.Equals("There was a problem connecting to the server"))
            {
                log.Error(e);
            }
            else if (e.Equals("Register before logging in."))
            {
                log.Warn("Try login with user dont exist");
            }
            else if (e.Equals("Invalid input - Only Numbers are allowed as Group ID"))
            {
                log.Warn("enter Invalid input");
            }
            else
            {
                log.Warn(e);
            }
        }

        /*
         * Handling Enter key press through this window - similiar to click the button of register
         */
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(this, e);
                log.Info("click enter for login");
            }
        }

        /*
         * back to main menu when click on button back
         */
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wMain = new MainWindow();
            log.Info("Back to main window");
            wMain.Show();
            this.Close();
        }

        /**
         * Temporery storing an hashed password and passas it on to the BL.
         */
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pd = sender as PasswordBox;
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            if (regex.IsMatch(pd.Password) && pd.Password.Length <= 16 && pd.Password.Length >= 4)
            {
                _Password = ISE172_project.Logic.Hashing.hashPassword(pd.Password);
                IsPwValid = true;
            }
            else {
                IsPwValid = false;
            }
        }

        private void Notes_TextChanged(object sender, TextChangedEventArgs e) //unimplemented
        {

        }
    }
}
