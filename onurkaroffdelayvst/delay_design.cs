using System;
using NAudio.Wave;

namespace OnurKaroffDelay
{
    public class DelayEffect : ISampleProvider
    {
        private ISampleProvider source;
        private float[] delayBuffer;
        private int delayBufferIndex;

        public float DelayTime { get; set; }
        public float Feedback { get; set; }
        public float WetDryMix { get; set; }

        public DelayEffect(ISampleProvider source, int maxDelayMilliseconds, float delayTime, float feedback, float wetDryMix)
        {
            this.source = source;
            delayBuffer = new float[maxDelayMilliseconds * source.WaveFormat.SampleRate / 1000];
            DelayTime = delayTime;
            Feedback = feedback;
            WetDryMix = wetDryMix;
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int sourceSamplesRead = source.Read(buffer, offset, count);

            for (int n = 0; n < sourceSamplesRead; n++)
            {
                var readIndex = (delayBufferIndex + delayBuffer.Length - (int)(DelayTime * source.WaveFormat.SampleRate / 1000)) % delayBuffer.Length;
                var delayedSample = delayBuffer[readIndex];
                var inputSample = buffer[offset + n];

                delayBuffer[delayBufferIndex] = inputSample + delayedSample * Feedback;
                buffer[offset + n] = inputSample * (1 - WetDryMix) + delayedSample * WetDryMix;

                delayBufferIndex = (delayBufferIndex + 1) % delayBuffer.Length;
            }

            return sourceSamplesRead;
        }
    }
}


