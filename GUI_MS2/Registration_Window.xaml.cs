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
    /// A class representing the registratoin window of the chat (code behind of registration xaml)
    /// </summary>
    public partial class Registration_Window : Window
    {
        #region Atributes
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		ObservableModel _main = new ObservableModel();
        private string _password;
        private string _verPassword;
        private bool _isPwValid;

        public string _Password { get => _password; set => _password = value; }
        public bool IsPwValid { get => _isPwValid; set => _isPwValid = value; }
        public string _VerPassword { get => _verPassword; set => _verPassword = value; }
        #endregion

        public Registration_Window() //constructor
        {
            this.DataContext = _main;
            InitializeComponent();
        } 
            
        /*
         * after pressnig the registration button we will pass the proccess foroward and try to register with the values entered thourgh this function
         * the process is happening in the BuisnessLogic layer.
         */
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsPwValid)
                {
                    MainWindow._chatRoom.registration(_main.ChosedNickName, _main.ChosedGrID, _Password, _VerPassword);
                    MessageBox.Show("You are successfully registered");
                    log.Info("have new user :" + _main.ChosedNickName + "," + _main.ChosedGrID);
                    MainWindow m = new MainWindow();
                    m.Show();
                    this.Close();
                }
                else {
                    _main.RegNotes = "Password must be between 4 to 16 letters a-Z and numbers. ";
                    return;
                }
            }
            catch (Exception ex)
            {
                _main.RegNotes = ex.Message;
                loggerforReg(ex.Message);
            }
        }

        /*
         * collecting logs
         */
        private void loggerforReg(string e)
        {
            if (e.Equals("Only numbers are allowed as Group ID"))
            {
                log.Warn("Try register with unvalid value");
            }
            else if (e.Equals("User is already taken."))
            {
                log.Warn("Try registeration with exist user ");
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
                Register_Click(this, e);
            }
        }

        /*
         * back to the main window
         */
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wMain = new MainWindow();
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
            else
            {
                IsPwValid = false;
            }
        }

        /**
         * Temporery storing an hashed verification password and passas it on to the BL.
         */
        private void PasswordBoxVer_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pd = sender as PasswordBox;
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            if (regex.IsMatch(pd.Password) && pd.Password.Length <= 16 && pd.Password.Length >= 4)
            {
                _VerPassword = ISE172_project.Logic.Hashing.hashPassword(pd.Password);
                IsPwValid = true;
            }
            else
            {
                IsPwValid = false;
            }
        }

        private void ChooseGridTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
