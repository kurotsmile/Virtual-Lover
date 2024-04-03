using UnityEngine;
using UnityEngine.UI;

public class panel_chat_box : MonoBehaviour {
	[Header("Chat Box")]
	public GameObject panel_chat_text;
	public GameObject panel_chat_img;
	public Image img;
	public Text Text_text;
	public Text Text_img;

	[Header("Music info box")]
	public Sprite icon_avatar_default;
	public Image img_avatar_music;
	public GameObject panel_info_music;
	public GameObject panel_act;
	public Text Text_lyric;
	public GameObject panel_lyric;
	public GameObject btn_lyric;
	public GameObject btn_video;
	public GameObject panel_music_artist;
	public GameObject panel_music_year;
	public GameObject panel_music_album;
	public GameObject panel_music_genre;
	public Text txt_name_song;
	public Text txt_music_artist;
	public Text txt_music_year;
	public Text txt_music_album;
	public Text txt_music_genre;

	public void set_show_text(){
		this.Text_text.gameObject.SetActive(true);
		btn_lyric.SetActive (false);
		panel_lyric.SetActive (false);
		panel_chat_text.SetActive (true);
		panel_chat_img.SetActive (false);
		this.panel_act.SetActive (false);
		this.btn_video.SetActive (false);
		this.panel_info_music.SetActive(false);
	}

	public void show_lyric(){
		if (this.panel_lyric.activeInHierarchy == false) {
			panel_lyric.SetActive (true);
			panel_lyric.GetComponent<ScrollRect> ().horizontalNormalizedPosition = -1f;
		} else {
			panel_lyric.SetActive (false);
		}
		panel_chat_text.SetActive (true);
		panel_chat_img.SetActive (false);
	}
		
	public void set_show_image(string url_link){
		this.StopAllCoroutines ();
		this.img.sprite = null;
		panel_chat_text.SetActive (false);
		panel_chat_img.SetActive (true);
		this.Text_img.text = this.Text_text.text;
		GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_img(url_link, this.img);
	}

	public void show_info_music(string name_song,string s_artist,string s_album,string s_genre,string s_year,string url_avatar_music)
	{
		this.panel_music_artist.SetActive(false);
		this.panel_music_album.SetActive(false);
		this.panel_music_year.SetActive(false);
		this.panel_music_genre.SetActive(false);

		if (s_artist.ToString() != "")this.panel_music_artist.SetActive(true);
		if (s_genre.ToString() != "")this.panel_music_genre.SetActive(true);
		if (s_album.ToString() != "") this.panel_music_album.SetActive(true);
		if (s_year.ToString() != "") this.panel_music_year.SetActive(true);

		this.txt_music_artist.text = s_artist;
		this.txt_music_album.text = s_album;
		this.txt_music_genre.text = s_genre;
		this.txt_music_year.text = s_year;
		this.Text_text.gameObject.SetActive(false);
		this.txt_name_song.text = name_song;
		this.panel_info_music.SetActive(true);

		this.img_avatar_music.sprite = this.icon_avatar_default;
		if(url_avatar_music!="") GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_img(url_avatar_music, this.img_avatar_music);
	}

	public void btn_close_music_info()
	{
		this.panel_info_music.SetActive(false);
		this.panel_chat_text.SetActive(true);
		this.Text_text.gameObject.SetActive(true);
		this.panel_act.SetActive(false);
	}

	public void view_info_panel_music()
	{
		this.panel_info_music.SetActive(true);
		this.Text_text.gameObject.SetActive(false);
		this.panel_act.SetActive(true);
	}

	public void show_list_music_buy_type_info(string type_info)
	{
		if (type_info == "artist") GameObject.Find("mygirl").GetComponent<mygirl>().parameter_link = this.txt_music_artist.text;
		if (type_info == "album") GameObject.Find("mygirl").GetComponent<mygirl>().parameter_link = this.txt_music_album.text;
		if (type_info == "year") GameObject.Find("mygirl").GetComponent<mygirl>().parameter_link = this.txt_music_year.text;
		if (type_info == "genre") GameObject.Find("mygirl").GetComponent<mygirl>().parameter_link = this.txt_music_genre.text;

		GameObject.Find("mygirl").GetComponent<mygirl>().panel_msg_func.SetActive(true);
		GameObject.Find("mygirl").GetComponent<mygirl>().panel_msg_func.GetComponent<Panel_msg_box_func>().show(0);
	}
}
