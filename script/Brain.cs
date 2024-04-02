using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Reign;
using System.IO;
using Carrot;

public class Brain : MonoBehaviour {

    [Header("Obj Main")]
    public mygirl app;

	[Header("Obj Brain")]
	private int length;
	public GameObject icon_book;
	public Sprite icon;
	public GameObject prefab_item;
	private Carrot_Box box;

	public void check() {
		this.length = PlayerPrefs.GetInt ("length_brain", 0);
		if (this.length > 0) {
			this.icon_book.SetActive(true);
		} else {
			this.icon_book.SetActive (false);
		}
	}

	public bool add_brain(string txt_question,string txt_answer,int index_action,int index_face){
		if (txt_question == "" ||txt_question.Length<1) {
			this.app.carrot.Show_msg(app.carrot.L("brain","brain"),app.carrot.L("brain_question_error","brain_question_error"),Carrot.Msg_Icon.Error);
			return false;
		}

		if (txt_answer == "" ||txt_answer.Length<1) {
			this.app.carrot.Show_msg(app.carrot.L("brain","brain"),app.carrot.L("brain_answer_error","brain_answer_error"), Carrot.Msg_Icon.Error);
			return false;
		}

		PlayerPrefs.SetString ("brain_question_" + this.length, txt_question);
		PlayerPrefs.SetString ("brain_answer_" + this.length, txt_answer);
		PlayerPrefs.SetInt ("brain_action_" + this.length, index_action);
		PlayerPrefs.SetInt ("brain_face_" + this.length, index_face);
		this.length++;
		PlayerPrefs.SetInt("length_brain", this.length);
		this.app.carrot.Show_msg(app.carrot.L("brain","brain"),app.carrot.L("brain_add_success","brain_add_success"), Carrot.Msg_Icon.Success);
		return true;
	}

	public int search_brain(string txt_chat){
		if (this.length == 0) {
			return -1;
		}
		for (int i = 0; i < this.length; i++) {
			if (PlayerPrefs.GetString ("brain_question_" + i).Equals (txt_chat)) {
				return i;
			}
		}
		return -1;
	}

	public void show_data_brain(){
        if (this.length > 0)
        {
			this.box=this.app.carrot.Create_Box(app.carrot.L("brain", "Brain (Teaching)"), this.icon);
			for (int i = 0; i < this.length; i++)
            {
				Carrot_Box_Item item_brain=box.create_item("item_brain_" + i);
				item_brain.set_title(PlayerPrefs.GetString("brain_question_" + i));
				item_brain.set_tip(PlayerPrefs.GetString("brain_answer_" + i));
            }
        }
        else
        {
            this.app.carrot.Show_msg(app.carrot.L("brain", "brain"), app.carrot.L("none_data", "No data yet"),Carrot.Msg_Icon.Alert);
        }
	}

	public void delete_brain(int index){
		if (PlayerPrefs.GetString("brain_audio_"+index)!="0"){
			if (Application.isEditor) {
				File.Delete (Application.dataPath + "/voice/" + PlayerPrefs.GetString ("brain_audio_" + index));
			} else {
				File.Delete (Application.persistentDataPath + "/voice/" + PlayerPrefs.GetString ("brain_audio_" + index));
			}
		}

		if (length == 1) {
			PlayerPrefs.SetInt ("length_brain", 0);
			this.check ();
		} else {
			for (int i = index; i < this.length; i++) {
				PlayerPrefs.SetString ("brain_question_" + i, PlayerPrefs.GetString("brain_question_" + (i+1)));
				PlayerPrefs.SetString ("brain_answer_" + i, PlayerPrefs.GetString("brain_answer_" + (i+1)));
				PlayerPrefs.SetInt ("brain_action_" +i, PlayerPrefs.GetInt("brain_action_" + (i+1)));
				PlayerPrefs.SetInt ("brain_face_" + i, PlayerPrefs.GetInt("brain_face_" + (i+1)));
				if (PlayerPrefs.GetString("brain_audio_"+(i+1))!="0"){
					PlayerPrefs.SetString("brain_audio_"+i,PlayerPrefs.GetString("brain_audio_"+(i+1)));
				}else{
					PlayerPrefs.SetString ("brain_audio_" + i, "0");
				}
			}
			this.length--;
			PlayerPrefs.SetInt ("length_brain", this.length);
		}
	}

	public int get_length(){
		return this.length;
	}
}
