using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Tham so sever ["kiem tra da chon (-1 la khong co,0-3 la chi so chon)","tham so vui ve","chi so buon","chi so thu gian","chi so phan khich"]

public class Panel_select_music_emotions : MonoBehaviour {
	public Color32 color_nomal;
	public Color32 color_sel;

	public Image[] img_panel_sel;
	public Text[] txt_panel_sel;

	private string id_music="";

	public void show(string id_music){
		foreach (Text txt in this.txt_panel_sel) {
			txt.gameObject.SetActive (false);
		}
		this.sel_item_emotion (-1);
		this.gameObject.SetActive (true);
		StartCoroutine (get_emotion_music (id_music));
	}

	public void sel_panel(int index_sel_music){
		StartCoroutine (set_emotion_music (index_sel_music));
	}


	IEnumerator set_emotion_music(int index_emotions){
		WWWForm frm = GameObject.Find ("mygirl").GetComponent<mygirl> ().frm_action ("music_emotion");
		frm.AddField ("id_music", this.id_music);
		frm.AddField ("sel_emotion",index_emotions);
		frm.AddField ("sub_func","set");
		using (UnityWebRequest www = UnityWebRequest.Post(GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_url(), frm))
		{
			yield return www.SendWebRequest();

			if (www.result==UnityWebRequest.Result.Success)
			{
				IList arr_data = (IList)Carrot.Json.Deserialize(www.downloadHandler.text);
				this.check_show_list(arr_data);
				this.sel_item_emotion(int.Parse(arr_data[0].ToString()));
				GameObject.Find("mygirl").GetComponent<mygirl>().carrot.show_msg(PlayerPrefs.GetString("list_music", "list_music"), PlayerPrefs.GetString("music_emotions_msg", "music_emotions_msg"),Carrot.Msg_Icon.Alert);
            }
            else
            {
				this.close_box();
            }
		}
	}

	IEnumerator get_emotion_music(string id_music){
		WWWForm frm = GameObject.Find ("mygirl").GetComponent<mygirl> ().frm_action ("music_emotion");
		frm.AddField ("id_music",id_music);
		frm.AddField ("sub_func","get");

		using (UnityWebRequest www = UnityWebRequest.Post(GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_url(), frm))
		{
			yield return www.SendWebRequest();

			if (www.result==UnityWebRequest.Result.Success)
			{
				IList arr_data = (IList)Carrot.Json.Deserialize(www.downloadHandler.text);
				this.sel_item_emotion(int.Parse(arr_data[0].ToString()));
				this.check_show_list(arr_data);
				this.id_music = id_music;
            }
            else
            {
				this.close_box();
            }
		}
	}

	private void check_show_list(IList arr_data){
		if (arr_data [0].ToString () != "-1") {
			foreach (Text txt in this.txt_panel_sel) {
				txt.gameObject.SetActive (true);
			}
			txt_panel_sel[0].text = arr_data [1].ToString ();
			txt_panel_sel[1].text = arr_data [2].ToString ();
			txt_panel_sel[2].text = arr_data [3].ToString ();
			txt_panel_sel[3].text = arr_data [4].ToString ();
		}
	}

	private void sel_item_emotion(int sel_index){
		foreach(Image img_child in img_panel_sel){
			img_child.color = color_nomal;
		}
		if (sel_index > -1) {
			this.img_panel_sel [sel_index].color = this.color_sel;
		}
	}

	public void close_box(){
		this.StopAllCoroutines ();
		this.gameObject.SetActive (false);
	}
}
