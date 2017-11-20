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
using ZombieGame.Physics;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for GameCanvas.xaml
    /// </summary>
    public partial class GameCanvas : UserControl
    {
        public GameCanvas()
        {
            InitializeComponent();
        }

        public void Prepare()
        {
            P1Stats.AssociatedPlayer = GameMaster.GetPlayer(0);
            P2Stats.AssociatedPlayer = GameMaster.GetPlayer(1);
            P1Stats.PlayerName.Content = "Player 1";
            P2Stats.PlayerName.Content = "Player 2";
            Time.HighFrequencyTimer.Elapsed += HighFrequencyTimer_Elapsed;
        }

        public void AddChild(UIElement element)
        {
            Canvas.Children.Add(element);
        }

        public void RemoveChild(UIElement element)
        {
            Canvas.Children.Remove(element);
        }

        public void ResetUI()
        {
            RemoveChild(UserControls.PauseMenu);
        }

        private void HighFrequencyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                Physics.Vector v = Game.Prefabs.Entities.Camera.GetTopLeftFocusPoint();
                RenderTransform = new TranslateTransform(-v.X, v.Y);
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

    }
}
