using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Panel_input_voice : MonoBehaviour {

	private  int width = 500;
	private int height = 100;
	public Color wavebackgroundColor = Color.black; 
	private Color waveformColor = Color.red; 
	private int size = 2048;
	public RawImage rawImg_wave;

	public Image icon_voice;
	public Image icon_play_voice;
	public Sprite[] icon;
	public GameObject panel_voice_play;
	public GameObject panel_voice_record;
	public AudioSource myAudioRecord;
	public Text txt_time_record;
	public Text txt_time_play;

	private bool is_play=false;



	public void Start(){
		this.icon_voice.sprite = icon [0];
		this.icon_play_voice.sprite = icon [2];
		this.panel_voice_play.SetActive (false);
		this.panel_voice_record.SetActive (true);
		this.txt_time_record.gameObject.SetActive (false);
	}

	[Obsolete]
	public void voice_record(){
		if (icon_voice.sprite == icon [0]) {
			this.icon_voice.sprite = icon [1];
			myAudioRecord.clip = Microphone.Start ( null, false, 10, 44100 );
			this.txt_time_record.gameObject.SetActive (true);
			this.is_play = true;
			while(!(Microphone.GetPosition(Microphone.devices[0])>0)){};
			this.myAudioRecord.Play ();
			this.myAudioRecord.mute = true;
			this.Start_img_ware ();
			this.waveformColor = Color.red;
		} else {
			this.waveformColor =  Color.green;
			this.stop_record ();
			this.myAudioRecord.mute = false;
		}
	}

	[Obsolete]
	public void stop_record(){
		this.icon_voice.sprite = icon [0];
		this.panel_voice_play.SetActive (true);
		this.panel_voice_record.SetActive (false);
		this.txt_time_record.gameObject.SetActive (false);
		int lastTime = Microphone.GetPosition(null); if (lastTime == 0) return;
		Microphone.End(null);
		float[] samples = new float[myAudioRecord.clip.samples];
		myAudioRecord.clip.GetData(samples, 0);
		float[] ClipSamples = new float[lastTime];
		Array.Copy(samples, ClipSamples, ClipSamples.Length - 1);
		myAudioRecord.clip = AudioClip.Create("playRecordClip", ClipSamples.Length, 1, 44100, false, false);
		myAudioRecord.clip.SetData(ClipSamples, 0);
		this.is_play = false;
	}

	public void delete_voice(){
		this.panel_voice_play.SetActive (false);
		this.panel_voice_record.SetActive (true);
		this.is_play = false;
		this.icon_play_voice.sprite = icon [2];
		this.myAudioRecord.clip = null;
	}

	public void play_voice(){
		if (icon_play_voice.sprite == icon [2]) {
			myAudioRecord.Play ();
			this.icon_play_voice.sprite = icon [3];
			this.is_play = true;
			this.waveformColor = Color.green;
			this.myAudioRecord.mute = false;
		} else {
			myAudioRecord.Stop();
			this.is_play = false;
			this.icon_play_voice.sprite = icon [2];
			this.myAudioRecord.mute = true;
		}
	}

	void Update () {
		if (this.is_play) {
			this.txt_time_record.text =Microphone.devices[0].ToString();
			this.txt_time_play.text = string.Format("{0}:{1:00}", (int)this.myAudioRecord.time / 60, (int)this.myAudioRecord.time % 60);
			// clear the texture 
			texture.SetPixels (blank, 0); 

			// get samples from channel 0 (left) 
			this.myAudioRecord.GetOutputData (samples, 0); 

			// draw the waveform 
			for (int i = 0; i < size; i++) { 
				if (this.myAudioRecord.mute) {
					texture.SetPixel ((int)(width * i / size), (int)(height * (UnityEngine.Random.Range(0.0f,10f) + 1f) / 2f), waveformColor);
				} else {
					texture.SetPixel ((int)(width * i / size), (int)(height * (samples [i] + 1f) / 2f), waveformColor);
				}
			} // upload to the graphics card 

			texture.Apply (); 
		}

	}

	private Color[] blank; // blank image array 
	private Texture2D texture; 
	private float[] samples; // audio samples array

	public void Start_img_ware ()
	{ 

		// create the samples array 
		samples = new float[size]; 

		// create the texture and assign to the guiTexture: 
		texture = new Texture2D (width, height);

		this.rawImg_wave.texture = texture; 

		// create a 'blank screen' image 
		blank = new Color[width * height]; 

		for (int i = 0; i < blank.Length; i++) { 
			blank [i] = this.wavebackgroundColor; 
		} 

	}
}
