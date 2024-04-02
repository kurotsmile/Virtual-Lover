using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_music_offline_item : MonoBehaviour {
	public Text txt_name_song;
	public int index;
	public Image icon;
	public GameObject btn_lyric;

	public void delete(){
		GameObject.Find ("mygirl").GetComponent<mygirl> ().delete_music_offline (this.index);
	}

	public void play(){
		GameObject.Find ("mygirl").GetComponent<mygirl> ().play_music_offline (index,false);
	}

	public void play_show_lyric(){
		GameObject.Find ("mygirl").GetComponent<mygirl> ().play_music_offline (index,true);
	}
}
