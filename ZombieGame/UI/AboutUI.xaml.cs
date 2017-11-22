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

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for AboutUI.xaml
    /// </summary>
    public partial class AboutUI : UserControl
    {
        public AboutUI()
        {
            InitializeComponent();
        }

        private void BackButon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            GameMaster.TargetWindow.MainMenu.Grid.Children.Remove(this);
            GameMaster.TargetWindow.MainMenu.ReturnToOriginalState();
        }
    }
}
