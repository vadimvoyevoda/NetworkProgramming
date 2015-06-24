using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace OpenPop_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<Message> msg = FetchAllMessages("pop.gmail.com", 995, true, "vadimvoyevoda@gmail.com", "16101991");

            //foreach (var el in msg)
            //{
                txMessage.NavigateToString(Encoding.UTF8.GetString(msg[0].RawMessage));
                //Thread.Sleep(1000);
                //txMessage.Clear();
            //}
        }

        public static List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {            
            using (Pop3Client client = new Pop3Client())
            {
                client.Connect(hostname, port, useSsl);
                client.Authenticate(username, password, AuthenticationMethod.UsernameAndPassword);
                //int messageCount = client.GetMessageCount();
                int messageCount = 2;
                List<Message> allMessages = new List<Message>(messageCount);
                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }
                return allMessages;
            }
        }
    }
}
