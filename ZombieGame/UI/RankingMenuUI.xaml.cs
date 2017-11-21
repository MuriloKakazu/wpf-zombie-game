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

            var sortedScores = Database.Scores.OrderBy(x => x.Highscore).Reverse();

            for (int i = 0; i < sortedScores.Count(); i++)
            {
                string pos = (i + 1).ToString();
                if (i + 1 > 200) pos = "200+";
                while (pos.Length < 4) pos += " ";
                string name = "NOME: " + sortedScores.ElementAt(i).Name;
                if (name.Length > 40) name = name.Substring(0, 37) + "...";
                else while (name.Length < 40) name += " ";
                string highscore = Math.Round(sortedScores.ElementAt(i).Highscore).ToString();
                if (highscore.Length > 12) highscore = "PTS: >999999999999";
                else { highscore = "PTS: " + highscore; while (highscore.Length < 18) highscore += " "; }
                string date = "DATA: " + sortedScores.ElementAt(i).Date.ToString(@"dd\/MM\/yyyy");
                GradientLabelUI label = new GradientLabelUI()
                {
                    Content = string.Format("{0} {1} {2} {3}", pos, name, highscore, date),
                    FontSize = 22,
                    FontFamily = new FontFamily("Lucida Console"),
                    Padding = new Thickness(20, 5, 20, 5),
                    Height = 50,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };
                //label.UnfocusedForeground = new SolidColorBrush(Colors.White);
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
