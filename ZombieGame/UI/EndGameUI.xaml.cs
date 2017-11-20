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
using ZombieGame.Game.Serializable;
using ZombieGame.IO.Serialization;

namespace ZombieGame.UI
{
    /// <summary>
    /// Interaction logic for EndGameUI.xaml
    /// </summary>
    public partial class EndGameUI : UserControl
    {
        public EndGameUI()
        {
            InitializeComponent();
            SaveButton.Content = "SALVAR MINHA PONTUAÇÃO";
        }

        private void SaveButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Database.Scores.Add(new Score()
            {
                Date = DateTime.Now,
                Highscore = GameMaster.Score,
                Name = Name.Text.ToUpper()
            });
            Database.Scores.SaveTo(IO.GlobalPaths.DB + "scores.db");
            SaveButton.Text.Content = "FEITO!";
        }

        private void SaveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SaveButton.Text.Content = "SALVAR MINHA PONTUAÇÃO";
        }
    }
}
