using System;
using NAudio.Wave;


internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}

namespace OnurKaroffDelay
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new AudioFileReader("test.wav"))
            {
                var delay = new DelayEffect(reader, 1000, 500, 0.5f, 0.8f);
                WaveFileWriter.CreateWaveFile16("test_delay.wav", delay);
            }

            Console.WriteLine("Done!");
        }
    }
}

