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

namespace ClientUdp
{
    public partial class MainWindow : Window
    {
        private Socket clientSocket;
        private EndPoint epServer;
        IPEndPoint server;

        byte[] bytes;
        byte[] byteData;
        string data;
        int offset = 0;

        public MainWindow()
        {
            InitializeComponent();
            btnConnect.Click += btnConnect_Click;
            btnSend.Click += btnSend_Click;
            Closing += MainWindow_Closing;
        }
        
        void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msgConn = tbName.Text + " connected";
                
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress serverIP = IPAddress.Parse(tbIp.Text);
                server = new IPEndPoint(serverIP, 30000);
                epServer = (EndPoint)server;

                byteData = Encoding.UTF8.GetBytes(msgConn);
                clientSocket.BeginSendTo(byteData, 0, byteData.Length > 1024 ? 1024 : byteData.Length, SocketFlags.None, epServer, new AsyncCallback(OnSend), null);

                bytes = new byte[1024];
                clientSocket.BeginReceiveFrom(bytes, 0, 1024, SocketFlags.None, ref epServer, new AsyncCallback(ReceiveData), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message);
            }
        }
        
        void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clientSocket != null)
                {
                    if (tbName.Text.Length > 0)
                    {
                        byteData = Encoding.UTF8.GetBytes(tbName.Text + " : " + tbMsg.Text);

                        clientSocket.BeginSendTo(byteData, 0, byteData.Length > 1024 ? 1024 : byteData.Length, SocketFlags.None, epServer, new AsyncCallback(OnSend), null);                        
                    }
                    else
                    {
                        MessageBox.Show("Please enter your name");
                    }
                }
                else
                {
                    MessageBox.Show("At first please connected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message);
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            if (clientSocket != null)
            {
                if (tbName.Text.Length > 0)
                {
                    byteData = Encoding.UTF8.GetBytes(tbName.Text + " disconnected");

                    clientSocket.BeginSendTo(byteData, 0, byteData.Length > 1024 ? 1024 : byteData.Length, SocketFlags.None, epServer, new AsyncCallback(OnSend), null);
                }
            }
        }

        private void OnSend(IAsyncResult res)
        {
            try
            {
                int bytesSend = clientSocket.EndSend(res);
                offset += bytesSend;
                if (offset < byteData.Length)
                {
                    clientSocket.BeginSendTo(byteData, offset, byteData.Length - offset > 1024 ? 1024 : byteData.Length - offset, SocketFlags.None, epServer, new AsyncCallback(OnSend), null);
                }
                else
                {
                    offset = 0;
                    byteData = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Data: " + ex.Message);
            }
        }

        private void ReceiveData(IAsyncResult res)
        {
            try
            {
                int bytesRec = clientSocket.EndReceive(res);

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                if (bytesRec != 1024)
                {
                    Dispatcher.Invoke(new Action(
                        delegate()
                        {
                            tbText.Text += Environment.NewLine + data;
                        }));
                    
                    data = string.Empty;
                }

                bytes = new byte[1024];
                clientSocket.BeginReceiveFrom(bytes, 0, bytes.Length, SocketFlags.None, ref epServer, new AsyncCallback(ReceiveData), null);                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Receive Data: " + ex.Message);
            }
        }
    }
}
