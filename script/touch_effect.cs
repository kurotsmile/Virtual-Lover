using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touch_effect : MonoBehaviour {
	public GameObject effect;
	public GameObject effect2;

	private int count_click=0;

	void OnMouseDown(){
		count_click++;
		if (this.count_click == 1) {
			GameObject.Find ("mygirl").GetComponent<mygirl> ().reset_count_next ();
			GameObject.Find ("mygirl").GetComponent<mygirl> ().show_btn_main (true);
		}else if (this.count_click == 2) {
			GameObject.Find ("mygirl").GetComponent<mygirl> ().panel_btn_main.SetActive (false);
			GameObject.Find ("mygirl").GetComponent<mygirl> ().panel_msg_menu.SetActive (false);
		} else {
			GameObject.Find ("mygirl").GetComponent<mygirl> ().show_btn_main (false);
			GameObject.Find ("mygirl").GetComponent<mygirl> ().panel_msg_func.SetActive (false);
		}

		if (count_click >= 3) {
			GameObject.Find ("mygirl").GetComponent<mygirl> ().delete_all_act ();
			Vector3 clickedPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Debug.Log ("click" + Input.mousePosition.x);
			GameObject effect_clone = Instantiate (this.effect);
			effect_clone.transform.position = new Vector3 (clickedPosition.x, clickedPosition.y, -2f);
			Destroy (effect_clone, 1f);
			this.count_click = 0;
#if !UNITY_STANDALONE
			Handheld.Vibrate ();
#endif
			GameObject.Find ("mygirl").GetComponent<mygirl> ().act_hit ();
		} else {
			Vector3 clickedPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Debug.Log ("click" + Input.mousePosition.x);
			GameObject effect_clone = Instantiate (this.effect2);
			effect_clone.transform.position = new Vector3 (clickedPosition.x, clickedPosition.y, -2f);
			Destroy (effect_clone, 1f);
		}
	}
}
