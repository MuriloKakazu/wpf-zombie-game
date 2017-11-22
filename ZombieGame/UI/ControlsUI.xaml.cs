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
using ZombieGame.Game.Prefabs.Sprites;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for ControlsUI.xaml
    /// </summary>
    public partial class ControlsUI : UserControl
    {
        public ControlsUI()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Image.Source = new KeyboardLayoutSprite().Image;
        }

        private void BackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ControlCache.PauseMenu.Grid.Children.Remove(this);
            this.Visibility = Visibility.Collapsed;
            ControlCache.PauseMenu.PausedMenuContent.Visibility = Visibility.Visible;
        }
    }
}
