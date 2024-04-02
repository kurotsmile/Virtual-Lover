using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_item_contact : MonoBehaviour {
	public Text txt_name;
	public Text txt_phone;
	public Text txt_address;
	public Image avatar;
	public Image icon_sex;
	public Image img_panel;
	public string s_id;
	public string s_lang;
	public void click_show(){
		GameObject.Find ("mygirl").GetComponent<mygirl> ().show_user_info (this.s_id,this.s_lang);
	}
}
