using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace BallAnimation
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddBall(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SolidColorBrush color = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
            double radius = 20;
            Ellipse ball = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = color,
            };

            // Add the ball to the canvas
            canvas.Children.Add(ball);

            // Set initial position
            Canvas.SetLeft(ball, e.GetPosition(canvas).X - radius);
            Canvas.SetTop(ball, e.GetPosition(canvas).Y - radius);

            // Start animation for the new ball
            StartBallAnimation(ball);
        }

        private void StartAnimation(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in canvas.Children)
            {
                if (element is Ellipse ball)
                {
                    StartBallAnimation(ball);
                }
            }
        }

        private void StartBallAnimation(Ellipse ball)
        {
            double startX = Canvas.GetLeft(ball);
            double startY = Canvas.GetTop(ball);

            double endX = random.NextDouble() * (canvas.ActualWidth - ball.Width);
            double endY = random.NextDouble() * (canvas.ActualHeight - ball.Height);

            Duration duration = TimeSpan.FromSeconds(random.Next(3, 6));
            DoubleAnimation animationX = new DoubleAnimation(startX, endX, duration);
            DoubleAnimation animationY = new DoubleAnimation(startY, endY, duration);

            animationX.AutoReverse = true;
            animationY.AutoReverse = true;

            animationX.Completed += (s, args) =>
            {
                double newX = Canvas.GetLeft(ball);
                if (newX <= 0 || newX >= canvas.ActualWidth - ball.Width)
                {
                    animationX.To = newX <= 0 ? 0 : canvas.ActualWidth - ball.Width;
                    animationX.AutoReverse = !animationX.AutoReverse;
                }
            };

            animationY.Completed += (s, args) =>
            {
                double newY = Canvas.GetTop(ball);
                if (newY <= 0 || newY >= canvas.ActualHeight - ball.Height)
                {
                    animationY.To = newY <= 0 ? 0 : canvas.ActualHeight - ball.Height;
                    animationY.AutoReverse = !animationY.AutoReverse;
                }
            };

            ball.BeginAnimation(Canvas.LeftProperty, animationX);
            ball.BeginAnimation(Canvas.TopProperty, animationY);
        }
    }
}
