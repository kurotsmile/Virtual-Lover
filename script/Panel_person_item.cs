using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_person_item : MonoBehaviour {
	public string data;
	public Image avatar;

	public void click_person(){
		GameObject.Find ("mygirl").GetComponent<mygirl> ().show_btn_main (false);
		GameObject.Find ("figure_girl").GetComponent<Person> ().download_data (this.data);
	}
}
