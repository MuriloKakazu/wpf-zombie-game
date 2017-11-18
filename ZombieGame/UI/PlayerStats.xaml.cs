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
    /// Interaction logic for StatsBar.xaml
    /// </summary>
    public partial class PlayerStats : UserControl
    {
        public Player AssociatedPlayer
        {
            get { return (Player)GetValue(AssociatedPlayerProperty); }
            set { SetValue(AssociatedPlayerProperty, value); }
        }

        public static readonly DependencyProperty AssociatedPlayerProperty =
            DependencyProperty.Register("AssociatedPlayer", typeof(Player), typeof(PlayerStats), new PropertyMetadata(null));

        public PlayerStats()
        {
            InitializeComponent();

            AssociatedPlayer = new Player();
            healthBar.Background = new SolidColorBrush(Color.FromArgb(200, 255, 50, 50));
            ammoBar.Background = new SolidColorBrush(Color.FromArgb(200, 70, 70, 255));
            healthBar.Title = "HP:";
            ammoBar.Title = "AMMO:";
        }

        public void UpdateStats()
        {
            healthBar.FillPercentage = AssociatedPlayer.Character.Health / AssociatedPlayer.Character.MaxHealth * 100;
            healthBar.ProgressText = string.Format("{0} / {1}", AssociatedPlayer.Character.Health, AssociatedPlayer.Character.MaxHealth);

            var ammoPerc = (double)AssociatedPlayer.Character.Weapon.Ammo / (double)AssociatedPlayer.Character.Weapon.MagSize * 100;
            ammoBar.FillPercentage = ammoPerc;
            ammoBar.ProgressText = string.Format("{0} / {1}", AssociatedPlayer.Character.Weapon.Ammo, AssociatedPlayer.Character.Weapon.MagSize);

            if (ammoPerc <= 25)
                Tip.Content = "Low ammo!";
            else if (AssociatedPlayer.PlayerNumber == 2 && !AssociatedPlayer.IsPlaying)
                Tip.Content = "Press ENTER to play!";
            else
                Tip.Content = "";
        }
    }
}
