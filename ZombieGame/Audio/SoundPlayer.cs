using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

namespace ZombieGame.Audio
{
    public class SoundPlayer : IDisposable
    {
        #region Properties
        private static WaveOutEvent Device { get; set; }
        private static MixingSampleProvider Mixer { get; set; }
        public static readonly SoundPlayer Instance = new SoundPlayer(44100, 2);
        public static PlaybackState State { get { return Device.PlaybackState; } }
        public static float Volume { get { return Device.Volume; } set { Device.Volume = value; } }
        public static bool DestroyOnPlaybackEnd { get; set; }
        #endregion

        public SoundPlayer(int sampleRate = 44100, int channelCount = 2)
        {
            Device = new WaveOutEvent();
            Mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            Mixer.ReadFully = true;
            Device.Init(Mixer);
            Device.Play();
        }

        private void AddMixerInput(ISampleProvider input)
        {
            Mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == Mixer.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && Mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }
            return null;
        }

        public void Play(string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                return;

            var input = new AudioFileReader(fileName);
            AddMixerInput(new AutoDisposableStream(input));
        }

        public void Play(SoundTrack track)
        {
            if (track == null)
                return;

            AddMixerInput(new SoundTrackSampleProvider(track));
        }

        public void Dispose()
        {
            Device.Dispose();
        }
    }
}
