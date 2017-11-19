﻿using System;
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
        }
        public void Refresh()
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = GameMaster.Camera.RigidBody.Position.X;
            tt.Y = -GameMaster.Camera.RigidBody.Position.Y;
            RenderTransform = tt;
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
                SoundPlayer.Instance.Play(new WeaponReload());
            }
            else if (IsProjectile)
            {
                GameMaster.Players[0].Character.Weapon.SetProjectile(p.Mount());
                SoundPlayer.Instance.Play(new WeaponReload());
            }
            Application.Current.Windows.OfType<MainWindow>().
                    FirstOrDefault().RemoveFromUI(UserControls.ChoosePlayer);
            Application.Current.Windows.OfType<MainWindow>().
                FirstOrDefault().AddToUI(UserControls.StoreControl);
        }

        private void btnP2_Click(object sender, RoutedEventArgs e)
        {
            if (IsWeapon)
            {
                GameMaster.Players[1].Character.SetWeapon(w.Mount());
                SoundPlayer.Instance.Play(new WeaponReload());
            }
            else if (IsProjectile)
            {
                GameMaster.Players[1].Character.Weapon.SetProjectile(p.Mount());
                SoundPlayer.Instance.Play(new WeaponReload());
            }
            Application.Current.Windows.OfType<MainWindow>().
                    FirstOrDefault().RemoveFromUI(UserControls.ChoosePlayer);
            Application.Current.Windows.OfType<MainWindow>().
                FirstOrDefault().AddToUI(UserControls.StoreControl);
        }
    }
}
