using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ZombieGame.Game;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for PauseMenuUI.xaml
    /// </summary>
    public partial class PauseMenuUI : UserControl
    {
        #region Methods
        /// <summary>
        /// ctor
        /// </summary>
        public PauseMenuUI()
        {
            InitializeComponent();
            Canvas.SetZIndex(this, 11);
        }
        /// <summary>
        /// Evento de load do grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        /// <summary>
        /// Método de atualizar a tela, colocando-a no centro da celula
        /// </summary>
        public void Refresh()
        {
            if (GameMaster.Score > 0)
            {
                TranslateTransform tt = new TranslateTransform();
                if (GameMaster.GetPlayer(0).Character != null && GameMaster.GetPlayer(1).Character != null ||
                    GameMaster.GetPlayer(0).Character == null && GameMaster.GetPlayer(1).Character != null ||
                    GameMaster.GetPlayer(0).Character != null && GameMaster.GetPlayer(1).Character == null)
                {
                    tt.X = GameMaster.Camera.RigidBody.Position.X;
                    tt.Y = -GameMaster.Camera.RigidBody.Position.Y;
                }
                else
                {
                    tt.X = 0;
                    tt.Y = 0;
                    GameMaster.TargetCanvas.RenderTransform = tt;
                }
                RenderTransform = tt;
            }
            else
                RenderTransform = new TranslateTransform(0, 0);
        }
        /// <summary>
        /// Evento de clique do botão loja
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStore_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            Grid.Children.Add(ControlCache.StoreControl);
        }
        /// <summary>
        /// Evento de clique do botão configurações
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            var m = new SettingsMenuUI(fromMainMenu: false);
            m.DockPanel.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.Children.Add(m);
        }
        /// <summary>
        /// Evento de clique do botão menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            btnMainMenu.Content = "CLIQUE 2X PARA CONFIRMAR";
        }
        /// <summary>
        /// Evento de double clique do botão menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GameMaster.TargetCanvas.Visibility = Visibility.Collapsed;
            ResourceManager.DestroyEverything();
            GameMaster.TargetWindow.MainMenu.Visibility = Visibility.Visible;
            GameMaster.Started = false;
            GameMaster.TargetCanvas.ResetUI();
        }
        /// <summary>
        /// Evento de quando o mouse sai do botão menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            btnMainMenu.Content = "SAIR";
        }
        /// <summary>
        /// Evento do botão resume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResume_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PausedMenuContent.Visibility = Visibility.Collapsed;
            GameMaster.Resume();
            GameMaster.TargetCanvas.RemoveChild(ControlCache.PauseMenu);
            ControlCache.PauseMenu.Grid.Children.Clear();
            ControlCache.PauseMenu = new PauseMenuUI();
            GameMaster.HideCursor();
        }
        #endregion
    }
}
