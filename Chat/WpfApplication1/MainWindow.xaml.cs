using Microsoft.Win32;
using Newtonsoft.Json;
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
        
        bool isFile;

        public MainWindow()
        {            
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.btnConnect.Click += btnConnect_Click;
            this.btnSend.Click += btnSend_Click;
            this.btnAdd.Click += btnAdd_Click;
            this.tbMsg.KeyDown += tbMsg_KeyDown;
            this.btnCancel.Click += btnCancel_Click;
            this.btnReceive.Click += btnReceive_Click;
            isFile = false;                 
        }

        void btnReceive_Click(object sender, RoutedEventArgs e)
        {
            //btnReceive.IsEnabled = false;
            //btnCancel.IsEnabled = false;
            //SerializationClass ser = new SerializationClass();
            //ser.Command = "AcceptFile";
            //ser.FileName = client.fileName;

            //string msg = JsonConvert.SerializeObject(ser);
            //client.Send(msg, 2);
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //btnReceive.IsEnabled = false;
            //btnCancel.IsEnabled = false;

            //SerializationClass ser = new SerializationClass();
            //ser.Command = "NotAcceptFile";
            //ser.FileName = client.fileName;

            //string msg = JsonConvert.SerializeObject(ser);
            //client.Send(msg, 3);
        }

        void tbMsg_KeyDown(object sender, KeyEventArgs e)
        {
            isFile = false;
        }

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.DefaultExt = "txt";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbMsg.Text = ofd.FileName;
                isFile = true;
            }
        }

        void btnSend_Click(object sender, RoutedEventArgs e)
        {
            client.Send(tbMsg.Text, isFile);
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
            //if (msg.StartsWith("Do you want"))
            //{
            //    btnReceive.IsEnabled = true;
            //    btnCancel.IsEnabled = true;
            //}

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
