using GUI_MS2.Models;
using ISE172_project.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace GUI_MS2
{
    /// <summary>
    /// Interaction logic for editMessageWindow.xaml
    /// </summary>
    public partial class editMessageWindow : Window
    {
        public editMessageWindow() //constructor
        {
            InitializeComponent();
        }

        private void cancelButtonClick(object sender, RoutedEventArgs e) //if cancel button has been pressed
        {
            this.Close();
        }

        public void saveButtonClick(object sender, RoutedEventArgs e) //if save button has been pressed, saves the message to server
        {
            ObserChatRoomModel _chat = (ObserChatRoomModel)DataContext;
            Message tMsg = _chat.SelectedMessage;
            string tBody = _chat.SelectedMessageBody;
            MainWindow._chatRoom.saveEditedMessage(tMsg, tBody);
            MessageBox.Show("Your message has succefully edited");
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) // unimplemented
        {

        }

    }
}
