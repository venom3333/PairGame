using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace AudioEngine
{
	public class CachedSound
	{
		public float[] AudioData { get; private set; }
		public WaveFormat WaveFormat { get; private set; }

		protected readonly int rightSampleRate = 44100;

		public CachedSound(string audioFileName)
		{
			
			using (var audioFileReader = new AudioFileReader(audioFileName))
			{
				List<float> wholeFile = new List<float>((int)(audioFileReader.Length / 4));
				int samplesRead;
				float[] readBuffer = new float[rightSampleRate * 8];
				//float[] readBuffer = new float[WaveFormat.SampleRate * WaveFormat.Channels * 4];


				if (audioFileReader.WaveFormat.SampleRate != rightSampleRate)
				{
					// Ресемплер
					var resampler = new WdlResamplingSampleProvider(audioFileReader, rightSampleRate);
					WaveFormat = resampler.WaveFormat;

					while ((samplesRead = resampler.Read(readBuffer, 0, readBuffer.Length)) > 0)
					{
						wholeFile.AddRange(readBuffer.Take(samplesRead));
					}
				}
				else {
					WaveFormat = audioFileReader.WaveFormat;

					while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
						wholeFile.AddRange(readBuffer.Take(samplesRead));
					}
				}
				AudioData = wholeFile.ToArray();
			}
		}
	}
}