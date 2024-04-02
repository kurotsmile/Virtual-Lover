using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class Recording_comand : MonoBehaviour {
	public Text txt_status;
	public Text txt_status_2;

	public Text txt_status_maximize;

	public Image img_btn_voice;
	public Image img_btn_voice_maximize;

	public Image img_btn_auto;
	public GameObject panel_voice;
	public Color32 color_start;

	public bool auto_chat=false;

	private bool is_maximize=false;

	private InputField obj_inp = null;

	void Start () {
		this.panel_voice.SetActive (false);
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

	public void OnEndOfSpeech() {
		this.img_btn_voice.color = Color.white;
		Microphone.End(null);
		this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
	}

	public void OnError(string error) {
		Debug.LogError(error);
		if (this.is_maximize) {
			txt_status_maximize.text = PlayerPrefs.GetString ("voice_error", "Something went wrong... Try again!");
			this.img_btn_voice_maximize.color = Color.white;
		} else {
			txt_status.text = PlayerPrefs.GetString ("voice_error", "Something went wrong... Try again!");
			txt_status_2.text = PlayerPrefs.GetString ("voice_start", "Start Recording");
			this.img_btn_voice.color = Color.white;

		}
		this.GetComponent<mygirl> ().speech.mute = false;
		this.GetComponent<mygirl> ().speech.volume = 1f;
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
