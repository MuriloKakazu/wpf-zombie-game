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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZombieGame.Game;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for RankingMenuUI.xaml
    /// </summary>
    public partial class RankingMenuUI : UserControl
    {
        public RankingMenuUI()
        {
            InitializeComponent();
            BackButton.Text.Content = "BACK";

            var sortedScores = Database.Scores.OrderBy(x => x.Highscore).Reverse();

            for (int i = 0; i < sortedScores.Count(); i++)
            {
                var pos = i + 1;
                var name = sortedScores.ElementAt(i).Name;
                var highscore = sortedScores.ElementAt(i).Highscore;
                var date = sortedScores.ElementAt(i).Date.ToString(@"dd\/MM\/yyyy");
                GradientLabelUI label = new GradientLabelUI()
                {
                    Content = string.Format("POSITION: {0,-" + (20 - (pos.ToString().Length)) + "} NAME: {1,-" + 
                    (50 - (name.ToString().Length)) + "} SCORE: {2,-" + 
                    (20 - (highscore.ToString().Length)) + "} {3," + 
                    (40 - (date.ToString().Length)) + "}", pos, name, highscore, "DATE: " + date),
                    FontSize = 22,
                };
                Scores.Children.Add(label);
            }
        }

        private void BackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            GameMaster.TargetWindow.MainMenu.Grid.Children.Remove(this);
            GameMaster.TargetWindow.MainMenu.ReturnToOriginalState();
        }
    }
}
