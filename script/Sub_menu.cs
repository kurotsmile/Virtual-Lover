using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sub_menu : MonoBehaviour {
	public GameObject prefab_sub_menu;
	public GameObject panel_menu_sub;
	public Transform content_sub_menu;

	void Start ()
	{
		this.panel_menu_sub.SetActive (false);
	}

	public void show_menu_sub(IList list_data){
		if (list_data.Count > 0) {
			this.panel_menu_sub.SetActive (true);
			this.panel_menu_sub.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
		}
		foreach (Transform child in this.content_sub_menu) {
			Destroy (child.gameObject);
		}

		for(int i=0;i<list_data.Count;i++){
			GameObject sub_item = Instantiate (this.prefab_sub_menu);
			IList list_index_data =(IList)list_data[i];
			sub_item.GetComponent<Panel_menu_sub_item> ().id = list_index_data[0].ToString();
			sub_item.GetComponent<Panel_menu_sub_item> ().id_func_box= list_index_data[1].ToString();
			sub_item.GetComponent<Panel_menu_sub_item> ().name_act_func_box = list_index_data[2].ToString();
			sub_item.GetComponent<Panel_menu_sub_item> ().txt_value.text= list_index_data[3].ToString();

			if(list_index_data.Count>4){
				Color myColor = new Color ();
				ColorUtility.TryParseHtmlString ("#"+list_index_data[4].ToString (), out myColor);
				sub_item.GetComponent<Panel_menu_sub_item> ().GetComponent<Image> ().color = myColor;
			}


			sub_item.transform.SetParent (this.content_sub_menu);
			sub_item.gameObject.SetActive (true);
			sub_item.transform.localScale = new Vector3 (1f, 1f, 1f);
			sub_item.transform.localPosition = new Vector3 (sub_item.transform.localPosition.x, sub_item.transform.localPosition.y, 0);
		}

	}
		

	public void act_sub_function(string id,string id_func_box,string name_act_func_box){
		if (name_act_func_box == "msg_box_func") {
			GameObject.Find ("mygirl").GetComponent<mygirl> ().panel_msg_func.GetComponent<Panel_msg_box_func> ().send_id_from_sub_menu = id;
			GameObject.Find ("mygirl").GetComponent<mygirl> ().panel_msg_func.GetComponent<Panel_msg_box_func> ().show (int.Parse (id_func_box));
		} else if (name_act_func_box == "show_chat") {
			GameObject.Find ("mygirl").GetComponent<mygirl> ().show_chat_by_id(id);
		} else if (name_act_func_box == "inp_chat") {
			GameObject.Find ("mygirl").GetComponent<mygirl> ().inpText.text = id_func_box;
			GameObject.Find ("mygirl").GetComponent<mygirl> ().play_s ();
		} else if (name_act_func_box == "link") {
			Application.OpenURL (id_func_box);
		} else {
			WWWForm frm=GameObject.Find ("mygirl").GetComponent<mygirl> ().frm_action (name_act_func_box);
			frm.AddField (id_func_box, id);
			//GameObject.Find("mygirl").GetComponent<mygirl>().carrot.send(frm, GameObject.Find("mygirl").GetComponent<mygirl>().act_chat_girl);
		}
	}

	public void act_sub_function_one_on_list(IList list_data){
		if (list_data.Count > 0) {
			IList list_index_data =(IList)list_data[0];
			this.act_sub_function (list_index_data [0].ToString (), list_index_data [1].ToString (), list_index_data [2].ToString ());
		}
	}

	public void close(){
		this.panel_menu_sub.SetActive (false);
	}
}
