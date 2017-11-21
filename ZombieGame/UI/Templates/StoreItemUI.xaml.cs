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
using ZombieGame.Audio;
using ZombieGame.Game;
using ZombieGame.Game.Prefabs.Audio;
using ZombieGame.Game.Serializable;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for StoreItemUI.xaml
    /// </summary>
    public partial class StoreItemUI : UserControl
    {
        #region Properties
        public SimpleWeapon w { get; protected set; }
        public SimpleProjectile p { get; protected set; }
        public bool IsSellingWeapon { get { return p == null && w != null; } }
        public bool IsSellingProjectile { get { return p != null && w == null; } }
        #endregion

        #region Methods
        public StoreItemUI()
        {
            InitializeComponent();
        }

        public void SetSellingItem(SimpleWeapon w)
        {
            this.w = w;
            p = null;
            lblName.Content = w.Name;
            img.Source = new BitmapImage(new Uri(IO.GlobalPaths.WeaponSprites + w.SpriteFileName));
            SetBtnStatus();
        }
        public void SetSellingItem(SimpleProjectile p)
        {
            this.p = p;
            w = null;
            lblName.Content = p.Name;
            img.Source = new BitmapImage(new Uri(IO.GlobalPaths.ProjectileSprites + p.SpriteFileName));
            SetBtnStatus();
        }
        public void SetSellingNull()
        {
            w = null;
            p = null;
            lblName.Content = "";
            img.Source = null;
            SetBtnStatus();
        }
        public void SetBtnStatus()
        {
            if (IsSellingWeapon)
            {
                if (!w.Sold)
                {
                    btnBuy.Content = "$" + w.Price;
                    if (GameMaster.Money >= w.Price)
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(20, 100, 0));
                        btnBuy.IsEnabled = true;
                    }
                    else
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(150, 0, 0));
                        btnBuy.IsEnabled = false;
                    }
              }
                else
                {
                    btnBuy.Background = new SolidColorBrush(Color.FromRgb(200, 150, 0));
                    btnBuy.Content = "Comprado";
                    btnBuy.IsEnabled = true;
                }
            }
            else if (IsSellingProjectile)
            {
                if (!p.Sold)
                {
                    btnBuy.Content = "$" + p.Price;
                    if (GameMaster.Money >= p.Price)
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(20, 100, 0));
                        btnBuy.IsEnabled = true;
                    }
                    else
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(150, 0, 0));
                        btnBuy.IsEnabled = false;
                    }
                }
                else if (GameMaster.Players[0].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type) ||
                         GameMaster.Players[1].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type))
                {
                    btnBuy.Background = new SolidColorBrush(Color.FromRgb(200, 150, 0));
                    btnBuy.Content = "Comprado";
                    btnBuy.IsEnabled = true;
                }
                else
                {
                    btnBuy.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                    btnBuy.Content = "Não compatível";
                    btnBuy.IsEnabled = false;
                }
            }
            else
            {
                btnBuy.IsEnabled = false;
                btnBuy.Background = new SolidColorBrush(Color.FromRgb(110, 110, 110));
                btnBuy.Content = "";
            }
        }
        private void btnBuy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsSellingWeapon)
            {
                if (btnBuy.Content.Equals("Comprar"))
                {
                    Store.BuyWeapon(w.ItemID);
                    SoundPlayer.Instance.Play(new NoAmmoSFX());
                }
                else if (btnBuy.Content.Equals("Equipar"))
                {
                    if (GameMaster.Players[1].IsPlaying)
                    {
                        ControlCache.ChoosePlayer.SetItem(w);
                        ControlCache.PauseMenu.Grid.Children.Remove(ControlCache.StoreControl);
                        ControlCache.PauseMenu.Grid.Children.Add(ControlCache.ChoosePlayer);
                        SoundPlayer.Instance.Play(new NoAmmoSFX());
                    }
                    else
                    {
                        GameMaster.Players[0].Character.SetWeapon(w.Mount());
                        SoundPlayer.Instance.Play(new WeaponReloadSFX());
                    }
                }
                else
                {
                    btnBuy.IsEnabled = false;
                }
            }
            else if (IsSellingProjectile)
            {
                if (btnBuy.Content.Equals("Comprar"))
                {
                    Store.BuyProjectile(p.ItemID);
                    SoundPlayer.Instance.Play(new NoAmmoSFX());
                }
                else if (btnBuy.Content.Equals("Equipar"))
                {
                    if (GameMaster.Players[1].IsPlaying)
                    {
                        if (GameMaster.Players[0].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type) &&
                            GameMaster.Players[1].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type))
                        {
                            ControlCache.ChoosePlayer.SetItem(p);
                            ControlCache.PauseMenu.Grid.Children.Remove(ControlCache.StoreControl);
                            ControlCache.PauseMenu.Grid.Children.Add(ControlCache.ChoosePlayer);
                            SoundPlayer.Instance.Play(new NoAmmoSFX());
                        }
                        else if (GameMaster.Players[0].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type))
                        {
                            GameMaster.Players[0].Character.Weapon.SetProjectile(p.Mount());
                            GameMaster.Players[0].Character.Weapon.Ammo = 0;
                            SoundPlayer.Instance.Play(new WeaponReloadSFX());
                        }
                        else if (GameMaster.Players[1].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type))
                        {
                            GameMaster.Players[1].Character.Weapon.SetProjectile(p.Mount());
                            GameMaster.Players[1].Character.Weapon.Ammo = 0;
                            SoundPlayer.Instance.Play(new WeaponReloadSFX());
                        }
                    }
                    else
                    {
                        GameMaster.Players[0].Character.Weapon.SetProjectile(p.Mount());
                        GameMaster.Players[0].Character.Weapon.Ammo = 0;
                        SoundPlayer.Instance.Play(new WeaponReloadSFX());
                    }
                }
                else
                {
                    btnBuy.IsEnabled = false;
                }
            }
            SetBtnStatus();
            ControlCache.StoreControl.Update();
        }

        private void btnInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsSellingWeapon)
            {
                ControlCache.WeaponInfo.SetWeapon(w);
                ControlCache.PauseMenu.Grid.Children.Remove(ControlCache.StoreControl);
                ControlCache.PauseMenu.Grid.Children.Add(ControlCache.WeaponInfo);
            }
            else if (IsSellingProjectile)
            {
                ControlCache.ProjectileInfo.SetProjectile(p);
                ControlCache.PauseMenu.Grid.Children.Remove(ControlCache.StoreControl);
                ControlCache.PauseMenu.Grid.Children.Add(ControlCache.ProjectileInfo);
            }
        }
        #endregion

        private void btnBuy_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsSellingWeapon)
            {
                if (!w.Sold)
                {
                    if (GameMaster.Money >= w.Price)
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(0, 80, 0));
                        btnBuy.Content = "Comprar";
                        btnBuy.IsEnabled = true;
                    }
                    else
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                        btnBuy.Content = "$" + w.Price;
                        btnBuy.IsEnabled = false;
                    }
                }
                else
                {
                    btnBuy.Background = new SolidColorBrush(Color.FromRgb(170, 120, 0));
                    btnBuy.Content = "Equipar";
                    btnBuy.IsEnabled = true;
                }
            }
            else if (IsSellingProjectile)
            {
                if (!p.Sold)
                {
                    if (GameMaster.Money >= p.Price)
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(0, 50, 0));
                        btnBuy.Content = "Comprar";
                        btnBuy.IsEnabled = true;
                    }
                    else
                    {
                        btnBuy.Background = new SolidColorBrush(Color.FromRgb(130, 0, 0));
                        btnBuy.Content = "$" + p.Price;
                        btnBuy.IsEnabled = false;
                    }
                }
                else if (GameMaster.Players[0].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type) ||
                         GameMaster.Players[1].Character.Weapon.AcceptedProjectileTypes.Contains(p.Type))
                {
                    btnBuy.Background = new SolidColorBrush(Color.FromRgb(170, 120, 0));
                    btnBuy.Content = "Equipar";
                    btnBuy.IsEnabled = true;
                }
                else
                {
                    btnBuy.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70));
                    btnBuy.Content = "Não compatível";
                    btnBuy.IsEnabled = false;
                }
            }
            else
            {
                btnBuy.IsEnabled = false;
                btnBuy.Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                btnBuy.Content = "";
            }
        }

        private void btnBuy_MouseLeave(object sender, MouseEventArgs e)
        {
            SetBtnStatus();
        }
    }
}
