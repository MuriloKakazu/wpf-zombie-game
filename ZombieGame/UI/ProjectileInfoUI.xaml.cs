using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZombieGame.Game.Serializable;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for ProjectileInfoUI.xaml
    /// </summary>
    public partial class ProjectileInfoUI : UserControl
    {
        /// <summary>
        /// O projétil exibido.
        /// </summary>
        public SimpleProjectile p { get; protected set; }

        /// <summary>
        /// ctor
        /// </summary>
        public ProjectileInfoUI()
        {
            InitializeComponent();
            Canvas.SetZIndex(this, 10);
        }

        /// <summary>
        /// O método para definir o projétil
        /// </summary>
        /// <param name="p">O projétil</param>
        public void SetProjectile(SimpleProjectile p)
        {
            this.p = p;
            lblName.Content = p.Name;
            lblDamage.Content = "Dano: " + p.HitDamage;
            lblSpeed.Content = "Velocidade: " + p.SpeedMagnitude + " pixeis/s";
            lblStunner.Content = "Atordoador: " + ((p.IsStunner) ? "Sim (" + p.StunTimeMs / 1000 + "s)" : "Não");
            lblType.Content = "Tipo: " + p.Type.ToString();
            lblKnockback.Content = "Empurro: " + p.KnockbackMagnitude + " pixeis";
            lblExplosive.Content = "Explosivo: " + ((p.IsExplosive) ? "Sim" : "Não");
            lblDistance.Content = "Distância máxima: " + p.TravelDistance + " pixeis";
            ItemIcon.Source = new BitmapImage(new Uri(IO.GlobalPaths.ProjectileSprites + p.SpriteFileName));
        }

        /// <summary>
        /// O botão voltar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GradientButtonUI_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ControlCache.PauseMenu.Grid.Children.Remove(this);
            ControlCache.PauseMenu.Grid.Children.Add(ControlCache.StoreControl);
        }
    }
}