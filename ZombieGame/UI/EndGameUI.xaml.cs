using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZombieGame.Game;
using ZombieGame.Game.Serializable;
using ZombieGame.IO.Serialization;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for EndGameUI.xaml
    /// </summary>
    public partial class EndGameUI : UserControl
    {
        bool ScoreSaved = false;

        public EndGameUI()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ControlCache.PauseMenu.Refresh();
            Score.Content = string.Format("PONTUAÇÃO: {0}", Math.Round(GameMaster.Score));
        }

        private void SaveButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ScoreSaved)
                return;

            if (Name.Text.Length < 2)
            {
                SaveButton.Content = "NOME MUITO CURTO! (CLIQUE AQUI PARA TENTAR NOVAMENTE)";
                return;
            }

            ScoreSaved = true;
            Database.Scores.Add(new Score()
            {
                Date = DateTime.Now,
                Highscore = GameMaster.Score,
                Name = Name.Text.ToUpper()
            });
            Database.Scores.SaveTo(IO.GlobalPaths.DB + "scores.db");
            SaveButton.Content = "PONTUAÇÃO SALVA!";
        }

        private void BackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameMaster.TargetCanvas.Visibility = Visibility.Collapsed;
            ResourceManager.DestroyEverything();
            GameMaster.TargetWindow.MainMenu.Visibility = Visibility.Visible;
            GameMaster.Started = false;
            GameMaster.TargetCanvas.ResetUI();
        }

        private void SaveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //SaveButton.Content = "SALVAR MINHA PONTUAÇÃO";
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Name.Text = Name.Text.ToUpper();
            if (Name.CaretIndex == 0)
                Name.CaretIndex = Name.Text.Length;
        }
    }
}
