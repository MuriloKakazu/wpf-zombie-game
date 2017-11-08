using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace ZombieGame.Game
{
    [Serializable]
    public class Sprite
    {
        /// <summary>
        /// Caminho do arquivo
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Componente visual cuja imagem tem como fonte o caminho do arquivo
        /// </summary>
        [XmlIgnore]
        public BitmapSource Image { get { return new BitmapImage(new Uri(Uri)); } }

        /// <summary>
        /// ctor
        /// </summary>
        public Sprite()
        {
            Uri = IO.GlobalPaths.Sprites + "unavailable.png";
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uri">Caminho do arquivo</param>
        public Sprite(string uri)
        {
            Uri = uri;
        }
    }
}
