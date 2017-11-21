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

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for GameStageUI.xaml
    /// </summary>
    public partial class GameStageUI : UserControl
    {
        public GameStageUI()
        {
            InitializeComponent();
        }

        public void Update()
        {
            Score.Content = "PONTUAÇÃO: " + Math.Round(GameMaster.Score);
            RunningTime.Content = TimeSpan.FromSeconds(GameMaster.RunningTime).ToString(@"hh\:mm\:ss");
        }
    }
}
