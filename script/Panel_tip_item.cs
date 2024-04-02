using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_tip_item : MonoBehaviour {
	public Text txt;
	public void send_chat(){
		GameObject.Find("mygirl").GetComponent<Tip_chat>().chat_in_tip(this.txt.text);
	}

	public void set_text(string txt){
		this.txt.text = txt;
	}
}
