using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Client
    {        
        IPAddress ipAddr;
        IPEndPoint ipEnd;

        Socket sSender;
                
        byte[] bytes;
        int offset = 0;
        string fileName;        

        public Client(string ip, int port)
        {
            ipAddr = IPAddress.Parse(ip); 
            ipEnd = new IPEndPoint(ipAddr, port);
        }

        public void Send(string message, bool isFile)
        {
            try
            {
                SerializationClass serCl = new SerializationClass();
                
                if (isFile)
                {
                    serCl.FileName = message.Substring(message.LastIndexOf('\\')+1);
                    serCl.Command = "SendFile";
                    using (FileStream file = new FileStream(message, FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(file);
                        string cont = sr.ReadToEnd();
                        serCl.Content = Encoding.UTF8.GetBytes(cont);
                    }
                }
                else
                {
                    serCl.FileName = message;
                    serCl.Content = Encoding.UTF8.GetBytes(message);
                    serCl.Command = "SendMessage";                    
                }
                string msg = JsonConvert.SerializeObject(serCl);

                bytes = Encoding.UTF8.GetBytes(msg);
                sSender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);            
                AsyncCallback acConn = new AsyncCallback(OnConnect);
                sSender.BeginConnect(ipEnd, acConn, sSender);                
            }
            catch (Exception exc)
            {

            }

        }

        private void OnConnect(IAsyncResult res)
        {
            var mySocket = (res.AsyncState as Socket);
            if (mySocket != null)
            {
                mySocket.EndConnect(res);
                                
                offset = 0;
                mySocket.BeginSend(bytes, 0, bytes.Length > 1024 ? 1024 : bytes.Length, SocketFlags.None, new AsyncCallback(OnSend), mySocket);
            }
        }

        private void OnSend(IAsyncResult res)
        {
            var mySocket = (res.AsyncState as Socket);
            if (mySocket != null)
            {
                int bytesSend = mySocket.EndSend(res);
                offset += bytesSend;
                if (offset < bytes.Length)
                {
                    mySocket.BeginSend(bytes, offset, bytes.Length - offset > 1024 ? 1024 : bytes.Length - offset, SocketFlags.None, new AsyncCallback(OnSend), mySocket);
                }
                else
                {
                    mySocket.Shutdown(SocketShutdown.Both);
                    mySocket.Close();
                }
            }
        }
    }
}
