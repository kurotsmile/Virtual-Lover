using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tip_chat : MonoBehaviour {
	public Sprite icon;
	private IList tip_chat;
	private int tip_chat_index=0;
	public Text txt_tip_chat;
	private float count_time_next_tip = 0f;
	public GameObject prefab_tip_chat_item;
	public bool is_active = false;
	void Update () {
		if (is_active)
		{
			this.count_time_next_tip += 1f * Time.deltaTime;
			if (this.count_time_next_tip > 5f)
			{
				this.tip_chat_index++;
				if (this.tip_chat_index >= this.tip_chat.Count)
				{
					this.tip_chat_index = 0;
				}
				this.txt_tip_chat.text = this.tip_chat[this.tip_chat_index].ToString();
				this.count_time_next_tip = 0f;
			}
		}
	}

	public void set_list_tip(IList list){
		this.tip_chat = list;
		this.txt_tip_chat.text = this.tip_chat [0].ToString();
		this.is_active = true;
	}

	public void show_list_tip(Transform area_body){
		foreach (string tip in tip_chat) {
			GameObject tip_chat_item = Instantiate (this.prefab_tip_chat_item);
			tip_chat_item.transform.SetParent (area_body);
			tip_chat_item.transform.localPosition = new Vector3 (tip_chat_item.transform.localPosition.x, tip_chat_item.transform.localPosition.y, 0f);
			tip_chat_item.transform.localScale = new Vector3 (1f, 1f,1f);
			tip_chat_item.GetComponent<Panel_tip_item> ().set_text (tip.ToString());
		}
	}

	public void chat_in_tip(string s_chat)
	{
		WWWForm frm=this.GetComponent<mygirl>().frm_action("chat_tip");
		frm.AddField("text", s_chat);
		this.GetComponent<mygirl>().carrot.send(frm, this.GetComponent<mygirl>().act_chat_girl);
		this.GetComponent<mygirl>().carrot.close();
	}
}
