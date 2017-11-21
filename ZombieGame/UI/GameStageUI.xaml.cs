using System;
using System.Windows.Controls;
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
