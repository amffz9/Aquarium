using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Threading;
using System.Windows.Threading;

using System.Timers;

// Comparing the Timer Classes in the .NET Framework Class Library
// http://msdn.microsoft.com/en-us/magazine/cc164015.aspx

// Threads
// http://msdn.microsoft.com/en-us/library/1h2f2459.aspx

// Thread lock
// http://support.microsoft.com/default.aspx?scid=kb;en-us;816161

// Dispatcher
// http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatcher.aspx

namespace Aquarium
{
    class Fish
    {
        Canvas aquarium;
        Image fishImage;
        BitmapImage leftBitmap;
        BitmapImage rightBitmap;
        double aquariumWidth = 0.0;
        double fishWidth = 100.0;
        double maxX = 0.0;
        double incrementSize = 1.0;

        private double y;
        private double x;
        private Dispatcher dispatcher;
        private Int32 waitTime;
        private Boolean goRight = true;

        private Boolean updatePending = false;
        private Thread updateThread = null;

        System.Timers.Timer tmr;

        private int skips = 0;


        public Fish(Canvas aquarium, Dispatcher dispatcher, String leftImage, String rightImage)
        {
            this.aquarium = aquarium;
            this.dispatcher = dispatcher;
            aquariumWidth = (int)this.aquarium.Width;
            maxX = aquariumWidth - fishWidth;

            fishImage = new Image();
            fishImage.Width = fishWidth;

            leftBitmap = LoadBitmap(leftImage);
            rightBitmap = LoadBitmap(rightImage);
        }

        private BitmapImage LoadBitmap(String imageFile)
        {
            BitmapImage theBitmap = new BitmapImage();
            theBitmap.BeginInit();
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, imageFile);
            theBitmap.UriSource = new Uri(path, UriKind.Absolute);
            theBitmap.DecodePixelWidth = (int)fishWidth;
            theBitmap.EndInit();

            return theBitmap;
        }

        public void Place(double x = 100.0, double y = 200.0, String direction = "right", Int32 wait = 100)
        {
            switch (direction)
            {
                case "left":
                    {
                        fishImage.Source = leftBitmap;
                        goRight = false;
                        break;
                    }
                default:
                    {
                        fishImage.Source = rightBitmap;
                        goRight = true;
                        break;
                    }
            }

            this.waitTime = wait;
            this.x = x;
            this.y = y;
            aquarium.Children.Add(fishImage);
            fishImage.SetValue(Canvas.LeftProperty, this.x);
            fishImage.SetValue(Canvas.TopProperty, this.y);

            tmr = new System.Timers.Timer();
            tmr.Elapsed += new ElapsedEventHandler(DoUpdate);
            tmr.Interval = this.waitTime;
            tmr.Start();
        }

        void DoUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (goRight)
            {
                x += incrementSize;

                if (x > maxX)
                {
                    goRight = false;
                    UpdateUIImage(leftBitmap);
                }
            }
            else
            {
                x -= incrementSize;

                if (x < 0)
                {
                    goRight = true;
                    UpdateUIImage(rightBitmap);
                }
            }

            if (updatePending) { skips++; return; }
            updatePending = true;
            updateThread = new Thread(UpdateUIPosition);
            updateThread.Start();
        }

        void UpdateUIPosition()
        {
            Action posnAction = () => { fishImage.SetValue(Canvas.LeftProperty, x); fishImage.SetValue(Canvas.TopProperty, y); };
            dispatcher.Invoke(posnAction);  // synch invoke

            updatePending = false;
        }

        void UpdateUIImage(BitmapImage theBitmap)
        {
            Action bitmapAction = () => { fishImage.Source = theBitmap; };
            dispatcher.BeginInvoke(bitmapAction); // asynch invoke
        }

        public void Shutdown()
        {
            tmr.Stop();

            //string messageBoxText = "Number of skips: " + skips;
            //string caption = "Skips";
            //MessageBoxButton button = MessageBoxButton.OK;
            //MessageBoxImage icon = MessageBoxImage.Warning;
            //MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}