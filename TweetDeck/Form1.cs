using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TweetDeck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string initLocation = Properties.Settings.Default.InitialLocation;
            Point il = new Point(0, 0);
            Size sz = Size;
            if (!string.IsNullOrWhiteSpace(initLocation))
            {
                string[] parts = initLocation.Split(',');
                if (parts.Length >= 2)
                {
                    il = new Point(int.Parse(parts[0]), int.Parse(parts[1]));
                }
                if (parts.Length >= 4)
                {
                    sz = new Size(int.Parse(parts[2]), int.Parse(parts[3]));
                }
            }
            Size = sz;
            Location = il;



            var settings = new CefSettings();
            settings.CachePath = Environment.CurrentDirectory;
            
            Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);


            CefSharp.WinForms.ChromiumWebBrowser chrom = new CefSharp.WinForms.ChromiumWebBrowser("https://tweetdeck.twitter.com/");
            this.Controls.Add(chrom);
            chrom.BrowserSettings.ApplicationCache = CefSharp.CefState.Enabled;
            CefSharp.CefSettings sc = new CefSharp.CefSettings();
            
            chrom.BrowserSettings.WebSecurity = CefSharp.CefState.Enabled;
            chrom.Dock = DockStyle.Fill;
            chrom.Load("https://tweetdeck.twitter.com/");
            chrom.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Point location = Location;
            Size size = Size;
            if (WindowState != FormWindowState.Normal)
            {
                location = RestoreBounds.Location;
                size = RestoreBounds.Size;
            }
            string initLocation = string.Join(",", location.X, location.Y, size.Width, size.Height);
            Properties.Settings.Default.InitialLocation = initLocation;
            Properties.Settings.Default.Save();
        }
    }
}
