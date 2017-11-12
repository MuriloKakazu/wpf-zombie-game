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
using ZombieGame.Game.Controls;
using ZombieGame.Game.Enums;
using ZombieGame.Game.Prefabs.Entities;
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
        Wall BottomWall { get; set; }
        Wall TopWall { get; set; }
        Wall LeftWall { get; set; }
        Wall RightWall { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GameMaster.Setup();
            Time.InternalTimer.Elapsed += UpdateTimer_Elapsed;

            BottomWall = new Wall(WallTypes.BottomWall);
            BottomWall.RigidBody.SetPosition(new Physics.Vector(0, -Camera.Height + 10));
            BottomWall.RigidBody.Resize(new Physics.Vector(Camera.Width, 100));
            BottomWall.RigidBody.Freeze();
            TopWall = new Wall(WallTypes.TopWall);
            TopWall.RigidBody.SetPosition(new Physics.Vector(0, 99));
            TopWall.RigidBody.Resize(new Physics.Vector(Camera.Width, 100));
            TopWall.RigidBody.Freeze();
            LeftWall = new Wall(WallTypes.LeftWall);
            LeftWall.RigidBody.SetPosition(new Physics.Vector(-100, 0));
            LeftWall.RigidBody.Resize(new Physics.Vector(100, Camera.Height));
            LeftWall.RigidBody.Freeze();
            RightWall = new Wall(WallTypes.RightWall);
            RightWall.RigidBody.SetPosition(new Physics.Vector(Camera.Width - 10, 0));
            RightWall.RigidBody.Resize(new Physics.Vector(100, Camera.Height));
            RightWall.RigidBody.Freeze();
        }

        public void AddVisualComponent(VisualControl v)
        {
            Camera.Children.Add(v);
        }

        public void RemoveVisualComponent(VisualControl v)
        {
            Camera.Children.Remove(v);
        }

        public void SetCameraOpacity(float value)
        {
            Camera.Opacity = value;
        }


        /// <summary>
        /// Move os retângulos de acordo com a posição dos jogadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //try
            //{
            //    Dispatcher.Invoke(new Action(() =>
            //    {
            //        Camera.Children.Clear();

                    //Line l0 = new Line
                    //{
                    //    StrokeThickness = 1,
                    //    Stroke = Brushes.Red,
                    //    X1 = GameMaster.Player1.Character.RigidBody.CenterPoint.X,
                    //    Y1 = -GameMaster.Player1.Character.RigidBody.CenterPoint.Y,
                    //    X2 = (GameMaster.Player1.Character.RigidBody.CenterPoint - GameMaster.Player1.Character.RigidBody.Front * -1000).X,
                    //    Y2 = -(GameMaster.Player1.Character.RigidBody.CenterPoint - GameMaster.Player1.Character.RigidBody.Front * -1000).Y,
                    //};
                    //Ellipse el = new Ellipse
                    //{
                    //    StrokeThickness = 10,
                    //    Fill = Brushes.Red,
                    //    Width = 200,
                    //    Height = 200,
                    //    Stretch = Stretch.Uniform,
                    //};
                    //Canvas.SetLeft(el, GameMaster.Player2.Character.RigidBody.CenterPoint.X);
                    //Canvas.SetTop(el, -GameMaster.Player2.Character.RigidBody.CenterPoint.Y);
                    //Camera.Children.Add(el);
                    //Camera.Children.Add(l0);

                    //Camera.Children.Add(new Image { Source = new BitmapImage(new Uri(@"C:\Users\yurik\source\repos\clones\zombiegame\ZombieGame\bin\Debug\resources\scenes\defaultScene\bg.png")) });

                    //foreach (var p in Projectile.GetAllActiveProjectiles())
                    //{
                    //    Camera.Children.Add(p.VisualControl);
                    //}
        //            foreach (var ee in Enemy.Enemies.ToArray())
        //            {
        //                Line healthBar = new Line
        //                {
        //                    StrokeThickness = 2,
        //                    Stroke = Brushes.Red,
        //                    X1 = ee.RigidBody.Position.X,
        //                    Y1 = -ee.RigidBody.Position.Y - 10,
        //                    X2 = ee.RigidBody.Position.X + ee.Health,
        //                    Y2 = -ee.RigidBody.Position.Y - 10,
        //                };
        //                Camera.Children.Add(ee.VisualControl);
        //                Camera.Children.Add(healthBar);
        //            }
        //            Line healthBar1 = new Line
        //            {
        //                StrokeThickness = 2,
        //                Stroke = Brushes.Red,
        //                X1 = GameMaster.Player2.Character.RigidBody.Position.X,
        //                Y1 = -GameMaster.Player2.Character.RigidBody.Position.Y - 20,
        //                X2 = GameMaster.Player2.Character.RigidBody.Position.X + GameMaster.Player2.Character.Health,
        //                Y2 = -GameMaster.Player2.Character.RigidBody.Position.Y - 20,
        //            };
        //            Line healthBar2 = new Line
        //            {
        //                StrokeThickness = 2,
        //                Stroke = Brushes.Red,
        //                X1 = GameMaster.Player2.Character.RigidBody.Position.X,
        //                Y1 = -GameMaster.Player2.Character.RigidBody.Position.Y - 20,
        //                X2 = GameMaster.Player2.Character.RigidBody.Position.X + GameMaster.Player2.Character.Health,
        //                Y2 = -GameMaster.Player2.Character.RigidBody.Position.Y - 20,
        //            };
        //            Camera.Children.Add(GameMaster.Player2.Character.VisualControl);
        //            Camera.Children.Add(GameMaster.Player1.Character.VisualControl);
        //            Camera.Children.Add(healthBar1);
        //            Camera.Children.Add(healthBar2);
        //        }));
        //    }
        //    catch
        //    {

        //    }
        }
    }
}
