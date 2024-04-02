using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_msg_item : MonoBehaviour {
	public string id;
	public Text value;
	public Image img;
	public string lang;
	public void action_click(){
		if (value == null) {
			GameObject.Find ("panel_msg_box_func").GetComponent<Panel_msg_box_func> ().set_bk (this.id);
		} else {
			if (PlayerPrefs.GetInt ("sel_option_music", 0) == 0) {
				GameObject.Find ("mygirl").GetComponent<mygirl> ().show_chat_by_id (this.id);
			} else {
				GameObject.Find ("mygirl").GetComponent<mygirl> ().show_chat_by_id_lang (this.id,this.lang);
			}
		}

	}
}
