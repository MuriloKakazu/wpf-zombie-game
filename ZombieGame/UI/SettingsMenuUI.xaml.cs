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
using ZombieGame.IO.Serialization;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenuUI : UserControl
    {
        public bool AccessedFromMainMenu { get; set; }

        public SettingsMenuUI()
        {
            InitializeComponent();

            SaveButton.Text.Content = "APPLY";
            BackButton.Text.Content = "BACK";

            if (GameMaster.Settings.AntiAliasingEnabled)
                AntiAliasComboBox.SelectedIndex = 0;
            else
                AntiAliasComboBox.SelectedIndex = 1;
            if (GameMaster.Settings.RenderScale == 1)
                RenderScaleComboBox.SelectedIndex = 0;
            else
                RenderScaleComboBox.SelectedIndex = 1;
            VolumeSlider.Value = GameMaster.Settings.Volume;
                
        }

        private void BackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            if (AccessedFromMainMenu)
            {
                GameMaster.TargetWindow.MainMenu.Grid.Children.Remove(this);
                GameMaster.TargetWindow.MainMenu.ReturnToOriginalState();
            }
            else
            {
                UserControls.PauseMenu.Grid.Children.Remove(this);
                UserControls.PauseMenu.PausedMenuContent.Visibility = Visibility.Visible;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeValue.Content = Math.Round(VolumeSlider.Value * 100);
        }

        private void SaveButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameMaster.Settings.Volume = (float)VolumeSlider.Value;
            if (AntiAliasComboBox.SelectedIndex == 0)
                GameMaster.Settings.AntiAliasingEnabled = true;
            else
                GameMaster.Settings.AntiAliasingEnabled = false;
            if (RenderScaleComboBox.SelectedIndex == 0)
                GameMaster.Settings.RenderScale = 1;
            else
                GameMaster.Settings.RenderScale = 2;

            GameMaster.Settings.SaveTo("settings.config");
            GameMaster.LoadSettings();

            SaveButton.Text.Content = "DONE!";
        }

        private void SaveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SaveButton.Text.Content = "APPLY";
        }
    }
}
