using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZombieGame.IO;

namespace ZombieGame.Game
{
    public class Sprite
    {
        public string Uri { get; set; }

        public BitmapSource Image { get { return new BitmapImage(new Uri(Uri)); } }

        public Sprite(string uri)
        {
            Uri = uri;
        }
    }
}
