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
        public MainWindow()
        {
            InitializeComponent();
            GameMaster.Setup();
            P1Stats.AssociatedPlayer = GameMaster.GetPlayer(0);
            P2Stats.AssociatedPlayer = GameMaster.GetPlayer(1);
            P1Stats.Name.Content = "Player 1";
            P2Stats.Name.Content = "Player 2";
            Time.HighFrequencyTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public void AddToCamera(UIElement v)
        {
            Camera.Children.Add(v);
        }

        public void RemoveFromCamera(UIElement v)
        {
            Camera.Children.Remove(v);
        }

        public void AddToUI(UIElement v)
        {
            UI.Children.Add(v);
        }

        public void RemoveFromUI(UIElement v)
        {
            UI.Children.Remove(v);
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
                Physics.Vector v = Game.Prefabs.Entities.Camera.GetTopLeftFocusPoint();

                Camera.RenderTransform = new TranslateTransform(-v.X, v.Y);
                UI.RenderTransform = new TranslateTransform(-v.X, v.Y);
                P1Stats.RenderTransform = new TranslateTransform(v.X, -v.Y);
                P2Stats.RenderTransform = new TranslateTransform(v.X, -v.Y);
                GameInfo.RenderTransform = new TranslateTransform(v.X, -v.Y);
                UpdateUI();
            });
        }

        public void UpdateUI()
        {
            P1Stats.UpdateStats();
            if (GameMaster.Players.Length > 1)
                P2Stats.UpdateStats();
            GameInfo.Update();
        }

        private void Camera_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Camera_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
