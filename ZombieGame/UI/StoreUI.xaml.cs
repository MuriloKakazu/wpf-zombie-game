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
using ZombieGame.Game.Serializable;
using ZombieGame.Game.Enums;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for StoreUC.xaml
    /// </summary>
    public partial class StoreUC : UserControl
    {
        #region Properties
        public int MaxWIndex, MaxPIndex;
        public int CurWIndex = 0, CurPIndex = 0;
        public SimpleWeapon[] SellingWeapons = new SimpleWeapon[4];
        public SimpleProjectile[] SellingProjectiles = new SimpleProjectile[4];
        public bool IsOpen = false;
        #endregion

        #region Methods
        private void btnWeaponsLeft_Click(object sender, RoutedEventArgs e)
        {
            CurWIndex--;
            SetSellingWeapons();
            UpdateArrowButtons();
        }
        private void btnWeaponsRight_Click(object sender, RoutedEventArgs e)
        {
            CurWIndex++;
            SetSellingWeapons();
            UpdateArrowButtons();
        }
        private void btnProjectilesRight_Click(object sender, RoutedEventArgs e)
        {
            CurPIndex++;
            SetSellingProjectiles();
            UpdateArrowButtons();
        }
        private void btnProjectilesLeft_Click(object sender, RoutedEventArgs e)
        {
            CurPIndex--;
            SetSellingProjectiles();
            UpdateArrowButtons();
        }
        public void UpdateArrowButtons()
        {
            btnProjectilesLeft.IsEnabled = CurPIndex != 0;
            btnProjectilesRight.IsEnabled = CurPIndex != MaxPIndex;
            btnWeaponsLeft.IsEnabled = CurWIndex != 0;
            btnWeaponsRight.IsEnabled = CurWIndex != MaxWIndex;
        }

        public StoreUC()
        {
            InitializeComponent();
            Canvas.SetZIndex(this, Convert.ToInt32(ZIndex.UserInterface));
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            CurWIndex = 0;
            CurPIndex = 0;
            SetMaxIndex();
            Width = Physics.Vector.WindowSize.X;
            Height = Physics.Vector.WindowSize.Y;
            Refresh();
            lblDinheiro.Content = "Dinheiro: " + GameMaster.Money;
        }
        public void Refresh()
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = GameMaster.Camera.RigidBody.Position.X;
            tt.Y = -GameMaster.Camera.RigidBody.Position.Y;
            RenderTransform = tt;
            SetSellingItems();
            UpdateArrowButtons();
        }

        public void SetMaxIndex()
        {
            MaxWIndex = Store.SellingWeapons.Count - 4;
            MaxPIndex = Store.SellingProjectiles.Count - 4;
            if (MaxWIndex < 0)
                MaxWIndex = 0;
            if (MaxPIndex < 0)
                MaxPIndex = 0;
        }
        public void SetSellingItems()
        {
            SetSellingWeapons();
            SetSellingProjectiles();
        }
        private void SetSellingProjectiles()
        {
            for (int i = 0; i < SellingProjectiles.Length; i++)
            {
                if (Store.SellingProjectiles.Count > CurPIndex + i)
                    SellingProjectiles[i] = Store.SellingProjectiles[CurPIndex + i];
                else
                    SellingProjectiles[i] = null;
            }
            UpdateProjectileInferfaces();
        }
        private void SetSellingWeapons()
        {
            for (int i = 0; i < SellingWeapons.Length; i++)
            {
                if (Store.SellingWeapons.Count > CurWIndex + i)
                    SellingWeapons[i] = Store.SellingWeapons[CurWIndex + i];
                else
                    SellingWeapons[i] = null;
            }
            UpdateWeaponInterfaces();
        }
        private void UpdateWeaponInterfaces()
        {
            siWeapon1.SetSellingItem(SellingWeapons[0]);
            siWeapon2.SetSellingItem(SellingWeapons[1]);
            siWeapon3.SetSellingItem(SellingWeapons[2]);
            siWeapon4.SetSellingItem(SellingWeapons[3]);
        }
        private void UpdateProjectileInferfaces()
        {
            siProjectile1.SetSellingItem(SellingProjectiles[0]);
            siProjectile2.SetSellingItem(SellingProjectiles[1]);
            siProjectile3.SetSellingItem(SellingProjectiles[2]);
            siProjectile4.SetSellingItem(SellingProjectiles[3]);
        }
        #endregion
    }
}
