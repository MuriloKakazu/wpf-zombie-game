using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZombieGame.Audio;
using ZombieGame.Game;
using ZombieGame.Game.Prefabs.Audio;
using ZombieGame.Game.Serializable;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for ChoosePlayerUI.xaml
    /// </summary>
    public partial class ChoosePlayerUI : UserControl
    {
        public SimpleWeapon w { get; protected set; }
        public SimpleProjectile p { get; protected set; }
        public bool IsWeapon { get { return p == null && w != null; } }
        public bool IsProjectile { get { return p != null && w == null; } }


        public ChoosePlayerUI()
        {
            InitializeComponent();
            imgP1.Source = new BitmapImage(new Uri(IO.GlobalPaths.CharacterSprites + "player1.png"));
            imgP2.Source = new BitmapImage(new Uri(IO.GlobalPaths.CharacterSprites + "player2.png"));
            Canvas.SetZIndex(this, 10);
        }

        public void SetItem(SimpleWeapon w)
        {
            this.w = w;
            p = null;
            lblName.Content = w.Name;
        }
        public void SetItem(SimpleProjectile p)
        {
            this.p = p;
            w = null;
            lblName.Content = p.Name;
        }

        private void btnP1_Click(object sender, RoutedEventArgs e)
        {
            if (IsWeapon)
            {
                GameMaster.Players[0].Character.SetWeapon(w.Mount());
                SoundPlayer.Instance.Play(new WeaponReloadSFX());
            }
            else if (IsProjectile)
            {
                GameMaster.Players[0].Character.Weapon.SetProjectile(p.Mount());
                SoundPlayer.Instance.Play(new WeaponReloadSFX());
                GameMaster.Players[0].Character.Weapon.Ammo = 0;
            }
            ControlCache.PauseMenu.Grid.Children.Remove(ControlCache.ChoosePlayer);
            ControlCache.PauseMenu.Grid.Children.Add(ControlCache.StoreControl);
        }

        private void btnP2_Click(object sender, RoutedEventArgs e)
        {
            if (IsWeapon)
            {
                GameMaster.Players[1].Character.SetWeapon(w.Mount());
                SoundPlayer.Instance.Play(new WeaponReloadSFX());
            }
            else if (IsProjectile)
            {
                GameMaster.Players[1].Character.Weapon.SetProjectile(p.Mount());
                GameMaster.Players[1].Character.Weapon.Ammo = 0;
                SoundPlayer.Instance.Play(new WeaponReloadSFX());
            }
            ControlCache.PauseMenu.Grid.Children.Remove(ControlCache.ChoosePlayer);
            ControlCache.PauseMenu.Grid.Children.Add(ControlCache.StoreControl);
        }
    }
}
