using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessage(12000);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void SendMessage(int port)
        {
            byte[] bytes = new byte[1024];

            IPAddress ipAddr = IPAddress.Parse("10.7.5.10"); 
            //IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            //IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEnd = new IPEndPoint(ipAddr, port);

            Socket sSender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sSender.Connect(ipEnd);

            Console.WriteLine("Enter your message:");
            string message = Console.ReadLine();

            Console.WriteLine("Client connect with {0}", sSender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);

            int bytesSend = sSender.Send(msg);
            int bytesRec = sSender.Receive(bytes);

            Console.WriteLine("Answer from server : {0}", Encoding.UTF8.GetString(bytes,0,bytesRec));

            if (message.IndexOf("<TheEnd>") == -1)
                SendMessage(port);

            sSender.Shutdown(SocketShutdown.Both);
            sSender.Close();



        }
    }
}
