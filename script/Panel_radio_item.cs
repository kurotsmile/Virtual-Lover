using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_radio_item : MonoBehaviour {

	public string str_url_stream;
	public Image ico;
	public Text txt_name;

	public void click(){
		Debug.Log ("click radio");
		GameObject.Find ("mygirl").GetComponent<mygirl> ().play_radio (this.txt_name.text, this.str_url_stream,this.ico.sprite);
	}
}
