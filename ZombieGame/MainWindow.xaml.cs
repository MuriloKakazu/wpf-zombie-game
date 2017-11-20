using System.Windows;
using System.Windows.Controls;
using ZombieGame.Game;
using ZombieGame.UI;

namespace ZombieGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainMenuUI MainMenu { get; protected set; }

        public MainWindow()
        {
            GameMaster.TargetWindow = this;
            GameMaster.LoadSettings();
            Database.Setup();
            InitializeComponent();
            GameCanvas.Visibility = Visibility.Hidden;
            GameMaster.TargetCanvas = GameCanvas;
            MainMenu = new MainMenuUI();
            Grid.Children.Add(MainMenu);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            ResourceManager.DestroyEverything();
        }
    }
}
