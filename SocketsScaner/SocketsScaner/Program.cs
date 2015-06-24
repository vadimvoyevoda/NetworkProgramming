using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketsScaner
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[1];
            List<int> freeSockets = new List<int>();

            Parallel.For(0, 400, (i) =>
                {
                    IPEndPoint ipEnd = new IPEndPoint(ipAddr, i);
                    Socket Listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        Listener.Connect(ipEnd);
                        freeSockets.Add(i);
                        Console.WriteLine("Port {0} is free", i);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Port {0} is busy", i);
                    }
                    finally
                    {
                        Listener.Close();
                    }
                }
                );

            Console.WriteLine("\nFree sockets:");
            Parallel.For(0, freeSockets.Count, (i) =>
                {
                    Console.WriteLine(freeSockets[i].ToString());
                });
        }
    }
}
