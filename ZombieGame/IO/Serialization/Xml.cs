using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZombieGame.IO.Serialization
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Serializa uma instância de um objeto qualquer e o salva em um arquivo .xml
        /// </summary>
        /// <typeparam name="T">Tipo de objeto</typeparam>
        /// <param name="obj">Objeto</param>
        /// <param name="path">Caminho do arquivo</param>
        /// <param name="append">True: adiciona ao arquivo sem substituir valores anteriores. False: adiciona ao arquivo substituindo valores anteriores</param>
        public static void SaveTo<T>(this T obj, string path, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(path, append);
                serializer.Serialize(writer, obj);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Desserializa um objeto a partir de um arquivo .xml
        /// </summary>
        /// <typeparam name="T">Tipo de objeto</typeparam>
        /// <param name="obj">Objeto</param>
        /// <param name="path">Caminho do arquivo</param>
        /// <returns></returns>
        public static T LoadFrom<T>(this T obj, string path) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(path);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
