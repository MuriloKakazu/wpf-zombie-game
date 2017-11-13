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
        Physics.Vector CameraLocation = new Physics.Vector();

        public MainWindow()
        {
            InitializeComponent();
            GameMaster.Setup();
            Time.InternalTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public void AddVisualComponent(UIElement v)
        {
            Camera.Children.Add(v);
        }

        public void RemoveVisualComponent(UIElement v)
        {
            Camera.Children.Remove(v);
        }

        public void SetCameraOpacity(float value)
        {
            Camera.Opacity = value;
        }

        public Physics.Vector GetCanvasSize()
        {
            return new Physics.Vector(Camera.Width, Camera.Height);
        }

        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                TranslateTransform tt = new TranslateTransform();
                var p1Pos = GameMaster.GetPlayer(0).Character.RigidBody.CenterPoint;
                var p2Pos = GameMaster.GetPlayer(1).Character.RigidBody.CenterPoint;
                var newX = -(p1Pos.X + p2Pos.X) / 2 + Width / 2;
                var newY = (p1Pos.Y + p2Pos.Y) / 2 + Height / 2;

                if (-newX > -1274 && -newX < 1274)
                    tt.X = newX;
                else
                    tt.X = CameraLocation.X;

                if (-newY > -691 && -newY < 1382)
                    tt.Y = newY;
                else
                    tt.Y = CameraLocation.Y;

                CameraLocation = new Physics.Vector(tt.X, tt.Y);
                Camera.RenderTransform = tt;
            });
        }

        private void Camera_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Camera_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
