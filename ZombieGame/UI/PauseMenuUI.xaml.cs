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
using ZombieGame.Game.Enums;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for PauseMenuUI.xaml
    /// </summary>
    public partial class PauseMenuUI : UserControl
    {
        public PauseMenuUI()
        {
            InitializeComponent();
            Canvas.SetZIndex(this, 11);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        public void ShowDarkBackground()
        {
            Background = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
            GradRect.Fill = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
        }

        public void HideDarkDarkBackground()
        {
            Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            GradRect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        public void Refresh()
        {
            if (GameMaster.Score > 0)
            {
                TranslateTransform tt = new TranslateTransform();
                if (GameMaster.GetPlayer(0).Character != null && GameMaster.GetPlayer(1).Character != null ||
                    GameMaster.GetPlayer(0).Character == null && GameMaster.GetPlayer(1).Character != null ||
                    GameMaster.GetPlayer(0).Character != null && GameMaster.GetPlayer(1).Character == null)
                {
                    tt.X = GameMaster.Camera.RigidBody.Position.X;
                    tt.Y = -GameMaster.Camera.RigidBody.Position.Y;
                }
                else
                {
                    tt.X = 0;
                    tt.Y = 0;
                    GameMaster.TargetCanvas.RenderTransform = tt;
                }
                RenderTransform = tt;
            }
            else
                RenderTransform = new TranslateTransform(0, 0);
        }

        private void btnStore_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            Grid.Children.Add(ControlCache.StoreControl);
        }

        private void btnSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            var m = new SettingsMenuUI(fromMainMenu: false);
            m.DockPanel.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.Children.Add(m);
        }

        private void btnMainMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            btnMainMenu.Content = "CLIQUE 2X PARA CONFIRMAR";
        }

        private void btnMainMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GameMaster.TargetCanvas.Visibility = Visibility.Collapsed;
            ResourceManager.DestroyEverything();
            GameMaster.TargetWindow.MainMenu.Visibility = Visibility.Visible;
            GameMaster.Started = false;
            GameMaster.TargetCanvas.ResetUI();
        }

        private void btnMainMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            btnMainMenu.Content = "SAIR";
        }

        private void btnResume_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            GameMaster.Resume();
            GameMaster.TargetCanvas.RemoveChild(ControlCache.PauseMenu);
            ControlCache.PauseMenu.Grid.Children.Clear();
            ControlCache.PauseMenu = new PauseMenuUI();
            GameMaster.HideCursor();
        }

        private void btnControls_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            var m = new ControlsUI();
            Grid.Children.Add(m);
        }
    }
}
