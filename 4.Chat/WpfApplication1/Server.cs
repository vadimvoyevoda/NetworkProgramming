using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Server
    {
        IPAddress ipAddr;
        IPEndPoint ipEnd;

        Socket sListener;

        string data;
        byte[] bytes;

        public delegate void DataReceived(string msg);
        public event DataReceived OnDataRecieved;

        public Server(string ip, int port)
        {
            ipAddr = IPAddress.Parse(ip); 
            ipEnd = new IPEndPoint(ipAddr, port);

            sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        
            bytes = new byte[1024];
        }

        public void Listen()
        {
            try
            {
                sListener.Bind(ipEnd);
                sListener.Listen(10);

                AsyncCallback acAcc = new AsyncCallback(OnAccept);
                sListener.BeginAccept(acAcc, sListener);                
            }
            catch (Exception exc)
            {
               
            }

        }

        private void OnAccept(IAsyncResult res)
        {
            var mySocket = (res.AsyncState as Socket);
            if (mySocket != null)
            {
                Socket newSocket = mySocket.EndAccept(res);
                mySocket.BeginAccept(OnAccept, mySocket);

                data = String.Empty;

                newSocket.BeginReceive(bytes, 0, 1024, SocketFlags.None, new AsyncCallback(OnReceive), newSocket);
            }
        }

        private void OnReceive(IAsyncResult res)
        {
            var mySocket = (res.AsyncState as Socket);
            if (mySocket != null)
            {
                int bytesRec = mySocket.EndReceive(res);
                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                if (bytesRec == 1024)
                {
                    mySocket.BeginReceive(bytes, 0, 1024, SocketFlags.None, new AsyncCallback(OnReceive), mySocket);
                }
                else
                {
                    mySocket.Shutdown(SocketShutdown.Both);
                    mySocket.Close();

                    if (OnDataRecieved != null)
                    {
                        OnDataRecieved(data);
                    }
                }
            }
        }
    }
}
