using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZombieGame.Game.Serializable;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for WeaponInfoUI.xaml
    /// </summary>
    public partial class WeaponInfoUI : UserControl
    {
        /// <summary>
        /// Arma sendo exibida
        /// </summary>
        public SimpleWeapon w { get; protected set; }

        /// <summary>
        /// ctor
        /// </summary>
        public WeaponInfoUI()
        {
            InitializeComponent();
            Canvas.SetZIndex(this, 10);
        }

        /// <summary>
        /// Método para definir a arma
        /// </summary>
        /// <param name="w"></param>
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
            ItemIcon.Source = new BitmapImage(new Uri(IO.GlobalPaths.WeaponSprites + w.SpriteFileName));
        }

        /// <summary>
        /// Evento do botão voltar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ControlCache.PauseMenu.Grid.Children.Remove(this);
            ControlCache.PauseMenu.Grid.Children.Add(ControlCache.StoreControl);
        }
    }
}
