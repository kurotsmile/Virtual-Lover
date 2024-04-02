using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_menu_sub_item : MonoBehaviour {

	public string id;
	public Text txt_value;
	public string name_act_func_box;
	public string id_func_box;

	public void onclick(){
		GameObject.Find ("mygirl").GetComponent<Sub_menu> ().act_sub_function (this.id, this.id_func_box, this.name_act_func_box);
	}
}
