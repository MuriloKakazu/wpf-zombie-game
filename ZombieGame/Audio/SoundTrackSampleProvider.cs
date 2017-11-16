using NAudio.Wave;
using System;

namespace ZombieGame.Audio
{
    class SoundTrackSampleProvider : ISampleProvider
    {
            private readonly SoundTrack Track;
            private long position;

            public SoundTrackSampleProvider(SoundTrack track)
            {
                this.Track = track;
            }

            public int Read(float[] buffer, int offset, int count)
            {
                var availableSamples = Track.AudioData.Length - position;
                var samplesToCopy = Math.Min(availableSamples, count);
                Array.Copy(Track.AudioData, position, buffer, offset, samplesToCopy);
                position += samplesToCopy;
                return (int)samplesToCopy;
            }

            public WaveFormat WaveFormat { get { return Track.WaveFormat; } }
    }
}
