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

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for WeaponInfoUI.xaml
    /// </summary>
    public partial class WeaponInfoUI : UserControl
    {
        public SimpleWeapon w { get; protected set; }

        public WeaponInfoUI()
        {
            InitializeComponent();
        }

        public void SetWeapon(SimpleWeapon w)
        {
            this.w = w;
            lblName.Content = w.Name;
            lblFireRate.Content = "Cadência: " + w.FireRate.ToString() + "/min";
            lblMagSize.Content = "Carregador: " + w.MagSize + " balas";
            lblPrice.Content = "Preço: " + w.Price;
            lblReloadTime.Content = "Recarga: " + w.ReloadTime / 1000 + "s";

            string accepted = "";
            for (int i = 0; i < w.AcceptedProjectileTypes.Length; i++)
            {
                accepted += w.AcceptedProjectileTypes[i].ToString();
                if (i != w.AcceptedProjectileTypes.Length - 1)
                    accepted += ", ";
            }

            lblAcceptedPrectiles.Content = "Projéteis aceitos: " + accepted;
            //imgWeapon.Source = new BitmapImage(new Uri(IO.GlobalPaths.WeaponSprites + w.SpriteFileName));
        }

        public void Refresh()
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = GameMaster.Camera.RigidBody.Position.X;
            tt.Y = -GameMaster.Camera.RigidBody.Position.Y;
            RenderTransform = tt;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            GameMaster.TargetCanvas.RemoveChild(this);
            GameMaster.TargetCanvas.AddChild(UserControls.StoreControl);
        }
    }
}
