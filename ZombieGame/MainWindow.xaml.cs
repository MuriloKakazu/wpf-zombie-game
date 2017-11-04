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
using ZombieGame.Debug;
using ZombieGame.Game;
using ZombieGame.Game.Enums;
using ZombieGame.Physics;
using ZombieGame.Physics.Enums;
using ZombieGame.Physics.Extensions;

namespace ZombieGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entity BottomWall { get; set; }
        Entity TopWall { get; set; }
        Entity LeftWall { get; set; }
        Entity RightWall { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GameMaster.Setup();
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;

            BottomWall = new Entity("BottomWall", Tags.BottomWall);
            BottomWall.RigidBody.SetPosition(new Physics.Vector(700, -490));
            BottomWall.RigidBody.Resize(new Physics.Vector(200, 10));
            BottomWall.RigidBody.FixedPosition = true;
            TopWall = new Entity("TopWall", Tags.TopWall);
            TopWall.RigidBody.SetPosition(new Physics.Vector(700, -300));
            TopWall.RigidBody.Resize(new Physics.Vector(200, 10));
            TopWall.RigidBody.FixedPosition = true;
            LeftWall = new Entity("LeftWall", Tags.LeftWall);
            LeftWall.RigidBody.SetPosition(new Physics.Vector(700, -300));
            LeftWall.RigidBody.Resize(new Physics.Vector(10, 200));
            LeftWall.RigidBody.FixedPosition = true;
            RightWall = new Entity("RightWall", Tags.RightWall);
            RightWall.RigidBody.SetPosition(new Physics.Vector(890, -300));
            RightWall.RigidBody.Resize(new Physics.Vector(10, 200));
            RightWall.RigidBody.FixedPosition = true;
        }

        /// <summary>
        /// Move os retângulos de acordo com a posição dos jogadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var p1 = GameMaster.Player1;
                var p2 = GameMaster.Player2;
                var pos1 = p1.Character.RigidBody.Position;
                var pos2 = p2.Character.RigidBody.Position;

                Line line0 = new Line
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.Red,
                    X1 = p1.Character.RigidBody.CenterPoint.X,
                    Y1 = -p1.Character.RigidBody.CenterPoint.Y,
                    X2 = p2.Character.RigidBody.CenterPoint.X,
                    Y2 = -p2.Character.RigidBody.CenterPoint.Y,
                };
                Line line1 = new Line
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.DarkGreen,
                    X1 = p2.Character.RigidBody.CenterPoint.X,
                    Y1 = -p2.Character.RigidBody.CenterPoint.Y,
                    X2 = p2.Character.RigidBody.CenterPoint.X + p2.Character.RigidBody.Velocity.X,
                    Y2 = -(p2.Character.RigidBody.CenterPoint.Y + p2.Character.RigidBody.Velocity.Y),
                };
                Line line2 = new Line
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.Green,
                    X1 = p1.Character.RigidBody.CenterPoint.X,
                    Y1 = -p1.Character.RigidBody.CenterPoint.Y,
                    X2 = p1.Character.RigidBody.CenterPoint.X + p1.Character.RigidBody.Velocity.X,
                    Y2 = -(p1.Character.RigidBody.CenterPoint.Y + p1.Character.RigidBody.Velocity.Y),
                };
                Rectangle TopWallR = new Rectangle
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.Red,
                    Fill = Brushes.Red,
                    Width = TopWall.RigidBody.Size.X,
                    Height = TopWall.RigidBody.Size.Y
                };
                Rectangle BottomWallR = new Rectangle
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.Green,
                    Fill = Brushes.Green,
                    Width = BottomWall.RigidBody.Size.X,
                    Height = BottomWall.RigidBody.Size.Y
                };
                Rectangle LeftWallR = new Rectangle
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.Blue,
                    Fill = Brushes.Blue,
                    Width = LeftWall.RigidBody.Size.X,
                    Height = LeftWall.RigidBody.Size.Y
                };
                Rectangle RightWallR = new Rectangle
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.Black,
                    Fill = Brushes.Black,
                    Width = RightWall.RigidBody.Size.X,
                    Height = RightWall.RigidBody.Size.Y
                };

                Camera.Children.Clear();
                Camera.Children.Add(Rectangle1);
                Camera.Children.Add(Rectangle2);
                Camera.Children.Add(line0);
                Camera.Children.Add(line1);
                Camera.Children.Add(line2);
                Camera.Children.Add(TopWallR);
                Camera.Children.Add(BottomWallR);
                Camera.Children.Add(LeftWallR);
                Camera.Children.Add(RightWallR);

                Canvas.SetLeft(Rectangle1, pos1.X);
                Canvas.SetTop(Rectangle1, -pos1.Y);
                Rectangle1.Width = p1.Character.RigidBody.Size.X;
                Rectangle1.Height = p1.Character.RigidBody.Size.Y;
                Canvas.SetLeft(Rectangle2, pos2.X);
                Canvas.SetTop(Rectangle2, -pos2.Y);
                Rectangle2.Width = p2.Character.RigidBody.Size.X;
                Rectangle2.Height = p2.Character.RigidBody.Size.Y;
                Canvas.SetLeft(TopWallR, TopWall.RigidBody.Position.X);
                Canvas.SetTop(TopWallR, -TopWall.RigidBody.Position.Y);
                TopWallR.Width = TopWall.RigidBody.Size.X;
                TopWallR.Height = TopWall.RigidBody.Size.Y;
                Canvas.SetLeft(BottomWallR, BottomWall.RigidBody.Position.X);
                Canvas.SetTop(BottomWallR, -BottomWall.RigidBody.Position.Y);
                BottomWallR.Width = BottomWall.RigidBody.Size.X;
                BottomWallR.Height = BottomWall.RigidBody.Size.Y;
                Canvas.SetLeft(LeftWallR, LeftWall.RigidBody.Position.X);
                Canvas.SetTop(LeftWallR, -LeftWall.RigidBody.Position.Y);
                LeftWallR.Width = LeftWall.RigidBody.Size.X;
                LeftWallR.Height = LeftWall.RigidBody.Size.Y;
                Canvas.SetLeft(RightWallR, RightWall.RigidBody.Position.X);
                Canvas.SetTop(RightWallR, -RightWall.RigidBody.Position.Y);
                RightWallR.Width = RightWall.RigidBody.Size.X;
                RightWallR.Height = RightWall.RigidBody.Size.Y;
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DebugMonitor dm = new DebugMonitor();
            dm.Show();
        }
    }
}
