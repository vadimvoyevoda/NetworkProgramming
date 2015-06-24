using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        Server server;
        Client client;

        public MainWindow()
        {            
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.btnConnect.Click += btnConnect_Click;
            this.btnSend.Click += btnSend_Click;
        }

        void btnSend_Click(object sender, RoutedEventArgs e)
        {            
            client.Send(tbMsg.Text);
        }

        void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            server = new Server(tbIp.Text, Int32.Parse(tbPort.Text));
            client = new Client(tbIpCl.Text, Int32.Parse(tbPortCl.Text));

            server.OnDataRecieved += server_OnDataRecieved;
            server.Listen();
        }

        void server_OnDataRecieved(string msg)
        {
            Dispatcher.Invoke(new Action(
                delegate()
                {

                    if (rtbChat.Text.Length != 0)
                    {
                        rtbChat.Text += Environment.NewLine;
                    }
                    rtbChat.Text += msg;
                }));
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {            
        }
    }
}
