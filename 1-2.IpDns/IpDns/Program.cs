using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IpDns
{
    class Program
    {
        static void Main(string[] args)
        {           
           IPAddress[] ip = Dns.GetHostAddresses("vk.com");

           foreach (IPAddress el in ip)
           {               
               Console.WriteLine(el.ToString());
               IPHostEntry ih = Dns.GetHostEntry(el);
               Console.WriteLine(ih.HostName);
           }
        }
    }
}
