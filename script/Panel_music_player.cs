using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.Radio;
using UnityEngine.Networking;

public class Panel_music_player : MonoBehaviour {

	public AudioSource soud_player_music;
	public RawImage img_ware;
	public Color wavebackgroundColor = Color.black; 
	private Color waveformColor = Color.green;
	public Text txt_name_player;
	public Slider slide_download;
	private int size = 2048;
	public Image img_download;
	public Image img_time_player;
	private  int width = 500;
	private int height = 100;
	private Color[] blank;
	private Texture2D texture; 
	private float[] samples;

	public Image Img_btn_playe;
	public Sprite play;
	public Sprite stop;
	public Sprite[] ramdom;
	public Image img_fnc_pause_play;
	public Image img_fnc_random_play;
	public Text txt_fnc_time_play;

	public GameObject panel_player_download;
	private bool is_click_control=false;
	private int sel_random = 0;

	private bool is_sound_3d=false;
	private bool is_view_bk = false;

	public Image pic_sound_3d;
	public Slider slider_music;
	public GameObject point_effect_sound3d;

	public GameObject panel_radio_info;
	public Image panel_radio_info_image;

	public bool is_offline;
	public bool is_radio;

	public Text txt_info_radio;

	private string str_name_radio="";

	public GameObject panel_chat_act;
	public GameObject btn_lyric_1;
	public GameObject btn_lyric_2;

	public GameObject btn_video;
	public GameObject btn_download;
	public GameObject btn_mp3;
	public GameObject btn_share;

	[Header("Audio mix")]
	public Image img_btn_audio_setting;
	public GameObject panel_audio_setting;
	public Dropdown dropdown_ReverbFilters;
	private System.Collections.Generic.List<AudioReverbPreset> reverbPresets = new System.Collections.Generic.List<AudioReverbPreset>();

	[Header ("Data save offline")]
	private byte[] data_music;
	private string url_id;
	private string data_lyric="";
	private string data_link_video = "";
	private string data_s_artist = "";
	private string data_s_genre = "";
	private string data_s_album = "";
	private string data_s_year = "";
	private string lang_music= "";
	public string id_chat_music="";

	void Start()
	{
		this.soud_player_music.GetComponent<RadioPlayer>().OnErrorInfo += this.Radio_act_error;
		System.Collections.Generic.List<Dropdown.OptionData> options = new System.Collections.Generic.List<Dropdown.OptionData>();

		foreach (AudioReverbPreset arp in System.Enum.GetValues(typeof(AudioReverbPreset)))
		{
			options.Add(new Dropdown.OptionData(arp.ToString()));
			reverbPresets.Add(arp);
		}

		if (dropdown_ReverbFilters != null)
		{
			dropdown_ReverbFilters.ClearOptions();
			dropdown_ReverbFilters.AddOptions(options);
		}
	}

	IEnumerator downloadAudios(string url_mp3){
		using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url_mp3, AudioType.MPEG))
		{
			www.SendWebRequest();
			while (!www.isDone)
			{
				this.slide_download.value = www.downloadProgress;
				yield return null;
			}

			if (www.result!=UnityWebRequest.Result.Success)
			{
				this.panel_player_download.SetActive(false);
			}
			else
			{
				this.Start_img_ware();
				GameObject.Find("mygirl").GetComponent<mygirl>().set_fnc_music(true);
				this.data_music = www.downloadHandler.data;
				soud_player_music.clip = DownloadHandlerAudioClip.GetContent(www);
				this.slider_music.maxValue = soud_player_music.clip.length;
				soud_player_music.Play();
				soud_player_music.ignoreListenerPause = true;
				this.txt_name_player.GetComponent<Animation>().enabled = true;
				GameObject.Find("mygirl").GetComponent<mygirl>().play_music_and_dance(true);
				this.Img_btn_playe.sprite = this.stop;
				this.img_fnc_pause_play.sprite = this.stop;
				this.panel_player_download.SetActive(false);
				this.url_id = url_mp3;
			}
		}
	}

	void Update () {
		if (this.soud_player_music.isPlaying) {
			this.update_ware_music ();
		}

		if (this.is_radio) {
			if (this.soud_player_music.isPlaying == false) {
				this.txt_name_player.text = string.Format (PlayerPrefs.GetString("wait_radio","wait_radio")+" ({0:N0}) KB", this.soud_player_music.GetComponent<RadioPlayer> ().CurrentBufferSize / 1024);
			}else{
				this.txt_fnc_time_play.text = string.Format ("{0}:{1:00}", (int)this.soud_player_music.GetComponent<RadioPlayer>().PlayTime / 60, (int)this.soud_player_music.GetComponent<RadioPlayer> ().PlayTime % 60);
				this.txt_info_radio.text = string.Format ("{0:N0} KB", this.soud_player_music.GetComponent<RadioPlayer>().CurrentDownloadSpeed / 1024);
				this.txt_name_player.text = str_name_radio;
			}
		} else {
			if (this.soud_player_music.clip != null) {
				if (this.soud_player_music.isPlaying == false && this.is_click_control == false) {
					if (this.sel_random == 0) {
						this.btn_delete_music ();
					}

					if (this.sel_random == 1) {
						this.soud_player_music.Play ();
					}

					if (this.sel_random == 2) {
						if (this.is_offline == false) {
							GameObject.Find ("mygirl").GetComponent<Music_offline> ().play_random_music ();
						} else {
							this.btn_next_music ();
						}
					}

				} else {
					this.txt_fnc_time_play.text = string.Format ("{0}:{1:00}", (int)this.soud_player_music.time / 60, (int)this.soud_player_music.time % 60);
					this.slider_music.value = this.soud_player_music.time;
				}

			}
		}
	}

	public void update_ware_music(){
		texture.SetPixels (blank, 0); 
		this.soud_player_music.GetOutputData (samples, 0); 
		for (int i = 0; i < size; i++) { 
			texture.SetPixel ((int)(width * i / size), (int)(height * (samples [i] + 1f) / 2f), waveformColor);
		} 
		texture.Apply (); 
	}

	public void Start_img_ware ()
	{ 
		samples = new float[size]; 
		this.img_ware.color = Color.white;
		texture = new Texture2D (width, height);
		this.img_ware.texture = texture; 
		blank = new Color[width * height]; 
		for (int i = 0; i < blank.Length; i++) { 
			blank [i] = this.wavebackgroundColor; 
		} 
	}

	public void set_download_wating(string title_name,string url_player,Color color_ware,bool is_offline,string lyric_txt,string link_video,string s_artist,string s_album,string s_genre,string s_year,string url_avatar,string s_lang)
	{
		this.gameObject.SetActive (true);
		this.StopAllCoroutines ();
		this.txt_name_player.text = title_name;
		this.txt_name_player.color = Color.white;
		this.txt_name_player.GetComponent<Animation> ().enabled = false;
		this.panel_player_download.SetActive (true);
		this.img_ware.color = Color.black;
		this.Img_btn_playe.sprite = this.play;
		this.waveformColor = color_ware;
		this.img_download.color = color_ware;
		this.img_time_player.color = color_ware;
		this.is_view_bk = false;
		this.soud_player_music.clip = null;
		this.slider_music.value = 0f;
		StartCoroutine (downloadAudios (url_player));
		GameObject.Find ("mygirl").GetComponent<mygirl> ().set_bk_music (false);
		GameObject.Find ("mygirl").GetComponent<mygirl> ().set_fnc_music (false);
		this.btn_download.SetActive (is_offline);
		this.panel_radio_info.SetActive (false);
		this.is_offline = is_offline;
		this.slider_music.gameObject.SetActive (true);
		this.is_radio = false;
		GameObject.Find("mygirl").GetComponent<mygirl>().chat_box.show_info_music(title_name, s_artist,s_album,s_genre,s_year,url_avatar);
		if (lyric_txt == ""&&link_video=="") {
			this.btn_lyric_1.SetActive (false);
			this.btn_lyric_2.SetActive (false);
			this.btn_video.SetActive (false);
			this.panel_chat_act.SetActive (false);
		} else {
			this.panel_chat_act.SetActive (true);
			if (lyric_txt != "") {
				this.btn_lyric_1.SetActive (true);
				this.btn_lyric_2.SetActive (true);
            }
            else
            {
                this.btn_lyric_1.SetActive(false);
                this.btn_lyric_2.SetActive(false);
            }

			if (link_video != "") {
				this.btn_video.SetActive (true);
            }
            else
            {
                this.btn_video.SetActive(false);
            }
		}
		this.audio_setting_close ();
		this.data_lyric = lyric_txt;
		this.data_link_video = link_video;
		this.data_s_album = s_album;
		this.data_s_genre = s_genre;
		this.data_s_year = s_year;
		this.data_s_artist = s_artist;
		this.lang_music = s_lang;

        if (s_lang != "")
        {
			this.btn_share.SetActive(true);
			this.btn_mp3.SetActive(true);
		}
        else
        {
			this.btn_share.SetActive(false);
			this.btn_mp3.SetActive(false);
		}
	}

	public void set_radio(string title_name,string url_player,Sprite icon_radio){
		this.gameObject.SetActive (true);
		this.StopAllCoroutines ();
		this.img_ware.color = Color.black;
		this.waveformColor = Color.green;
		this.img_download.color =  Color.green;
		this.img_time_player.color =  Color.green;
		this.txt_name_player.text = title_name;
		this.panel_radio_info.SetActive (true);
		this.panel_player_download.SetActive (false);
		this.soud_player_music.clip = null;
		GameObject.Find ("mygirl").GetComponent<mygirl> ().set_bk_music (false);
		GameObject.Find ("mygirl").GetComponent<mygirl> ().set_fnc_music (true);
		this.panel_radio_info.SetActive (true);
		this.panel_radio_info_image.sprite = icon_radio;
        this.soud_player_music.GetComponent<RadioPlayer> ().Restart (2f);
		this.soud_player_music.GetComponent<RadioPlayer> ().Station.Name = title_name;
		this.soud_player_music.GetComponent<RadioPlayer> ().Station.Url = url_player;
		this.soud_player_music.GetComponent<RadioPlayer> ().Play ();
		this.Start_img_ware ();
		this.update_ware_music ();
		this.is_radio = true;
		this.slider_music.gameObject.SetActive (false);
		this.txt_info_radio.text = "0.0 KB";
		this.str_name_radio = title_name;
		this.btn_lyric_1.SetActive (false);
		this.btn_lyric_2.SetActive (false);
		this.btn_video.SetActive (false);
		this.panel_chat_act.SetActive (false);
	}

	public void btn_play(){
		this.is_click_control = true;
		if(this.soud_player_music.clip!=null){
			if(this.soud_player_music.isPlaying){
				this.soud_player_music.Pause ();
				this.Img_btn_playe.sprite = this.play;
				this.img_fnc_pause_play.sprite = this.play;
			}else{
				this.soud_player_music.Play ();
				this.Img_btn_playe.sprite = this.stop;
				this.img_fnc_pause_play.sprite = this.stop;
				this.is_click_control = false;
			}
		}
	}

	public void btn_view_bk(){
		if (this.soud_player_music.clip != null&&this.is_radio==false) {
			if (this.is_view_bk) {
				this.is_view_bk = false;
				GameObject.Find ("mygirl").GetComponent<mygirl> ().set_bk_music (false);
			} else {
				GameObject.Find ("mygirl").GetComponent<mygirl> ().set_skybox_Texture (texture);
				this.is_view_bk = true;
				GameObject.Find ("mygirl").GetComponent<mygirl> ().set_bk_music (true);
				GameObject.Find("mygirl").GetComponent<mygirl>().chat_box.view_info_panel_music();
			}
		}
	}

	public void btn_delete_music(){
		this.soud_player_music.clip = null;
		this.soud_player_music.Stop ();
        this.soud_player_music.GetComponent<RadioPlayer>().Stop();
        GameObject.Find ("mygirl").GetComponent<mygirl> ().set_fnc_music (false);
		GameObject.Find ("mygirl").GetComponent<mygirl> ().play_music_and_dance (false);
		if (this.is_view_bk) {
			this.is_view_bk = false;
			GameObject.Find ("mygirl").GetComponent<mygirl> ().set_bk_music (false);
		}

		if (this.panel_audio_setting.activeInHierarchy == true) {
			this.audio_setting_close ();
		}
		if (this.is_radio == false) GameObject.Find("mygirl").GetComponent<mygirl>().chat_box.btn_close_music_info();
		this.gameObject.SetActive (false);
	}

	public void btn_next_music(){
		this.audio_setting_close ();
		if (this.is_offline==false) {
			GameObject.Find ("mygirl").GetComponent<Music_offline> ().next_music ();
		} else {
			WWWForm frm_chat = GameObject.Find("mygirl").GetComponent<mygirl> ().frm_action ("next_music");
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.send(frm_chat, GameObject.Find("mygirl").GetComponent<mygirl>().act_chat_girl);
		}
	}

	public void btn_random_music(){
		this.sel_random++;
		if (this.sel_random > 2) {
			this.sel_random = 0;
		}
		this.img_fnc_random_play.sprite = this.ramdom [this.sel_random];
	}

	public void btn_sound_3d(){
		if (this.is_sound_3d) {
			this.is_sound_3d = false;
			this.pic_sound_3d.color = Color.white;
			this.soud_player_music.GetComponent<Animator> ().enabled = false;
			this.soud_player_music.volume = 1f;
			this.soud_player_music.panStereo = 0f;
			this.point_effect_sound3d.SetActive (false);
		} else {
			this.is_sound_3d = true;
			this.pic_sound_3d.color = Color.yellow;
			this.soud_player_music.GetComponent<Animator> ().enabled = true;
			this.point_effect_sound3d.SetActive (true);
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.show_msg(PlayerPrefs.GetString("list_music","list_music"),PlayerPrefs.GetString("list_music_3d_sound","list_music_3d_sound"), Carrot.Msg_Icon.Alert);
		}
	}

	public void save_music_offline(){
		if (GameObject.Find ("mygirl").GetComponent<Music_offline> ().add_music (this.url_id, this.txt_name_player.text,this.data_music,ToHex(this.waveformColor),this.data_lyric,this.data_link_video,this.data_s_artist,this.data_s_genre,this.data_s_album,this.data_s_year)) {
			this.btn_download.SetActive (false);
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.show_msg(PlayerPrefs.GetString("list_music","list_music"),PlayerPrefs.GetString("ms_add_success","ms_add_success"),Carrot.Msg_Icon.Success);
			GameObject.Find ("mygirl").GetComponent<Music_offline> ().check ();
		}

	}

	public static string ToHex(Color color)
	{
		Color32 c = color;
		var hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", c.r, c.g, c.b, c.a);
		return hex;
	}

	public void btn_audio_setting(){
		if (this.panel_audio_setting.activeInHierarchy) {
			this.audio_setting_close ();
		} else {
			this.panel_audio_setting.SetActive (true);
			this.img_btn_audio_setting.color = Color.yellow;
			GameObject.Find ("mygirl").GetComponent<mygirl> ().show_magic_touch (false);
		}
	}


	public void audio_setting_close(){
		this.panel_audio_setting.SetActive (false);
		this.img_btn_audio_setting.color = Color.white;
		GameObject.Find ("mygirl").GetComponent<mygirl> ().show_magic_touch (true);
	}


	public void show_video(){
        this.is_click_control = true;
        if (this.soud_player_music.clip != null)
        {
            if (this.soud_player_music.isPlaying)
            {
                this.soud_player_music.Pause();
                this.Img_btn_playe.sprite = this.play;
                this.img_fnc_pause_play.sprite = this.play;
            }
        }
        Application.OpenURL (this.data_link_video);
	}

	public void Radio_act_error(Crosstales.Radio.Model.RadioStation station, string info)
	{
		if (!info.Contains("Station is already playing!"))
		{
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.show_msg(PlayerPrefs.GetString("radio", "Radio"), PlayerPrefs.GetString("radio_error", "This radio channel is currently inactive, please try again another time. Now choose another radio station to listen to!"), Carrot.Msg_Icon.Alert);
			this.btn_delete_music();
		}
	}


	public void audio_setting_reset_mixer()
	{
		this.dropdown_ReverbFilters.value = 0;
		this.dropdown_ReverbFilters.RefreshShownValue();
	}

	public void audio_setting_reverbFilterDropdownChanged()
	{
		this.soud_player_music.GetComponent<AudioReverbFilter>().reverbPreset = reverbPresets[dropdown_ReverbFilters.value];
	}

	public void show_share_music()
    {
		string url_share_mp3 = GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_url_host()+ "/music/"+this.id_chat_music+"/"+this.lang_music;
		GameObject.Find("mygirl").GetComponent<mygirl>().carrot.show_share(url_share_mp3, this.txt_name_player.text);
	}

	public void get_mp3_file()
    {
		GameObject.Find("mygirl").GetComponent<mygirl>().buy_product(1);
	}

	public void act_download_mp3()
    {
		string url_download=GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_url_host() + "/app_mygirl/app_my_girl_"+this.lang_music+"/"+this.id_chat_music+".mp3";
		Application.OpenURL(url_download);
    }

}
