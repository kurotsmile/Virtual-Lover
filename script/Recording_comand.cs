using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KKSpeech;
using UnityEngine.Android;

public class Recording_comand : MonoBehaviour {
	public Text txt_status;
	public Text txt_status_2;

	public Text txt_status_maximize;
	public Text txt_status_2_maximize;

	public Image img_btn_voice;
	public Image img_btn_voice_maximize;

	public Image img_btn_auto;
	public GameObject panel_voice;
	public GameObject panel_voice_maximize;
	public Color32 color_start;

	public bool auto_chat=false;

	private bool is_maximize=false;

	private InputField obj_inp = null;

	void Start () {
		if (SpeechRecognizer.ExistsOnDevice()) {
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
			listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
			SpeechRecognizer.SetDetectionLanguage (PlayerPrefs.GetString("key_voice","en-US"));
			SpeechRecognizer.RequestAccess();
			SpeechRecognizer.SetDetectionLanguage (PlayerPrefs.GetString("key_voice","en-US"));
		} else {
			txt_status.text =PlayerPrefs.GetString ("voice_not_suport", "Sorry, but this device doesn't support speech recognition");
			txt_status_maximize.text=PlayerPrefs.GetString ("voice_not_suport", "Sorry, but this device doesn't support speech recognition");
		}

		this.panel_voice.SetActive (false);
		this.panel_voice_maximize.SetActive (false);
		txt_status_2_maximize.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
		txt_status_2.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
	}
	
	public void OnFinalResult(string result) {
		if (this.is_maximize) {
			this.img_btn_voice_maximize.color = Color.white;
			txt_status_maximize.text = result;
			txt_status_maximize.color = this.color_start;
			this.img_btn_voice_maximize.color = Color.white;
			this.obj_inp.text = result;
			txt_status_maximize.color = Color.yellow;
		} else {
			this.img_btn_voice.color = Color.white;
			txt_status.text = result;
			txt_status.color = this.color_start;
			this.img_btn_voice.color = Color.white;
			this.GetComponent<mygirl> ().inpText.text = result;
			this.GetComponent<mygirl> ().play_s ();
			txt_status.color = Color.yellow;
		}

        if (this.GetComponent<mygirl>().panel_report.activeInHierarchy == false && this.GetComponent<mygirl>().panel_learn.activeInHierarchy == false)
        {
            this.GetComponent<mygirl>().play_s();
            this.stop_recoding();
        }

        this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
	}

	public void OnPartialResult(string result) {
		if (this.is_maximize) {
			txt_status_maximize.text = result;
			txt_status_maximize.color = Color.green;
		} else {
			this.GetComponent<mygirl> ().txt_show_inp_chat.text = result + "_";
			this.GetComponent<mygirl> ().txt_show_inp_chat.transform.parent.gameObject.SetActive (true);
			txt_status.text = result;
			txt_status.color = Color.green;
		}
	}

	public void OnAvailabilityChange(bool available) {
		if (!available) {
			if (this.is_maximize) {
				txt_status_maximize.text = PlayerPrefs.GetString ("voice_not_available", "Speech Recognition not available");
			} else {
				txt_status.text = PlayerPrefs.GetString ("voice_not_available", "Speech Recognition not available");
			}
		} else {
			if (this.is_maximize) {
				txt_status_maximize.text = PlayerPrefs.GetString ("voice_statu", "Say something :-)");
			} else {
				txt_status.text = PlayerPrefs.GetString ("voice_statu", "Say something :-)");
			}
		}

		if (this.is_maximize) {
			this.img_btn_voice_maximize.color = Color.white;
		} else {
			this.img_btn_voice.color = Color.white;
		}

		this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
	}

	public void OnAuthorizationStatusFetched(AuthorizationStatus status) {
		switch (status) {
		case AuthorizationStatus.Authorized:
			break;
		default:
			if (this.is_maximize) {
				txt_status.text = PlayerPrefs.GetString ("voice_not_rote", "Cannot use Speech Recognition, authorization status is ") + status;
			} else {
				txt_status_maximize.text = PlayerPrefs.GetString ("voice_not_rote", "Cannot use Speech Recognition, authorization status is ") + status;
			}
			break;
		}
		if (this.is_maximize) {
			this.img_btn_voice_maximize.color = Color.white;
		} else {
			this.img_btn_voice.color = Color.white;
		}
		this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
	}

	public void OnEndOfSpeech() {
		this.img_btn_voice.color = Color.white;
		if (this.is_maximize) {
			txt_status_2_maximize.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
		} else {
			txt_status_2.text = PlayerPrefs.GetString ("voice_start", "Start Recording");

		}
		Microphone.End(null);
		this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
	}

	public void OnError(string error) {
		Debug.LogError(error);
		if (this.is_maximize) {
			txt_status_maximize.text = PlayerPrefs.GetString ("voice_error", "Something went wrong... Try again!");
			txt_status_2_maximize.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
			this.img_btn_voice_maximize.color = Color.white;
		} else {
			txt_status.text = PlayerPrefs.GetString ("voice_error", "Something went wrong... Try again!");
			txt_status_2.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
			this.img_btn_voice.color = Color.white;

		}
		this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
	}


	public void start_recoding(){
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        if (SpeechRecognizer.IsRecording()) {
			SpeechRecognizer.StopIfRecording();
			if (this.is_maximize) {
				txt_status_2_maximize.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
				this.img_btn_voice.color = Color.white;
			} else {
				txt_status_2.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
				this.img_btn_voice.color = Color.white;
			}

			this.GetComponent<mygirl> ().speech.mute = false;
			this.GetComponent<mygirl> ().speech.volume = 1f;
		} else {
			this.play_recoding ();
		}
		if (this.is_maximize) {
			this.panel_voice_maximize.SetActive (true);
		} else {
			this.panel_voice.SetActive (true);
		}
	}

	public void start_recoding_minimax ()
	{
		
		this.is_maximize = false;
		this.start_recoding ();
	}

	public void start_recoding_maximize(InputField inp_voice_command){
		this.obj_inp = inp_voice_command;
		this.is_maximize = true;
		this.start_recoding ();
        if (this.GetComponent<mygirl>().panel_report.activeInHierarchy == false && this.GetComponent<mygirl>().panel_learn.activeInHierarchy == false)
        {
            this.GetComponent<mygirl>().Magic_tocuh.SetActive(false);
        }
    }


	public void stop_recoding(){
		SpeechRecognizer.StopIfRecording();
		if (this.is_maximize) {
			this.img_btn_voice_maximize.color = Color.white;
			this.panel_voice_maximize.SetActive (false);
		} else {
			this.img_btn_voice.color = Color.white;
			this.panel_voice.SetActive (false);
		}
		this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
        if (this.GetComponent<mygirl>().panel_report.activeInHierarchy == false && this.GetComponent<mygirl>().panel_learn.activeInHierarchy == false)
        {
            this.GetComponent<mygirl>().Magic_tocuh.SetActive(true);
        }
    }

	public void play_recoding(){
		SpeechRecognizer.StartRecording(true);
		if (this.is_maximize) {
			this.img_btn_voice_maximize.color = Color.red;
			txt_status_maximize.color = this.color_start;
			txt_status_maximize.text = PlayerPrefs.GetString ("input_tip", "Say something :-)");
		} else {
			this.img_btn_voice.color = Color.red;
			txt_status.color = this.color_start;
			txt_status.text = PlayerPrefs.GetString ("voice_statu", "Say something :-)");
		}
		this.GetComponent<mygirl> ().speech.mute = true;                                                                                                                                                   
	}
		

	public void btn_auto_chat(){
		if (this.auto_chat) {
			this.img_btn_auto.color = this.color_start;
			this.txt_status.text = PlayerPrefs.GetString ("voice_auto_off", "OFF Auto chat");
			this.txt_status_2.text = PlayerPrefs.GetString ("voice_auto_off_tip", "OFF Auto chat");
			this.auto_chat = false;
		} else {
			this.auto_chat = true;
			this.txt_status.text = PlayerPrefs.GetString ("voice_auto_on", "On Auto chat");
			this.txt_status_2.text = PlayerPrefs.GetString ("voice_auto_on_tip", "On Auto chat");
			this.img_btn_auto.color = Color.red;
		}
	}
}
