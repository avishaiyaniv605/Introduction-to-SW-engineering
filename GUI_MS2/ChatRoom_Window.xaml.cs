using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ISE172_project.Logic;
using ISE172_project.Pressistence;
using ISE172_project;
using System.Collections.ObjectModel;
using GUI_MS2.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI_MS2
{
    /// <summary>
    /// A class representing the chat room window. (code behind of chatroom xaml). 
    /// </summary>
    public partial class ChatRoom_Window : Window
    {
        #region Atributes
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         private ObserChatRoomModel _chat;
        private DispatcherTimer dispatcherTimer; //= new DispatcherTimer();  // the timer 
        private editMessageWindow editMessage;
        #endregion

        public ChatRoom_Window() //constructor
        { 
            _chat = new ObserChatRoomModel();
            this.DataContext = _chat;
            initiatePreferences();
            InitializeComponent();
            _chat.Messages = new ObservableCollection<Message>(MainWindow._chatRoom.Messages);
            editMessage = new editMessageWindow();
            editMessage.Hide();
            MainWindow._chatRoom.CenterOperation = new SortTime(true);
            editMessage.DataContext = _chat;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer_Tick(null, null);
            playDispatcherTimer();
        }  

        #region Timer
        /**
         * start the timer that works every 2 seconds
      **/
        private void playDispatcherTimer()
        {
            scrollDown();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            log.Info("The Timer is start");
        }

        public void startTimer()
        {
            dispatcherTimer.Start();
        }
        /**
         * the actions that happens every timer tick
         * */
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            comboBoxSwitch();
            initcombox();
            updateMessages();
            updateUsersSelection();
        }
        #endregion

        #region initiating window

        /**
         * Setting the initial preferences of the window
         */
        private void initiatePreferences()
        {
            _chat.NewMessage = "Type Your Message Here";
            log.Info("The chat room window is open");
            _chat.Group_id_filter = "All";
            _chat.Group_id_and_nickname_filter = "All";
        }
        /*
         * defines the init value of the comboboxs
         */
        private void initcombox()
        {
            if (_chat.Group_id_and_nickname_filter == null)
            {
                int last = filter_gid_nickname.Items.Count - 1;
                filter_gid_nickname.SelectedIndex = last;
            }
            if (_chat.Group_id_filter == null)
            {
                int last = filter_g_id.Items.Count - 1;
                filter_g_id.SelectedIndex = last;
            }
            if (_chat.Sort_by == null)
            {
                sort_combo_box.SelectedIndex = 0;
            }
        }

        /*
         * Determine wich of the Filter ComboBox is available to chooe
         * while using GroupID filter one cannot use the GroupID and Filter option.
         */
        private void comboBoxSwitch()
        {
            if (_chat.Group_id_and_nickname_filter != "All" && _chat.Group_id_and_nickname_filter != null)
            {
                filter_g_id.IsEnabled = false;

            }
            else if (_chat.Group_id_filter != "All" && _chat.Group_id_filter != null)
            {
                filter_gid_nickname.IsEnabled = false;
            }
            else
            {
                filter_gid_nickname.IsEnabled = true;
                filter_g_id.IsEnabled = true;
            }
        }
        #endregion

        #region Reaching buissnes logic

        /*
        * reaching the chatroom in logic in order to get the selectoins of user in regard of filter and sort
        */
        private void updateUsersSelection()
        {
            MainWindow._chatRoom.updateUsers();
            if (MainWindow._chatRoom.UserListChanged)
            {
                List<string>[] tSelections = MainWindow._chatRoom.getListsReady();
                _chat.Group_id_select = tSelections[0];
                _chat.Group_id_and_nickname_Select = tSelections[1];
            }
        }

        /*
         * reaching the chatroom logic in order to update message list in gui
         */
        private void updateMessages()
        {
            MainWindow._chatRoom.updateMessages();
            if (MainWindow._chatRoom.MessagesListChanged)
            {
                _chat.Messages = new ObservableCollection<Message>(MainWindow._chatRoom.Messages);
                scrollDown();
                MainWindow._chatRoom.MessagesListChanged = false;
            }
        }

        /*
         * reaching the chatroom in logic in order to send a message
         */
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            log.Info("The user:" + MainWindow._chatRoom.LoggedInUser.ToString() + " send message");
            if (_chat.NewMessage != "")
                try
                {
                    MainWindow._chatRoom.send(_chat.NewMessage);
                    _chat.NewMessage = "";
                    NewMessageTextBox.Foreground = Brushes.Black;
                    NewMessageTextBox.GotFocus += Enter_Message_GotFocus;

                }
                catch (Exception ex)
                {
                    log.Warn(ex.Message);
                }
        }

        /*
         * act when a message was selected in listbox xaml.
        */
        private void Messages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox tList;
            if (sender is ListBox)
            {
                editMessage.Hide();
                tList = (ListBox)sender;
                if (tList != null)
                {
                    Message m = (Message)tList.SelectedItem;
                    if (m != null && m.Writer_gID == MainWindow._chatRoom.LoggedInUser.Group_ID && m.Writer_name == MainWindow._chatRoom.LoggedInUser.NickName)
                    {
                        _chat.IsEditable = Visibility.Visible;
                        _chat.SelectedMessageBody = m.MessageBody;
                        _chat.SelectedMessage = m;
                    }
                    else
                    {
                        _chat.IsEditable = Visibility.Hidden;
                        _chat.SelectedMessageBody = "";
                        _chat.SelectedMessage = null;
                    }
                }
            }
        }

        /*
        *this method working when filter and sort button is pressed. it start the process of sort and fiter messages by user's choices
         */
        private void Button_Click_Sort_Filter(object sender, RoutedEventArgs e)
        {
            log.Info("Click on the  button sort and filter ");
            MainWindow._chatRoom.Messages.Clear();
            MainWindow._chatRoom.resetTime();
            Filter();
            MainWindow._chatRoom.Messages = Sort(MainWindow._chatRoom.Messages);
            MainWindow._chatRoom.MessagesListChanged = true;           
            scrollDown();
        }

        #endregion

        #region Additional methods
        /*
         * Once pressed the LogOut Button
         * the chat window will be closed and move on to Main Menu window
         */
        private void Button_Click_Logout(object sender, RoutedEventArgs e)
        {
            this.dispatcherTimer.Stop();
            MainWindow._chatRoom = null;
            MainWindow mw = new MainWindow();
            log.Info("Back to main window");
            mw.Show();           
            this.Hide();
        }

        /*
         * method that removes and adds the last message of the observable list in order to make a change
         */
        private void scrollDown()
        {
            if (_chat.Messages.Any())
            {
                Message tLastMessage = _chat.Messages.Last();
                _chat.Messages.RemoveAt(_chat.Messages.Count() - 1);
                _chat.Messages.Add(tLastMessage);
            }
        }

        /*
         * if the text box is pressed (getting focus)
         */
        private void Enter_Message_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Text = string.Empty;
            box.Foreground = Brushes.Black;
            box.GotFocus -= Enter_Message_GotFocus;
        }

        /*
         * if the text box is released (losing focus) 
         */
        private void Enter_Message_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text.Trim().Equals(string.Empty))
            {
                box.Text = "";
                box.Foreground = Brushes.Black;
                box.GotFocus += Enter_Message_GotFocus;
            }
        }
        /*
         * enabeling enter key to send message 
         */
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            log.Info("Click on Enter for send message");
            if (e.Key == Key.Enter)
            {
                send.Focus();
                Send_Click(this, e);
                ((TextBox)sender).Focus();
            }
        }

        /*
      * when a user select a message he wrote an edit button will appear and the user can edit the message
      */
        private void editButtonClick(object sender, RoutedEventArgs e)
        {
            editMessage = new editMessageWindow();
            editMessage.DataContext = _chat;
            editMessage.Show();
        }

        #endregion

        #region Sort and Filter
        /*
         * get list of message and sort them by required sort 
         */
        private List<Message> Sort(List<Message> listMessages)
        {
            CenterOperations newSort = new SortTime(_chat.Ascending);
          
            if (_chat.Sort_by == "System.Windows.Controls.ComboBoxItem: Nickname")
            {
                log.Info("Select sort by :Nickname");
                newSort = new SortNickName(_chat.Ascending);
            }
            else if (_chat.Sort_by == "System.Windows.Controls.ComboBoxItem: g_id, nickname, and timestamp")
            {
                log.Info("Select sort by : g_id, nickname, and timestamp");
                newSort = new SortGidNicknameTime(_chat.Ascending);
            }

            MainWindow._chatRoom.CenterOperation = newSort;
            return newSort.doOpeation_NameClass(listMessages);
        }

        /*
         * get list of message and fitler them by required filter 
         */
        private void Filter()
        {
            MainWindow._chatRoom.clearFillers();
            if (_chat.Group_id_filter != "All")
            {
                log.Info("select filter by :group id");
                MainWindow._chatRoom.addGroupFilter(_chat.Group_id_filter);
            }
            else if (_chat.Group_id_and_nickname_filter != "All")
            {
                log.Info("select filter by :group id and nick name");
                string[] split = _chat.Group_id_and_nickname_filter.Split(',');
                MainWindow._chatRoom.addGroupFilter(split[0]);
                MainWindow._chatRoom.addNicknameFilter(split[1].Substring(1));
            }
        }

        #endregion

        #region unimplemented methods

        /**
         * Unimplemented methods
         */

        private void Messages_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void group_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

       
        private void group_id_and_nickname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ComboBox_messages(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_ascending(object sender, RoutedEventArgs e)
        {


        }

        private void RadioButton_descending(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_send(object sender, RoutedEventArgs e)
        {

        }


        private void Sort_by(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EnterMessage_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Status_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void User_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void sort_nickName_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void sort_TimeStamp_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
    #endregion
}
