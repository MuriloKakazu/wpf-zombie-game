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
    /// Interaction logic for ProjectileInfoUI.xaml
    /// </summary>
    public partial class ProjectileInfoUI : UserControl
    {
        public SimpleProjectile p { get; protected set; }


        public ProjectileInfoUI()
        {
            InitializeComponent();
        }

        public void SetProjectile(SimpleProjectile p)
        {
            this.p = p;
            lblName.Content = p.Name;
            lblDamage.Content = "Dano: " + p.HitDamage;
            lblSpeed.Content = "Velocidade: " + p.SpeedMagnitude + " pixeis/s";
            lblStunner.Content = "Atordoador: " + ((p.IsStunner) ? "Sim (" + p.StunTimeMs / 1000 + "s)" : "Não");
            lblType.Content = "Tipo: " + p.Type.ToString();
            lblKnockback.Content = "Empurro: "+ p.KnockbackMagnitude + " pixeis";
            lblExplosive.Content = "Explosivo: " + ((p.IsExplosive) ? "Sim" : "Não");
            lblDistance.Content = "Distância máxima: " + p.TravelDistance + " pixeis";
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
            Application.Current.Windows.OfType<MainWindow>().
                FirstOrDefault().RemoveFromUI(this);
            Application.Current.Windows.OfType<MainWindow>().
                FirstOrDefault().AddToUI(UserControls.StoreControl);
        }
    }
}
