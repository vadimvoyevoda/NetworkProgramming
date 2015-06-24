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

namespace ServerUdp
{
    public partial class MainWindow : Window
    {
        private Socket serverSocket;
        private byte[] bytes = new byte[1024];
        byte[] dataSend;
        string data;
        int offset = 0;

        public delegate void DataReceived(string msg);
        public event DataReceived OnDataRecieved;

        private struct Client
        {
            public EndPoint endPoint;
            public string name;
        }
        
        private List<Client> clientList;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            OnDataRecieved += MainWindow_OnDataRecieved;
            clientList = new List<Client>();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tbInfo.Text = "Server started";
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint server = new IPEndPoint(IPAddress.Any, 30000);

                serverSocket.Bind(server);
                tbInfo.Text += Environment.NewLine + "Listening";

                IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
                EndPoint epSender = (EndPoint)clients;
                serverSocket.BeginReceiveFrom(bytes, 0, 1024, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveData), epSender);
            }
            catch (Exception exc)
            {
                tbInfo.Text += Environment.NewLine + exc.Message;
            }
        }

        private void ReceiveData(IAsyncResult res)
        {
            try
            {
                IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
                EndPoint epSender = (EndPoint)clients;
                int bytesRec = serverSocket.EndReceiveFrom(res, ref epSender);

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                if (bytesRec != 1024)
                {          
                    if (OnDataRecieved != null)
                    {
                        OnDataRecieved(data);
                    }

                    if (data.EndsWith("connected"))
                    {
                        clientList.Add(new Client() { endPoint = epSender, name = data.Substring(0, data.IndexOf(" connected")) });
                    }
                    else if (data.EndsWith("disconnected"))
                    {                        
                        for(int i=0; i<clientList.Count; i++)
                        {
                            if (clientList[i].name == data.Substring(0, data.IndexOf(" connected")))
                            {
                                clientList.Remove(clientList[i]);
                                break;
                            }
                        }                        
                    }
                    else
                    {
                        dataSend = Encoding.UTF8.GetBytes(data);
                        for (int i = 0; i < clientList.Count; i++)
                        {
                            serverSocket.BeginSendTo(dataSend, 0, dataSend.Length > 1024 ? 1024 : dataSend.Length, SocketFlags.None, clientList[i].endPoint, new AsyncCallback(SendData), clientList[i].endPoint);
                        }
                    }
                    data = string.Empty;
                }
                                                                
                serverSocket.BeginReceiveFrom(bytes, 0, 1024, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveData), epSender);
                
            }
            catch (Exception exc)
            {
            }
        }

        public void SendData(IAsyncResult res)
        {
            try
            {
                int bytesSend = serverSocket.EndSend(res);
                offset += bytesSend;
                if (offset < dataSend.Length)
                {
                    for (int i = 0; i < clientList.Count; i++)
                    {
                        serverSocket.BeginSendTo(dataSend, offset, dataSend.Length - offset > 1024 ? 1024 : dataSend.Length - offset, SocketFlags.None, clientList[i].endPoint, new AsyncCallback(SendData), clientList[i].endPoint);
                    }                    
                }
                else
                {
                    offset = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendData Error: " + ex.Message);
            }
        }

        void MainWindow_OnDataRecieved(string msg)
        {
            Dispatcher.Invoke(new Action(
                delegate()
                {
                    tbInfo.Text += Environment.NewLine + msg;
                }));
            
        }
    }
}
