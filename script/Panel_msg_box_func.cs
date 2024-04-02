﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Panel_msg_box_func : MonoBehaviour {
	public Transform tr_body;
	public GameObject prefab_music;
	public GameObject prefab_photo;
	public GameObject prefab_contact;
	public GameObject prefab_contact_full;
	public GameObject prefab_radio;
	public GameObject prefab_person;
	public Sprite[] icon;
	public Color32[] colorsex;

	public Color32[] color_option;
	public int sel_option_music = 0;

	private int id_func = 0;

	public GameObject panel_tip_seach_list_music;
	public Button[] btn_opt_music;

	public string send_id_from_sub_menu = "";

	public void show(int func) {
		this.panel_tip_seach_list_music.SetActive(false);
		this.StopAllCoroutines();
		foreach (Transform child in tr_body.transform) {
			Destroy(child.gameObject);
		}
		this.tr_body.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
		if (func == 0) {
			this.gameObject.SetActive(true);
			GameObject.Find("mygirl").GetComponent<mygirl>().show_magic_touch(false);
			this.check_option_music();

			WWWForm frm = GameObject.Find("mygirl").GetComponent<mygirl>().frm_action("list_music");
			frm.AddField("key_seach", GameObject.Find("mygirl").GetComponent<mygirl>().parameter_link);
			frm.AddField("search_option", PlayerPrefs.GetInt("sel_option_music", 0).ToString());
			GameObject.Find("mygirl").GetComponent<mygirl>().is_seach_music_list = true;
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.send(frm, this.act_get_list_music);
		}

		if (func == 1) {
			WWWForm frm = GameObject.Find("mygirl").GetComponent<mygirl>().frm_action("list_background");
			frm.AddField("id_sub_menu", this.send_id_from_sub_menu);
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.send(frm, this.act_get_list_background);
		}

		if (func == 2) {
			WWWForm frm = GameObject.Find("mygirl").GetComponent<mygirl>().frm_action("list_contact");
			frm.AddField("search", GameObject.Find("mygirl").GetComponent<mygirl>().parameter_link);
			frm.AddField("id_sub_menu", this.send_id_from_sub_menu);
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.send(frm, this.act_get_list_contact);
		}

		if (func == 3) {
			WWWForm frm = GameObject.Find("mygirl").GetComponent<mygirl>().frm_action("list_radio");
			frm.AddField("id_sub_menu", this.send_id_from_sub_menu);
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.send(frm, this.act_get_list_radio);
		}

		if (func == 4) {
			WWWForm frm = GameObject.Find("mygirl").GetComponent<mygirl>().frm_action("list_person");
			frm.AddField("id_sub_menu", this.send_id_from_sub_menu);
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.send(frm, this.act_get_list_person);
		}

		this.id_func = func;
	}

	public void check_option_music() {
		this.sel_option_music = PlayerPrefs.GetInt("sel_option_music", 0);
		if (this.sel_option_music == 0) {
			this.btn_opt_music[0].image.color = this.color_option[1];
			this.btn_opt_music[1].image.color = this.color_option[0];
		} else {
			this.btn_opt_music[0].image.color = this.color_option[0];
			this.btn_opt_music[1].image.color = this.color_option[1];
		}
	}

	public void select_option_music_seach(int sel) {
		PlayerPrefs.SetInt("sel_option_music", sel);
		this.check_option_music();
	}

	private void act_get_list_music(string s_data)
	{
		Debug.Log("List music:" + s_data);
		IList list_m = (IList)Carrot.Json.Deserialize(s_data);
		for (int i = 0; i < list_m.Count; i++)
		{
			IDictionary chat = (IDictionary)list_m[i];
			GameObject tip_chat_item = Instantiate(this.prefab_music);
			tip_chat_item.transform.SetParent(tr_body);
			tip_chat_item.transform.localPosition = new Vector3(tip_chat_item.transform.localPosition.x, tip_chat_item.transform.localPosition.y, 0f);
			tip_chat_item.transform.localScale = new Vector3(1f, 1f, 1f);
			tip_chat_item.GetComponent<Panel_msg_item>().id = chat["id_chat"].ToString();
			tip_chat_item.GetComponent<Panel_msg_item>().value.text = chat["chat"].ToString();
			tip_chat_item.GetComponent<Panel_msg_item>().lang = chat["lang_chat"].ToString();
			Color myColor = new Color();
			ColorUtility.TryParseHtmlString(chat["color"].ToString(), out myColor);
			tip_chat_item.GetComponent<Panel_msg_item>().img.color = myColor;
		}
		GameObject.Find("mygirl").GetComponent<mygirl>().is_seach_music_list = true;
		GameObject.Find("mygirl").GetComponent<mygirl>().act_no_magic_touch();
	}

	private void act_get_list_background(string s_data)
	{
		IDictionary data = (IDictionary)Carrot.Json.Deserialize(s_data);
		IList list_bk = (IList)data["list_bk"];
		for (int i = 0; i < list_bk.Count; i++)
		{
			IDictionary bk = (IDictionary)list_bk[i];
			GameObject tip_chat_item = Instantiate(this.prefab_photo);
			tip_chat_item.transform.SetParent(tr_body);
			tip_chat_item.transform.localPosition = new Vector3(tip_chat_item.transform.localPosition.x, tip_chat_item.transform.localPosition.y, 0f);
			tip_chat_item.transform.localScale = new Vector3(1f, 1f, 1f);
			tip_chat_item.GetComponent<Panel_msg_item>().id = bk["url"].ToString();
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_img(bk["url_thumb"].ToString(), tip_chat_item.GetComponent<Panel_msg_item>().img);
		}

		if (data["list_menu"] != null) GameObject.Find("mygirl").GetComponent<Sub_menu>().show_menu_sub((IList)data["list_menu"]);
	}

	private void act_get_list_contact(string s_data)
	{
		IList all_user = (IList)Carrot.Json.Deserialize(s_data);
		for (int i = 0; i < all_user.Count; i++)
		{
			GameObject tip_chat_item = Instantiate(this.prefab_contact);
			IDictionary user = (IDictionary)all_user[i];
			tip_chat_item.transform.SetParent(tr_body);
			tip_chat_item.transform.localPosition = new Vector3(tip_chat_item.transform.localPosition.x, tip_chat_item.transform.localPosition.y, 0f);
			tip_chat_item.transform.localScale = new Vector3(1f, 1f, 1f);
			tip_chat_item.GetComponent<Panel_item_contact>().txt_name.text = user["name"].ToString();
			tip_chat_item.GetComponent<Panel_item_contact>().s_id = user["id"].ToString();
			tip_chat_item.GetComponent<Panel_item_contact>().s_lang = user["lang"].ToString();
			if (int.Parse(user["sex"].ToString()) == 1)
			{
				tip_chat_item.GetComponent<Panel_item_contact>().icon_sex.sprite = icon[1];
				tip_chat_item.GetComponent<Panel_item_contact>().img_panel.color = this.colorsex[1];
			}
			else
			{
				tip_chat_item.GetComponent<Panel_item_contact>().icon_sex.sprite = icon[0];
				tip_chat_item.GetComponent<Panel_item_contact>().img_panel.color = this.colorsex[0];
			}
			if(user["avatar"]!=null) GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_img(user["avatar"].ToString(), tip_chat_item.GetComponent<Panel_item_contact>().avatar);
		}

	}

	private void act_get_list_radio(string s_data)
	{
		IDictionary data = (IDictionary)Carrot.Json.Deserialize(s_data);
		IList list_radio = (IList)data["list_radio"];
		for (int i = 0; i < list_radio.Count; i++)
		{
			IDictionary r = (IDictionary)list_radio[i];
			GameObject radio_item = Instantiate(this.prefab_radio);
			radio_item.transform.SetParent(tr_body);
			radio_item.transform.localPosition = new Vector3(radio_item.transform.localPosition.x, radio_item.transform.localPosition.y, 0f);
			radio_item.transform.localScale = new Vector3(1f, 1f, 1f);
			radio_item.GetComponent<Panel_radio_item>().txt_name.text = r["name_radio"].ToString();
			radio_item.GetComponent<Panel_radio_item>().str_url_stream = r["stream"].ToString();
			if(r["avatar"]!=null)GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_img(r["avatar"].ToString(), radio_item.GetComponent<Panel_radio_item>().ico);
		}
		if (data["list_menu"] != null) GameObject.Find("mygirl").GetComponent<Sub_menu>().show_menu_sub((IList)data["list_menu"]);
	}

	private void act_get_list_person(string s_data)
	{
		IList list_p = (IList)Carrot.Json.Deserialize(s_data);
		for (int i = 0; i < list_p.Count; i++)
		{
			IDictionary chat = (IDictionary)list_p[i];
			GameObject person_item = Instantiate(this.prefab_person);
			person_item.transform.SetParent(tr_body);
			person_item.transform.localPosition = new Vector3(person_item.transform.localPosition.x, person_item.transform.localPosition.y, 0f);
			person_item.transform.localScale = new Vector3(1f, 1f, 1f);
			person_item.GetComponent<Panel_person_item>().data = chat["data"].ToString();
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.get_img(chat["icon"].ToString(), person_item.GetComponent<Panel_person_item>().avatar);
		}
	}


	public void btn_re_load(){
		this.show (this.id_func);
	}

	public void btn_close(){
		this.send_id_from_sub_menu = "";
        StopAllCoroutines ();
		this.gameObject.SetActive (false);
		this.panel_tip_seach_list_music.SetActive (false);
		GameObject.Find ("mygirl").GetComponent<Sub_menu> ().close ();
		GameObject.Find ("mygirl").GetComponent<mygirl> ().show_btn_main (false);
		GameObject.Find ("mygirl").GetComponent<mygirl> ().show_magic_touch (true);
		if (this.id_func == 0) {
			GameObject.Find ("mygirl").GetComponent<mygirl>().is_seach_music_list = false;
		}
	}

	public void set_bk(string url){
		GameObject.Find("mygirl").GetComponent<mygirl>().carrot.slider_loading.maxValue = 1;
		GameObject.Find("mygirl").GetComponent<mygirl>().carrot.show_loading_with_process_bar();
		StartCoroutine (downloadBk (url));
	}

	public IEnumerator downloadBk(string url_bk){
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(url_bk);
		www.SendWebRequest();
		while (!www.isDone)
		{
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.slider_loading.value = www.downloadProgress;
			yield return null;
		}

		if (www.result==UnityWebRequest.Result.Success)
		{
			Texture2D profilePic = ((DownloadHandlerTexture)www.downloadHandler).texture;
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.hide_loading();
			GameObject.Find("mygirl").GetComponent<mygirl>().set_skybox_Texture(profilePic);
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.save_file("bk.png", profilePic.EncodeToPNG());
		}
	}

	public void show_list_contact_full(IList list_contact)
    {
		GameObject.Find("mygirl").GetComponent<mygirl>().carrot.show_list_box("Danh ba", this.icon[0]);
		
		for (int i = 0; i < list_contact.Count; i++)
        {
			IDictionary data_contact =(IDictionary)list_contact[i];
			GameObject item_contact_full = Instantiate(this.prefab_contact_full);
			item_contact_full.transform.SetParent(GameObject.Find("mygirl").GetComponent<mygirl>().carrot.area_body_box);
			item_contact_full.transform.localScale = new Vector3(1f, 1f, 1f);
			item_contact_full.transform.localPosition = new Vector3(item_contact_full.transform.localPosition.x, item_contact_full.transform.localPosition.y, 0f);
			item_contact_full.GetComponent<Panel_item_contact>().txt_name.text = data_contact["name"].ToString();
			item_contact_full.GetComponent<Panel_item_contact>().txt_address.text = data_contact["address"].ToString();
			item_contact_full.GetComponent<Panel_item_contact>().txt_phone.text = data_contact["sdt"].ToString();
			item_contact_full.GetComponent<Panel_item_contact>().s_lang = data_contact["lang"].ToString();
			item_contact_full.GetComponent<Panel_item_contact>().s_id = data_contact["id_device"].ToString();
			if (data_contact["sex"].ToString() == "0")
				item_contact_full.GetComponent<Panel_item_contact>().icon_sex.sprite = this.icon[0];
			else
				item_contact_full.GetComponent<Panel_item_contact>().icon_sex.sprite = this.icon[1];
		}

		GameObject.Find("mygirl").GetComponent<mygirl>().act_no_magic_touch();
	}

}