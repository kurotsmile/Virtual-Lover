using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Carrot;

public class Music_offline : MonoBehaviour {
	[Header("Obj Main")]
	public mygirl app;

	[Header("obj Music Offline")]
	private int length;
	public GameObject icon_music;
	public GameObject icon_music_menu;
	public Sprite icon_music_play;
	public Sprite icon;
	public GameObject prefab_item;

	public Color32 color_action;
	public Color32 color_nomal;

	public int index_music_offline = -1;

	public void check() {
		this.length = PlayerPrefs.GetInt ("length_music", 0);
		if (this.length > 0) {
			this.icon_music.SetActive(true);
			this.icon_music_menu.SetActive (true);
		} else {
			this.icon_music.SetActive (false);
			this.icon_music_menu.SetActive (false);
		}
	}

	public bool add_music(string id_song,string name_song,byte[] data_music,string colors,string txt_lyric,string link_video,string s_artist, string s_genre, string s_album,string s_year)
	{
		if (this.length>0) {
			if (check_song_is_exit(id_song)) {
				this.app.carrot.Show_msg(app.carrot.L("list_music","list_music"),app.carrot.L("ms_error_null","ms_error_null"),Carrot.Msg_Icon.Error);
				return false;
			}
		}
		PlayerPrefs.SetString ("ms_id" + this.length, id_song);
		PlayerPrefs.SetString ("ms_name" + this.length, name_song);
		PlayerPrefs.SetString ("ms_file" + this.length, "m"+this.length+".data");;
		PlayerPrefs.SetString ("ms_color" + this.length, colors);
		PlayerPrefs.SetString ("ms_lyric" + this.length, txt_lyric);
        PlayerPrefs.SetString ("ms_video" + this.length, link_video);
		PlayerPrefs.SetString("ms_artist" + this.length, s_artist);
		PlayerPrefs.SetString("ms_album" + this.length, s_album);
		PlayerPrefs.SetString("ms_genre" + this.length, s_genre);
		PlayerPrefs.SetString("ms_year" + this.length, s_year);

		if (Application.isEditor) {
			System.IO.File.WriteAllBytes (Application.dataPath + "/" + PlayerPrefs.GetString ("ms_file"+ this.length), data_music);
		} else {
			System.IO.File.WriteAllBytes (Application.persistentDataPath + "/" + PlayerPrefs.GetString ("ms_file"+ this.length), data_music);
		}
			

		this.length++;
		PlayerPrefs.SetInt("length_music", this.length);
		return true;
	}

	public bool check_song_is_exit(string id_song){
		if (this.length == 0) {
			return false;
		}

		for (int i = 0; i < this.length; i++) {
			if (PlayerPrefs.GetString ("ms_id" + i).Equals (id_song)) {
				return true;
			}
		}
		return false;
	}

	public void show_list_music_offline(){
		Carrot_Box box=this.app.carrot.Create_Box(app.carrot.L("list_music_offline", "Playlist offline"), this.icon);
		for (int i = 0; i < this.length; i++) {
			GameObject brain_item = Instantiate (this.prefab_item);
			brain_item.transform.SetParent (box.area_all_item);
			brain_item.transform.localPosition = new Vector3 (brain_item.transform.localPosition.x, brain_item.transform.localPosition.y, 0f);
			brain_item.transform.localScale = new Vector3 (1f, 1f,1f);
			brain_item.GetComponent<Panel_music_offline_item> ().index = i;
			brain_item.GetComponent<Panel_music_offline_item> ().txt_name_song.text=PlayerPrefs.GetString("ms_name"+i);
			if (PlayerPrefs.GetString ("ms_lyric" + i, "") != "") {
				brain_item.GetComponent<Panel_music_offline_item> ().btn_lyric.SetActive (true);
			} else {
				brain_item.GetComponent<Panel_music_offline_item> ().btn_lyric.SetActive (false);
			}

			if (this.index_music_offline== i) {
				brain_item.GetComponent<Panel_music_offline_item> ().icon.sprite = icon_music_play;
				brain_item.GetComponent<Image> ().color = Color.blue;
			}
		}
	}

	public void delete_brain(int index){
		if (PlayerPrefs.GetString("ms_id"+index,"0")!="0"){
			if (Application.isEditor) {
				File.Delete (Application.dataPath + "/" + PlayerPrefs.GetString ("ms_file" + index));
			} else {
				File.Delete (Application.persistentDataPath + "/" + PlayerPrefs.GetString ("ms_file" + index));
			}
		}

		if (length == 1) {
			PlayerPrefs.SetInt ("length_music", 0);
			this.check ();
		} else {
			for (int i = index; i < this.length; i++) {
				PlayerPrefs.SetString ("ms_id" + i, PlayerPrefs.GetString("ms_id" + (i+1)));
				PlayerPrefs.SetString ("ms_name" + i, PlayerPrefs.GetString("ms_name" + (i+1)));
				PlayerPrefs.SetString ("ms_color" + i, PlayerPrefs.GetString("ms_color" + (i+1)));
				PlayerPrefs.SetString ("ms_lyric" + i, PlayerPrefs.GetString("ms_lyric" + (i+1)));
                PlayerPrefs.SetString("ms_video" + i, PlayerPrefs.GetString("ms_video" + (i + 1),""));
				PlayerPrefs.SetString("ms_artist" + i, PlayerPrefs.GetString("ms_artist" + (i + 1), ""));
				PlayerPrefs.SetString("ms_album" + i, PlayerPrefs.GetString("ms_album" + (i + 1), ""));
				PlayerPrefs.SetString("ms_genre" + i, PlayerPrefs.GetString("ms_genre" + (i + 1), ""));
				PlayerPrefs.SetString("ms_year" + i, PlayerPrefs.GetString("ms_year" + (i + 1), ""));
				if (PlayerPrefs.GetString("ms_file"+(i+1))!="0"){
					PlayerPrefs.SetString("ms_file"+i,PlayerPrefs.GetString("ms_file"+(i+1)));
				}else{
					PlayerPrefs.SetString ("ms_file" + i, "0");
				}
			}
			this.length--;
			PlayerPrefs.SetInt ("length_music", this.length);
		}
	}

	public int get_length(){
		return this.length;
	}

	public void set_action(bool is_act){
		if (is_act) {
			this.icon_music.GetComponent<Image> ().color = this.color_action;
		} else {
			this.icon_music.GetComponent<Image> ().color = this.color_nomal;
		}
	}

	public void next_music(){
		this.index_music_offline++;
		if (this.index_music_offline >= this.length) {
			this.index_music_offline = 0;
		}
		this.play_music (index_music_offline);
	}

	public void play_music(int index){
		string s = "";
		if (Application.isEditor) {
			s="file://" + Application.dataPath + "/" + PlayerPrefs.GetString ("ms_file" + index);
		} else {
			s="file://" + Application.persistentDataPath + "/" + PlayerPrefs.GetString ("ms_file" + index);
		}
		Color myColor = new Color();
		ColorUtility.TryParseHtmlString("#"+PlayerPrefs.GetString ("ms_color" + index), out myColor);

		if (PlayerPrefs.GetString ("ms_lyric" + index, "") == "") {
			this.GetComponent<mygirl> ().chat_box.panel_lyric.SetActive (false);
		} else {
			this.GetComponent<mygirl> ().chat_box.Text_lyric.text = "\n\n"+PlayerPrefs.GetString ("ms_lyric" + index)+"\n";
		}

		this.GetComponent<mygirl> ().panel_music.set_download_wating( PlayerPrefs.GetString ("ms_name"+index), s, myColor,false,PlayerPrefs.GetString ("ms_lyric" + index),PlayerPrefs.GetString ("ms_video" + index,""), PlayerPrefs.GetString("ms_artist" + index), PlayerPrefs.GetString("ms_album" + index), PlayerPrefs.GetString("ms_genre" + index), PlayerPrefs.GetString("ms_year" + index),"","");
	}

	public void play_random_music(){
		this.index_music_offline = Random.Range (0, this.length);
		this.play_music (index_music_offline);
	}
		

	[ContextMenu ("Delete All music")]
	public void delete_all(){
		if (this.length > 0) {
			for (int i = 0; i < this.length; i++) {
				if (PlayerPrefs.GetString("ms_id"+i,"0")!="-1"){
					if (Application.isEditor) {
						File.Delete (Application.dataPath + "/" + PlayerPrefs.GetString ("ms_file" + i));
					} else {
						File.Delete (Application.persistentDataPath + "/" + PlayerPrefs.GetString ("ms_file" + i));
					}
				}
			}
			this.length = 0;
			PlayerPrefs.SetInt ("length_music", this.length);
		}
	}
}
