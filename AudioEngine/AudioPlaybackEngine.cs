using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace AudioEngine
{
	public class AudioPlaybackEngine : IDisposable
	{
		private readonly IWavePlayer outputDevice;
		private readonly MixingSampleProvider mixer;

		public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);

		public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
		{ 
			outputDevice = new WaveOutEvent();
			mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount))
			{
				ReadFully = true
			};
			outputDevice.Init(mixer);
			outputDevice.Play();
		}

		public void PlaySound(string fileName)
		{
			var input = new AudioFileReader(fileName);
			AddMixerInput(new AutoDisposeFileReader(input));
		}

		public void PlaySound(CachedSound sound) 
		{
			AddMixerInput(new CachedSoundSampleProvider(sound));
		}

		private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
		{
			if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
			{
				return input;
			}
			else if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
			{
				return new MonoToStereoSampleProvider(input);
			}
			else
			{
				throw new Exception(String.Format("Поддержка такого количества каналов: {0} еще не реализована", input.WaveFormat.Channels));
			}
		}

		private void AddMixerInput(ISampleProvider input)
		{
			mixer.AddMixerInput(ConvertToRightChannelCount(input));
		}

		public void Dispose()
		{
			outputDevice.Dispose();
		}
	}
}
