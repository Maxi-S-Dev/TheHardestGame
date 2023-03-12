using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace TheHardestGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer DP = new DispatcherTimer();
        DispatcherTimer MovementTimer;

        int tickSpeed = 16;

        float speedY, speedX;

        float speed = 2;

        int test = 3;

        bool up = false, down = false, left = false, right = false;

        public MainWindow()
        {
            InitializeComponent();
            Canvas.Focus();

            MovementTimer = new DispatcherTimer(DispatcherPriority.Send);

            MovementTimer.Interval = TimeSpan.FromMilliseconds(10);
            MovementTimer.Tick += Movement;
           

            DP.Interval = TimeSpan.FromMilliseconds(tickSpeed);
            DP.Tick += GameTick;
            DP.Start();
        }

        void GameTick(object? sender, EventArgs e)
        {
            
        }

        private void Movement(object? sender, EventArgs e)
        {
            //speedX = speedY = 0;

            if (up) speedY -= speed;
            if (down) speedY += speed;

            //Trace.WriteLine(left + " " + speedX);
            if (left) speedX -= speed;
            if (right) speedX += speed;

            speedX = speedX * 0.5f;
            speedY = speedY * 0.5f;

            
            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + speedX);
            CollideX();

            Canvas.SetTop(Player, Canvas.GetTop(Player) + speedY);
            CollideY();
        }

        private void CollideX()
        {
            Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            foreach(var x in Canvas.Children.OfType<Rectangle>()) 
            {
                if ((string)x.Tag == "collider")
                {
                    Rect colliderHB = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (PlayerHB.IntersectsWith(colliderHB))
                    {
                        Canvas.SetLeft(Player, Canvas.GetLeft(Player) - speedX);
                        speedX = 0;
                        return;
                    }
                }
            }
        }


        private void CollideY()
        {
            Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            foreach (var x in Canvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "collider")
                {
                    Rect colliderHB = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (PlayerHB.IntersectsWith(colliderHB))
                    {
                        Canvas.SetTop(Player, Canvas.GetTop(Player) - speedY);
                        speedY = 0;
                        return;
                    }
                }
            }
        }

        private void ButtonPressed(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            switch (e.Key)
            {
                case Key.W: 
                    up = true;
                    break;
                
                case Key.S: 
                    down = true;
                    break;

                case Key.D:
                    right = true;
                    break;

                case Key.A:
                    left = true;
                    break;
            }
            MovementTimer.Start();
            Movement(sender, e);
        }

        private void ButtonReleased(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    up = false;
                    break;

                case Key.S:
                    down = false;
                    break;

                case Key.D:
                    right = false;
                    break;

                case Key.A:
                    left = false;
                    break;
            }

            if(!(left || right || up || down)) MovementTimer.Stop();
        }
    }
}
