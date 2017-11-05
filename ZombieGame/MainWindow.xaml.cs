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
using ZombieGame.Game.Prefabs.Characters;
using ZombieGame.Game.Prefabs.OtherEntities;
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

        Zombie Z { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GameMaster.Setup();
            GameMaster.UpdateTimer.Elapsed += UpdateTimer_Elapsed;

            Z = new Zombie();
            Z.RigidBody.SetPosition(new Physics.Vector(500, -500));

            BottomWall = new Wall(WallTypes.BottomWall);
            BottomWall.RigidBody.SetPosition(new Physics.Vector(0, -Camera.Height));
            BottomWall.RigidBody.Resize(new Physics.Vector(Camera.Width, 100));
            BottomWall.RigidBody.FixedPosition = true;
            TopWall = new Wall(WallTypes.TopWall);
            TopWall.RigidBody.SetPosition(new Physics.Vector(0, 100));
            TopWall.RigidBody.Resize(new Physics.Vector(Camera.Width, 100));
            TopWall.RigidBody.FixedPosition = true;
            LeftWall = new Wall(WallTypes.LeftWall);
            LeftWall.RigidBody.SetPosition(new Physics.Vector(-100, 0));
            LeftWall.RigidBody.Resize(new Physics.Vector(100, Camera.Height));
            LeftWall.RigidBody.FixedPosition = true;
            RightWall = new Wall(WallTypes.RightWall);
            RightWall.RigidBody.SetPosition(new Physics.Vector(Camera.Width, 0));
            RightWall.RigidBody.Resize(new Physics.Vector(100, Camera.Height));
            RightWall.RigidBody.FixedPosition = true;
        }

        /// <summary>
        /// Move os retângulos de acordo com a posição dos jogadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Camera.Children.Clear();

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

                    foreach (var ae in Entity.Entities.ToArray())
                    {
                        Camera.Children.Add(ae.VisualControl);
                    }
                }));
            }
            catch
            {

            }
        }
    }
}
