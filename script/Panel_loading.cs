using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_loading : MonoBehaviour {
	public Text txt_loading;

	public void set_show(bool is_show){
		this.gameObject.SetActive (is_show);
	}
}
