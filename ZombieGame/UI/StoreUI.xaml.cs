using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZombieGame.Game;
using ZombieGame.Game.Serializable;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for StoreUC.xaml
    /// </summary>
    public partial class StoreUC : UserControl
    {
        #region Properties
        /// <summary>
        /// Índice máximo possível
        /// </summary>
        public int MaxWIndex, MaxPIndex;
        /// <summary>
        /// Índice atual
        /// </summary>
        public int CurWIndex = 0, CurPIndex = 0;
        /// <summary>
        /// As 4 armas que estão na tela.
        /// </summary>
        public SimpleWeapon[] SellingWeapons = new SimpleWeapon[4];
        /// <summary>
        /// Os 4 projéteis que estão na tela.
        /// </summary>
        public SimpleProjectile[] SellingProjectiles = new SimpleProjectile[4];
        /// <summary>
        /// O booleano de se está aberto ou não
        /// </summary>
        public bool IsOpen = false;
        #endregion

        #region Methods
        /// <summary>
        /// Evento do botão de ir para a esquerda das armas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWeaponsLeft_Click(object sender, RoutedEventArgs e)
        {
            CurWIndex--;
            SetSellingWeapons();
            UpdateArrowButtons();
        }
        /// <summary>
        /// Evento do botão de ir para a direita das armas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWeaponsRight_Click(object sender, RoutedEventArgs e)
        {
            CurWIndex++;
            SetSellingWeapons();
            UpdateArrowButtons();
        }
        /// <summary>
        /// Evento do botão de ir para a direita dos projéteis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProjectilesRight_Click(object sender, RoutedEventArgs e)
        {
            CurPIndex++;
            SetSellingProjectiles();
            UpdateArrowButtons();
        }
        /// <summary>
        /// Evento do botão de ir para a esquerda dos projéteis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProjectilesLeft_Click(object sender, RoutedEventArgs e)
        {
            CurPIndex--;
            SetSellingProjectiles();
            UpdateArrowButtons();
        }
        /// <summary>
        /// Método para verificar se os botãos de ir para os lados estarão ligados ou não
        /// </summary>
        public void UpdateArrowButtons()
        {
            btnProjectilesLeft.IsEnabled = CurPIndex != 0;
            btnProjectilesRight.IsEnabled = CurPIndex != MaxPIndex;
            btnWeaponsLeft.IsEnabled = CurWIndex != 0;
            btnWeaponsRight.IsEnabled = CurWIndex != MaxWIndex;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public StoreUC()
        {
            InitializeComponent();
            Canvas.SetZIndex(this, 20);
        }
        /// <summary>
        /// Evento de quando a grid é carregada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            CurWIndex = 0;
            CurPIndex = 0;
            SetMaxIndex();
            Width = Physics.Vector.WindowSize.X;
            Height = Physics.Vector.WindowSize.Y;
            Update();
        }

        /// <summary>
        /// Método de definir o máximo índice baseado no número de armas vendidas.
        /// </summary>
        public void SetMaxIndex()
        {
            MaxWIndex = Store.SellingWeapons.Count - 4;
            MaxPIndex = Store.SellingProjectiles.Count - 4;
            if (MaxWIndex < 0)
                MaxWIndex = 0;
            if (MaxPIndex < 0)
                MaxPIndex = 0;
        }
        /// <summary>
        /// Método para definir os itens vendidos
        /// </summary>
        public void SetSellingItems()
        {
            SetSellingWeapons();
            SetSellingProjectiles();
        }
        /// <summary>
        /// Método para definir os projéteis vendidos.
        /// </summary>
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
        /// <summary>
        /// Método para definir as armas vendidas
        /// </summary>
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
        /// <summary>
        /// Método atualizar os si's das armas vendidas.
        /// </summary>
        private void UpdateWeaponInterfaces()
        {
            siWeapon1.SetSellingItem(SellingWeapons[0]);
            siWeapon2.SetSellingItem(SellingWeapons[1]);
            siWeapon3.SetSellingItem(SellingWeapons[2]);
            siWeapon4.SetSellingItem(SellingWeapons[3]);
        }
        /// <summary>
        /// Método atualizar os si's dos projéteis vendidos.
        /// </summary>
        private void UpdateProjectileInferfaces()
        {
            siProjectile1.SetSellingItem(SellingProjectiles[0]);
            siProjectile2.SetSellingItem(SellingProjectiles[1]);
            siProjectile3.SetSellingItem(SellingProjectiles[2]);
            siProjectile4.SetSellingItem(SellingProjectiles[3]);
        }
        /// <summary>
        /// Evento do botão voltar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ControlCache.PauseMenu.Grid.Children.Remove(ControlCache.StoreControl);
            ControlCache.PauseMenu.PausedMenuContent.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Método de atualizar a tela da loja.
        /// </summary>
        public void Update()
        {
            SetSellingItems();
            UpdateArrowButtons();
            lblDinheiro.Content = "Dinheiro: " + GameMaster.Money;
        }
        #endregion
    }
}
