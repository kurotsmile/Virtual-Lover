using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class prefab_brain_item : MonoBehaviour {
	public int index;
	public Text txt_Question;
	public Text txt_answer;

	public void delete(){
		GameObject.Find ("mygirl").GetComponent<mygirl> ().delete_brain_book (this.index);
	}

	public void play(){
		GameObject.Find ("mygirl").GetComponent<mygirl> ().chat_offline (this.index);
		GameObject.Find("mygirl").GetComponent<mygirl>().carrot.hide_box();
	}
}
