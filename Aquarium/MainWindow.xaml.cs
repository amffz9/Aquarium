using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Random:
// http://msdn.microsoft.com/en-us/library/system.random.aspx

namespace Aquarium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Background background;
        List<Fish> fishes;

        public MainWindow()
        {
            int numFish = 6;
            Int32 minWait = 10;
            Int32 maxWait = 50;
            double minDistanceFromTop = 0;
            double minDistFromBottom = 140;

            Fish fish;
            Random random;

            double x, y;
            String direction;
            Int32 wait;
            Double minFishDepth, maxFishDepth;
            Double aquariumWidth;
            String leftImage, rightImage;

            InitializeComponent();

            random = new Random();

            fishes = new List<Fish>();

            background = new Background(backgroundImage);
            background.Load("background1.png");
            bg1ImageRadio.IsChecked = true;

            aquariumWidth = aquarium.Width;
            minFishDepth = minDistanceFromTop;
            maxFishDepth = aquarium.Height - minDistFromBottom;

            for (int i = 0; i < numFish; i++)
            {
                Int32 fishType = random.Next(1, 4);
                switch (fishType)
                {
                    case 1:
                        leftImage = "fish1_left.png";
                        rightImage = "fish1_right.png";
                        break;
                    case 2:
                        leftImage = "fish2_left.png";
                        rightImage = "fish2_right.png";
                        break;
                    default:
                        leftImage = "fish3_left.png";
                        rightImage = "fish3_right.png";
                        break;
                }

                x = random.Next(0, (Int32)aquariumWidth);
                y = random.Next((Int32)minFishDepth, (Int32)maxFishDepth);
                Int32 dir = random.Next(1, 3);
                if (dir == 1) direction = "left";
                else direction = "right";
                wait = random.Next(minWait, maxWait);

                fish = new Fish(aquarium: aquarium, dispatcher: Dispatcher, leftImage: leftImage, rightImage: rightImage);
                fish.Place(x: x, y: y, direction: direction, wait:wait);
                fishes.Add(fish);
            }
        }

        private void bg1RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            background.Load("background1.png");
        }

        private void bg2RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            background.Load("background2.png");
        }

        private void bg3RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            background.Load("background3.png");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Fish fish in fishes)
            {
                fish.Shutdown();
            }
        }
    }
}
