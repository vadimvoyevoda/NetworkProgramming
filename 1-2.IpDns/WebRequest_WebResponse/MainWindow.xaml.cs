using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WebRequest_WebResponse
{ 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WebRequest wr = WebRequest.Create("http://www.vk.com");
            WebResponse resp;

            resp = wr.GetResponse();

            tbResponse.Text = "ContentLength: " + resp.ContentLength + 
                              " ContentType: " + resp.ContentType +
                              " ResponseUri: " + resp.ResponseUri +
                              " IsFromCache: " + resp.IsFromCache;

            using (Stream s = resp.GetResponseStream())
            {
                StreamReader sr = new StreamReader(s);
                tbRequest.Text = sr.ReadToEnd();
            }
        
        }
    }
}
