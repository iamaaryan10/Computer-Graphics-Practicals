using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PaintEffectsDemo
{
    public partial class MainWindow : Window
    {
        List<Particle> particles = new List<Particle>();
        Random rand = new Random();
        DispatcherTimer timer;
        ImageBrush leafBrush;

        public MainWindow()
        {
            InitializeComponent();

            // Load leaf texture
            leafBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/leaf.png")));

            // Timer for animation
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += UpdateFrame;
            timer.Start();
        }

        void UpdateFrame(object sender, EventArgs e)
        {
            // Add fire + smoke particles
            for (int i = 0; i < 5; i++)
            {
                particles.Add(new Particle
                {
                    Position = new Point(400, 500),
                    Velocity = new Vector(rand.Next(-2, 2), -rand.Next(2, 6)),
                    Life = 1.0,
                    Type = rand.NextDouble() > 0.5 ? "fire" : "smoke"
                });
            }

            // Update particles
            foreach (var p in particles)
            {
                p.Position += p.Velocity;
                p.Life -= 0.02;
            }
            particles.RemoveAll(p => p.Life <= 0);

            // Redraw canvas
            DrawCanvas.Children.Clear();

            // Draw foliage (leaves at random positions)
            for (int i = 0; i < 8; i++)
            {
                System.Windows.Shapes.Rectangle leaf = new System.Windows.Shapes.Rectangle
                {
                    Width = 40,
                    Height = 40,
                    Fill = leafBrush
                };
                Canvas.SetLeft(leaf, rand.Next(100, 700));
                Canvas.SetTop(leaf, rand.Next(400, 550));
                DrawCanvas.Children.Add(leaf);
            }

            // Draw fire + smoke particles
            foreach (var p in particles)
            {
                SolidColorBrush brush;
                if (p.Type == "fire")
                {
                    brush = new SolidColorBrush(Color.FromArgb(
                        (byte)(p.Life * 255),
                        255, (byte)(200 * p.Life), 0));
                }
                else
                {
                    brush = new SolidColorBrush(Color.FromArgb(
                        (byte)(p.Life * 200),
                        180, 180, 180)); // smoke
                }

                System.Windows.Shapes.Ellipse circle = new System.Windows.Shapes.Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = brush
                };
                Canvas.SetLeft(circle, p.Position.X);
                Canvas.SetTop(circle, p.Position.Y);
                DrawCanvas.Children.Add(circle);
            }
        }
    }

    public class Particle
    {
        public Point Position { get; set; }
        public Vector Velocity { get; set; }
        public double Life { get; set; }
        public string Type { get; set; }
    }
}
