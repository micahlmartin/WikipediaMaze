using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Services.Interfaces;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WikipediaMaze.Services.Implementations
{
    public class WebSnapshotService : IWebSnapshotService
    {
        public System.Drawing.Image GetSnapshot(string url, int height, int width)
        {
            var imageMaker = new WebsiteImage(url, height, width);
            return imageMaker.GenerateWebSiteImage();
        }

        private class WebsiteImage
        {
            public WebsiteImage(string Url, int height, int width)
            {
                this.Url = Url;
                Height = height;
                Width = width;
            }

            public string Url { get; set; }
            public Bitmap Image { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public Image GenerateWebSiteImage()
            {
                Thread thread = new Thread(new ThreadStart(GenerateWebSiteImageInternal));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                return Image;
            }

            private void GenerateWebSiteImageInternal()
            {
                WebBrowser WebBrowser = new WebBrowser();
                WebBrowser.ScrollBarsEnabled = false;
                WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                WebBrowser.Navigate(Url);
                while (WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                    Application.DoEvents();
                WebBrowser.Dispose();
            }

            private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                WebBrowser WebBrowser = (WebBrowser)sender;
                WebBrowser.ClientSize = new Size(Width, Height);
                WebBrowser.ScrollBarsEnabled = false;
                Image = new Bitmap(WebBrowser.Bounds.Width, WebBrowser.Bounds.Height);
                WebBrowser.BringToFront();
                WebBrowser.DrawToBitmap(Image, WebBrowser.Bounds);
            }
        }
    }
}
