using System;
using NAudio.Wave;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ZombieGame.Extensions;

namespace ZombieGame.Audio
{
    public class SoundTrack
    {
        public string FileName { get; set; }
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }

        public SoundTrack(string fileName)
        {
            FileName = fileName;

            using (var audioFileReader = new AudioFileReader(IO.GlobalPaths.Audio + FileName))
            {
                WaveFormat = audioFileReader.WaveFormat;
                var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }

        public static SoundTrack GetAnyWithKey(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return null;

            string[] files = Directory.GetFiles(IO.GlobalPaths.Audio);

            List<string> found = new List<string>();

            foreach (var file in files)
                if (file.Contains(key))
                    found.Add(Path.GetFileName(file));

            if (found.Count == 0)
                return null;

            return new SoundTrack(found.PickAny());
        }
    }
}
