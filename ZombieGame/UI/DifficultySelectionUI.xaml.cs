using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            ControlCache.PauseMenu.HideDarkDarkBackground();
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
            ControlCache.PauseMenu.ShowDarkBackground();
            GameMaster.TargetCanvas.RemoveChild(ControlCache.PauseMenu);
            ControlCache.PauseMenu.Grid.Children.Clear();
            ControlCache.PauseMenu = new PauseMenuUI();
        }
    }
}
