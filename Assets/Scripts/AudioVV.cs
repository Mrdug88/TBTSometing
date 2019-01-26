using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioVV : MonoBehaviour {
	AudioSource audioSource;
	private float[] samples = new float[1024];
	private float[] samplesright = new float[1024];
	private float[] frequencyBands = new float[8];
	private float[] bandBuffer = new float[8];
	private float[] bufferDecrease = new float[8];
	private float[] freqBandHighest = new float[8];

	//64 bands
	private float[] frequencyBands64 = new float[64];
	private float[] bandBuffer64 = new float[64];
	private float[] bufferDecrease64 = new float[64];
	private float[] freqBandHighest64 = new float[64];

	[HideInInspector]
	public float[] audioBand, audioBandBuffer, audioBand64, audioBandBuffer64;
	public static float amplitude, amplitudeBuffer;
	private float amplitudeHightest;
	public float audioProfile;

	public enum channel { Stereo, Left, Right };
 public channel chan = new channel ();

 // Use this for initialization
 void Start () {
 audioBand = new float[8];
 audioBandBuffer = new float[8];

 audioBand64 = new float[64];
 audioBandBuffer64 = new float[64];

 audioSource = GetComponent<AudioSource> ();
 AudioProfile (audioProfile);
	}

	// Update is called once per frame
	void Update () {
		GetSpectrumAudio ();
		MakeFrequencyBand ();
		BandBuffer ();
		CreateAudioBands ();
		GetAmplitude ();
		MakeFrequencyBand64 ();
		BandBuffer64 ();
		CreateAudioBands64 ();
	}

	void AudioProfile (float audioprofile) {
		for (int i = 0; i < 8; i++) {
			freqBandHighest[i] = audioProfile;
		}
	}

	void GetAmplitude () {
		float currentAmplitude = 0;
		float currentAmpliteudeBuffer = 0;

		for (int x = 0; x < 8; x++) {
			currentAmplitude += audioBand[x];
			currentAmpliteudeBuffer += audioBandBuffer[x];
		}
		if (currentAmplitude > amplitudeHightest) {
			amplitudeHightest = currentAmplitude;
		}
		amplitude = currentAmplitude / amplitudeHightest;
		amplitudeBuffer = currentAmpliteudeBuffer / amplitudeHightest;
	}

	void GetSpectrumAudio () {
		audioSource.GetSpectrumData (samples, 0, FFTWindow.BlackmanHarris);
		audioSource.GetSpectrumData (samplesright, 1, FFTWindow.BlackmanHarris);
	}

	void CreateAudioBands () {
		for (int i = 0; i < 8; i++) {
			if (frequencyBands[i] > freqBandHighest[i]) {
				freqBandHighest[i] = frequencyBands[i];
			}

			audioBand[i] = (frequencyBands[i] / freqBandHighest[i]);
			audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
		}
	}

	void CreateAudioBands64 () {
		for (int i = 0; i < 64; i++) {
			if (frequencyBands64[i] > freqBandHighest64[i]) {
				freqBandHighest64[i] = frequencyBands64[i];
			}

			audioBand64[i] = (frequencyBands64[i] / freqBandHighest64[i]);
			audioBandBuffer64[i] = (bandBuffer64[i] / freqBandHighest64[i]);
		}
	}

	void BandBuffer () {
		for (int g = 0; g < 8; g++) {
			if (frequencyBands[g] > bandBuffer[g]) {
				bandBuffer[g] = frequencyBands[g];
				bufferDecrease[g] = 0.005f;
			}
			if (frequencyBands[g] < bandBuffer[g]) {
				bandBuffer[g] -= bufferDecrease[g];
				bufferDecrease[g] *= 1.2f;
			}
		}

	}

	void BandBuffer64 () {
		for (int g = 0; g < 64; g++) {
			if (frequencyBands64[g] > bandBuffer64[g]) {
				bandBuffer64[g] = frequencyBands64[g];
				bufferDecrease64[g] = 0.005f;
			}
			if (frequencyBands64[g] < bandBuffer64[g]) {
				bandBuffer64[g] -= bufferDecrease64[g];
				bufferDecrease64[g] *= 1.2f;
			}
		}

	}

	void MakeFrequencyBand () {
		int count = 0;
		for (int i = 0; i < 8; i++) {
			float average = 0f;
			int sampleCount = (int) Mathf.Pow (2, i) * 2;
			if (i == 7) {
				sampleCount += 2;
			}
			for (int j = 0; j < sampleCount; j++) {
				average += samples[count] * (count + 1);
				count++;
			}
			average /= count;
			frequencyBands[i] = average * 10;

		}
	}

	void MakeFrequencyBand64 () {
		int count = 0;
		int sampleCount = 1;
		int power = 0;

		for (int i = 0; i < 64; i++) {

			float average = 0f;
			//sampleCount = (int) Mathf.Pow (2, power) * 2;

			if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56) {
				power++;
				sampleCount = (int) Mathf.Pow (2, power);
				if (power == 3) {
					sampleCount -= 2;
				}
			}

			for (int j = 0; j < sampleCount; j++) {
				if (chan == channel.Stereo) {
					average += (samples[count] + samplesright[count] * (count + 1));
				}
				if (chan == channel.Left) {
					average += samples[count] + (count + 1);
				}
				if (chan == channel.Right) {
					average += samplesright[count] * (count + 1);
				}
				count++;
			}
			average /= count;
			frequencyBands64[i] = average * 80;

		}
	}
}