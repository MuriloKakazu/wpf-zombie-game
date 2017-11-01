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
using ZombieGame.Physics;

namespace ZombieGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var u = new Physics.Vector(2, 4);
            var v = new Physics.Vector(4, 14);

            var size = new Physics.Vector(10, 10);
            var bu = new Bounds(u, size);
            var bv = new Bounds(v, size);

            bool isInside = bu.IsInside(bv);
        }
    }
}
