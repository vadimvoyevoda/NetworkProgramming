using System;
using System.Collections.Generic;
using System.Linq;
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
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using GMap.NET;

namespace GMap_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GMapControl MainMap;
        Slider sl;

        public MainWindow()
        {            
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.SizeChanged += MainWindow_SizeChanged;
            this.MouseWheel += MainWindow_MouseWheel;
            this.MouseDown += MainWindow_MouseDown;
        }

        void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                GMapMarker marker = new GMapMarker(new PointLatLng(MainMap.Position.Lat, MainMap.Position.Lng));
                MainMap.Markers.Add(marker);                
            }
        }

        void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            sl.Value = MainMap.Zoom / MainMap.MaxZoom * sl.Maximum;            
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sl != null)
            {
                sl.Height = this.ActualHeight/3;
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainMap = new GMapControl();
            main.Children.Add(MainMap);
            MainMap.MapProvider = GMapProviders.OpenStreetMap;
            MainMap.Position = new PointLatLng(50.619900, 26.251617);
            MainMap.MinZoom = 1;
            MainMap.MaxZoom = 20;
            MainMap.Zoom = 9;
            
            sl = new Slider();
            sl.Orientation = Orientation.Vertical;            
            sl.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.Both;
            sl.TickFrequency = 10;            
            sl.Maximum = 100;
            sl.Minimum = 1;
            sl.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            sl.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            sl.Margin = new Thickness(5);
            sl.Value = MainMap.Zoom / MainMap.MaxZoom * sl.Maximum;
            sl.Height = this.Height / 3;
            sl.ValueChanged += sl_ValueChanged;
            main.Children.Add(sl);            
        }

        void sl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainMap.Zoom = sl.Value/sl.Maximum*MainMap.MaxZoom;
        }
    }
}
