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
    /// Interaction logic for DifficultySelectionUI.xaml
    /// </summary>
    public partial class DifficultySelectionUI : UserControl
    {
        public DifficultySelectionUI()
        {
            InitializeComponent();
        }

        private void EasyButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameMaster.Settings.Difficulty = Difficulty.Easy;
            Collapse();
        }

        private void MediumButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameMaster.Settings.Difficulty = Difficulty.Medium;
            Collapse();
        }

        private void HardButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameMaster.Settings.Difficulty = Difficulty.Hard;
            Collapse();
        }

        private void Collapse()
        {
            this.Visibility = Visibility.Collapsed;
            GameMaster.TargetCanvas.RemoveChild(ControlCache.PauseMenu);
            ControlCache.PauseMenu.PausedMenuContent.Visibility = Visibility.Visible;
            GameMaster.HideCursor();
            GameMaster.Resume();
            GameMaster.TargetCanvas.ShowUI();
            //GameMaster.TargetCanvas.Visibility = Visibility.Visible;
        }
    }
}
