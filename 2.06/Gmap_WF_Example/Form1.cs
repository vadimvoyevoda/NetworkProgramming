using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gmap_WF_Example
{
    public partial class Form1 : Form
    {
        GMapControl MainMap;
        TrackBar sl;
        GMapOverlay markersOverlay;
        GMapOverlay routeOverlay;
        bool isAddMarker; 

        public Form1()
        {
            InitializeComponent();            
        }
                
        private void Form1_Load(object sender, EventArgs e)
        {
            MainMap = new GMapControl();
            MainMap.MapProvider = GMapProviders.OpenStreetMap;
            MainMap.Position = new PointLatLng(50.619900, 26.251617);
            MainMap.MinZoom = 3;
            MainMap.MaxZoom = 20;
            MainMap.Zoom = 9;
            MainMap.Dock = DockStyle.Fill;
            MainMap.MarkersEnabled = true;
            MainMap.PolygonsEnabled = true;
            MainMap.RoutesEnabled = true;
            MainMap.MouseClick += MainMap_MouseClick;
            MainMap.OnMarkerClick += MainMap_OnMarkerClick;
            //MainMap.OnPositionChanged += MainMap_OnPositionChanged;
            Controls.Add(MainMap);

            sl = new TrackBar();
            sl.Orientation = Orientation.Vertical;
            sl.TickStyle = TickStyle.Both;
            sl.TickFrequency = 10;
            sl.Maximum = 100;
            sl.Minimum = 1;
            sl.Value = (int)((MainMap.Zoom - MainMap.MinZoom)/ MainMap.MaxZoom * sl.Maximum);
            sl.Height = this.Height / 3;
            sl.Width = 50;
            sl.Location = new Point(MainMap.Width - sl.Width - 5, MainMap.Height - sl.Height -10);
            sl.ValueChanged += sl_ValueChanged;

            Controls.Add(sl);
            sl.BringToFront();

            markersOverlay = new GMapOverlay("markers");
            routeOverlay = new GMapOverlay("routes");
            isAddMarker = true;
        }

        void MainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            markersOverlay.Markers.Remove(item);
            isAddMarker = false;
            System.Threading.Timer timer = new System.Threading.Timer(
                new TimerCallback(delegate(object state) 
                {
                    BuildRoute();
                    isAddMarker = true;
                }));
            timer.Change(100,0);
        }
        
        //void MainMap_OnPositionChanged(PointLatLng point)
        //{            
        //}

        void MainMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && isAddMarker)
            {                
                GMarkerGoogle marker = new GMarkerGoogle(
                    MainMap.FromLocalToLatLng(e.X, e.Y),
                    GMarkerGoogleType.red);                
                  
                markersOverlay.Markers.Add(marker);                
                MainMap.Overlays.Add(markersOverlay);
                MainMap.UpdateMarkerLocalPosition(marker);

                BuildRoute();
            }
        }

        void BuildRoute()
        {
            if (markersOverlay.Markers.Count > 0)
            {
                List<PointLatLng> points = new List<PointLatLng>();

                for (int i = 0; i < markersOverlay.Markers.Count; i++)
                {
                    points.Add(markersOverlay.Markers[i].Position);
                }

                GMapRoute route = new GMapRoute(points, "myRoute");
                routeOverlay.Routes.Clear();
                routeOverlay.Routes.Add(route);
                MainMap.Overlays.Add(routeOverlay);
                MainMap.UpdateRouteLocalPosition(route);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {            
            if (sl != null)
            {
                sl.Height = this.Height / 3;
                sl.Location = new Point(MainMap.Width - sl.Width - 5, MainMap.Height - sl.Height - 10);        
                sl.BringToFront();
            }
        }


        private void sl_ValueChanged(object sender, EventArgs e)
        {
            MainMap.Zoom = (double)sl.Value / sl.Maximum * MainMap.MaxZoom + MainMap.MinZoom;
        }
    }
}
