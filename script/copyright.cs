using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copyright : MonoBehaviour
{

	public SpriteRenderer sprite;


	float time_hide=0f;
	void Update ()
	{
		this.time_hide += 1f * Time.deltaTime;
		if (this.time_hide >4f) {
			this.gameObject.SetActive (false);
			this.time_hide = 0f;
		}   
	}

	public void reset(){
		this.time_hide = 0f;
		this.gameObject.SetActive (true);
	}
}
