using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aquarium
{
    class Background
    {
        Image bgImage;
        int aquariumWidth = 800;

        public Background(Image backgroundImage)
        {
            bgImage = backgroundImage;
        }

        public void Load(String filename)
        {
            BitmapImage theBitmap = new BitmapImage();
            theBitmap.BeginInit();
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, filename);
            theBitmap.UriSource = new Uri(path, UriKind.Absolute);
            theBitmap.DecodePixelWidth = aquariumWidth;
            theBitmap.EndInit();
            bgImage.Source = theBitmap;
        }
    }
}
