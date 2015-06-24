using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.ai-yai-yai.pe.hu/text.htm");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("u259512832", "16101991");

            StreamReader reader = new StreamReader("Text.txt");
            byte[] content = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            reader.Close();
            request.ContentLength = content.Length;
                        
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(content, 0, content.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Ftp Upload file");
            Console.WriteLine(response.StatusDescription);
                        
            response.Close();
        }
    }
}
