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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZombieGame.Game;
using ZombieGame.Game.Prefabs.Sprites;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for MainMenuUI.xaml
    /// </summary>
    public partial class MainMenuUI : UserControl
    {
        protected Image ParalaxLayer0 { get; set; }
        protected Image ParalaxLayer1 { get; set; }
        protected Image ParalaxLayer2 { get; set; }
        protected Image ParalaxLayer3 { get; set; }
        protected Image ParalaxLayer4 { get; set; }

        public MainMenuUI()
        {
            InitializeComponent();
            Grid.Background = new ImageBrush(new BitmapImage(new Uri(IO.GlobalPaths.ParallaxSprites + "splash_background.png")));
            ParalaxLayer0 = new Image() { Source = new BitmapImage(new Uri(IO.GlobalPaths.ParallaxSprites + "splash_layer0.png")) };
            ParalaxLayer1 = new Image() { Source = new BitmapImage(new Uri(IO.GlobalPaths.ParallaxSprites + "splash_layer1.png")) };
            ParalaxLayer2 = new Image() { Source = new BitmapImage(new Uri(IO.GlobalPaths.ParallaxSprites + "splash_layer2.png")) };
            ParalaxLayer3 = new Image() { Source = new BitmapImage(new Uri(IO.GlobalPaths.ParallaxSprites + "splash_layer3.png")) };
            ParalaxLayer4 = new Image() { Source = new BitmapImage(new Uri(IO.GlobalPaths.ParallaxSprites + "splash_layer4.png")) };
            Grid.Children.Add(ParalaxLayer0);
            Grid.Children.Add(ParalaxLayer1);
            Grid.Children.Add(ParalaxLayer2);
            Grid.Children.Add(ParalaxLayer3);
            Grid.Children.Add(ParalaxLayer4);
        }

        public void ReturnToOriginalState()
        {
            MainMenuOptions.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point cursorPos = new Point(e.GetPosition(relativeTo: this).X, e.GetPosition(relativeTo: this).Y);
            TranslateTransform tt0 = new TranslateTransform(-cursorPos.X / 50, -cursorPos.Y / 50);
            TranslateTransform tt1 = new TranslateTransform(-cursorPos.X / 40, -cursorPos.Y / 40);
            TranslateTransform tt2 = new TranslateTransform(cursorPos.X / 50, cursorPos.Y / 50);
            TranslateTransform tt3 = new TranslateTransform(cursorPos.X / 20, cursorPos.Y / 20);
            TranslateTransform tt4 = new TranslateTransform(-cursorPos.X / 15, -cursorPos.Y / 15);
            ParalaxLayer0.RenderTransform = tt0;
            ParalaxLayer1.RenderTransform = tt1;
            ParalaxLayer2.RenderTransform = tt2;
            ParalaxLayer3.RenderTransform = tt3;
            ParalaxLayer4.RenderTransform = tt4;
        }

        private void StartGameButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (GameMaster.Started)
                return;

            this.Visibility = Visibility.Collapsed;
            GameMaster.Setup();
            GameMaster.TargetCanvas.Prepare();
            GameMaster.TargetCanvas.Visibility = Visibility.Visible;
            GameMaster.TargetCanvas.P1Stats.UpdateStats();
            GameMaster.TargetCanvas.P2Stats.UpdateStats();
            GameMaster.TargetCanvas.GameInfo.Update();
        }

        private void SettingsButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainMenuOptions.Visibility = Visibility.Collapsed;
            var m = new SettingsMenuUI();
            m.AccessedFromMainMenu = true;
            GameMaster.TargetWindow.Grid.Children.Add(m);
        }

        private void QuitButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            QuitButton.Content = "CLIQUE 2X PARA CONFIRMAR";
        }

        private void RankingButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainMenuOptions.Visibility = Visibility.Collapsed;
            GameMaster.TargetWindow.Grid.Children.Add(new RankingMenuUI());
        }

        private void QuitButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void QuitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            QuitButton.Content = "SAIR";
        }

        private void AboutButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainMenuOptions.Visibility = Visibility.Collapsed;
            GameMaster.TargetWindow.Grid.Children.Add(new AboutUI());
        }
    }
}
