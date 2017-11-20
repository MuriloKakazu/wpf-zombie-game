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
            btnStore.Text.Content = "LOJA/INVENTÁRIO";
            btnSettings.Text.Content = "CONFIGURAÇÕES";
            btnMainMenu.Text.Content = "MENU PRINCIPAL";
        }

        public void Refresh()
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = GameMaster.Camera.RigidBody.Position.X;
            tt.Y = -GameMaster.Camera.RigidBody.Position.Y;
            RenderTransform = tt;
        }

        private void btnStore_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            Grid.Children.Add(UserControls.StoreControl);
        }

        private void btnSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            var m = new SettingsMenuUI();
            m.DockPanel.HorizontalAlignment = HorizontalAlignment.Center;
            m.AccessedFromMainMenu = false;
            Grid.Children.Add(m);
        }

        private void btnMainMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {            
            GameMaster.TargetCanvas.Visibility = Visibility.Collapsed;
            ResourceManager.DestroyEverything();
            GameMaster.TargetWindow.MainMenu.Visibility = Visibility.Visible;
            GameMaster.Started = false;
            GameMaster.TargetCanvas.ResetUI();
        }
    }
}
