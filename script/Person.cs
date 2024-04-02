using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Person : MonoBehaviour {
	public Sprite sprite_copyright_sex0;
	public Sprite[] sprite_statu_1_sex0;
	public Sprite[] sprite_statu_2_sex0;
	public Sprite[] sprite_statu_3_sex0;
	public Sprite[] sprite_statu_4_sex0;

	public Sprite sprite_copyright_sex1;
	public Sprite[] sprite_statu_1_sex1;
	public Sprite[] sprite_statu_2_sex1;
	public Sprite[] sprite_statu_3_sex1;
	public Sprite[] sprite_statu_4_sex1;

	public Sprite[] sprite_statu_1;
	public Sprite[] sprite_statu_2;
	public Sprite[] sprite_statu_3;
	public Sprite[] sprite_statu_4;


	private Sprite[] sprite_anim;

	public SpriteRenderer spRender;
	public copyright copyright_pic;

	private float time_next_img;
	private int index_img=0;
	private int sex=0;

	//Temp data
	private int count_file = 0;
	private Sprite[] arr_sp_statu1=new Sprite[4];
	private Sprite[] arr_sp_statu2=new Sprite[4];
	private Sprite[] arr_sp_statu3=new Sprite[4];
	private Sprite[] arr_sp_statu4=new Sprite[4];
	private Sprite[] arrSp_coppyright=new Sprite[1];

	// Update is called once per frame
	void Update () {
		this.time_next_img +=1f * Time.deltaTime;
		if(this.time_next_img>2f){
			this.index_img++;
			this.spRender.sprite = this.sprite_anim [this.index_img];
			this.time_next_img = 0f;
			if(this.index_img>=this.sprite_anim.Length-1){
				this.index_img = -1;
			}
		}

	}

	public void set_status(string status_s){
		this.index_img = 0;
		if (status_s == "0") {
			this.sprite_anim = this.arr_sp_statu4;
		}

		if (status_s == "1") {
			this.sprite_anim = this.arr_sp_statu1;
		}

		if (status_s == "2") {
			this.sprite_anim = this.arr_sp_statu3;
		}

		if (status_s == "3") {
			this.sprite_anim = this.arr_sp_statu2;
		}
		this.spRender.sprite = this.sprite_anim [this.index_img];
	}



	public void download_data(string data){
		IDictionary data_status= (IDictionary) Carrot.Json.Deserialize(data);
		this.count_file = 0;
		arr_sp_statu1=new Sprite[4];
		arr_sp_statu2=new Sprite[4];
		arr_sp_statu3=new Sprite[4];
		arr_sp_statu4=new Sprite[4];
		arrSp_coppyright=new Sprite[1];

		IList data_status1 =(IList)data_status["status1"];
		for (int i = 0; i < data_status1.Count; i++) {
			Debug.Log (data_status1 [i].ToString ()+" status 1");
			StartCoroutine(this.download_status(arr_sp_statu1,i,data_status1 [i].ToString ()));
		}

		IList data_status2 =(IList)data_status["status2"];
		for (int i = 0; i < data_status2.Count; i++) {
			Debug.Log (data_status2 [i].ToString ()+" status 2");
			StartCoroutine(this.download_status(arr_sp_statu2,i,data_status2 [i].ToString ()));
		}

		IList data_status3 =(IList)data_status["status3"];
		for (int i = 0; i < data_status3.Count; i++) {
			Debug.Log (data_status3 [i].ToString ()+" status 3");
			StartCoroutine(this.download_status(arr_sp_statu3,i,data_status3 [i].ToString ()));
		}

		IList data_status4 =(IList)data_status["status4"];
		for (int i = 0; i < data_status4.Count; i++) {
			Debug.Log (data_status4 [i].ToString ()+" status 4");
			StartCoroutine(this.download_status(arr_sp_statu4,i,data_status4 [i].ToString ()));
		}
		StartCoroutine(this.download_status(arrSp_coppyright,0,data_status["copyright"].ToString()));
	}


	public IEnumerator download_status(Sprite[] arrSp,int indexID,string effect_str){
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(effect_str);
		yield return www.SendWebRequest();

		if (www.result!=UnityWebRequest.Result.Success)
		{
			GameObject.Find("mygirl").GetComponent<mygirl>().carrot.hide_loading();
			this.StopAllCoroutines();
		}
		else
		{
			Texture2D profilePicRequest = ((DownloadHandlerTexture)www.downloadHandler).texture;
			arrSp[indexID] = Sprite.Create(profilePicRequest, new Rect(0.0f, 0.0f, profilePicRequest.width, profilePicRequest.height), new Vector2(0.5f, 0.5f), 100.0f);
			this.count_file++;
			if (this.count_file >= 17)
			{
				GameObject.Find("mygirl").GetComponent<mygirl>().carrot.hide_loading();
				Debug.Log("done all");
				this.sprite_statu_1 = arr_sp_statu1;
				this.sprite_statu_2 = arr_sp_statu2;
				this.sprite_statu_3 = arr_sp_statu3;
				this.sprite_statu_4 = arr_sp_statu4;

				this.save_status(1, this.sprite_statu_1);
				this.save_status(2, this.sprite_statu_2);
				this.save_status(3, this.sprite_statu_3);
				this.save_status(4, this.sprite_statu_4);

				this.copyright_pic.sprite.sprite = arrSp_coppyright[0];
				this.copyright_pic.reset();
				if (Application.isEditor)
				{
					System.IO.File.WriteAllBytes(Application.dataPath + "/copyright" + this.sex + ".png", arrSp_coppyright[0].texture.EncodeToPNG());
				}
				else
				{
					System.IO.File.WriteAllBytes(Application.persistentDataPath + "/copyright" + this.sex + ".png", arrSp_coppyright[0].texture.EncodeToPNG());
				}

				this.sprite_anim = sprite_statu_1;
				GameObject.Find("panel_msg_box_func").GetComponent<Panel_msg_box_func>().btn_close();
			}
		}
	}

	public void save_status(int index_status,Sprite[] sp){
		for (int i = 0; i < sp.Length; i++) {
			if (Application.isEditor) {
				System.IO.File.WriteAllBytes (Application.dataPath+"/status_"+this.sex+"_"+index_status+"_"+i+".png", sp[i].texture.EncodeToPNG());
			} else {
				System.IO.File.WriteAllBytes ( Application.persistentDataPath+"/status_"+this.sex+"_"+index_status+"_"+i+".png", sp[i].texture.EncodeToPNG());
			}
		}	
	}
		
	public void load_status(int sex){
		this.sex = sex;
		bool is_load_success;
		is_load_success=this.load_status_item (1, this.arr_sp_statu1);
		if (is_load_success == true) {
			is_load_success = this.load_status_item (2, this.arr_sp_statu2);
		}

		if (is_load_success == true) {
			is_load_success = this.load_status_item (3, this.arr_sp_statu3);
		}

		if (is_load_success == true) {
			is_load_success = this.load_status_item (4, this.arr_sp_statu4);
		}

		if (is_load_success == true) {
			is_load_success = this.load_copyright ();
		}

		if (is_load_success==false) {
			if (this.sex==0) {
				this.arr_sp_statu1 = this.sprite_statu_1_sex0;
				this.arr_sp_statu2 = this.sprite_statu_2_sex0;
				this.arr_sp_statu3 = this.sprite_statu_3_sex0;
				this.arr_sp_statu4 = this.sprite_statu_4_sex0;
				this.arrSp_coppyright [0] = this.sprite_copyright_sex0;
			} else {
				this.arr_sp_statu1 = this.sprite_statu_1_sex1;
				this.arr_sp_statu2 = this.sprite_statu_2_sex1;
				this.arr_sp_statu3 = this.sprite_statu_3_sex1;
				this.arr_sp_statu4 = this.sprite_statu_4_sex1;
				this.arrSp_coppyright [0] = this.sprite_copyright_sex1;
			}
		}

		this.sprite_statu_1 = arr_sp_statu1;
		this.sprite_statu_2 = arr_sp_statu2;
		this.sprite_statu_3 = arr_sp_statu3;
		this.sprite_statu_4 = arr_sp_statu4;
		this.copyright_pic.sprite.sprite = this.arrSp_coppyright [0];
		this.copyright_pic.reset ();
		this.sprite_anim = sprite_statu_1;
	}


	public bool load_status_item(int index_status,Sprite[] arrSp){
		for (int i = 0; i < arrSp.Length; i++) {
			string name_file_status = "";

			if (Application.isEditor) {
				name_file_status = Application.dataPath + "/status_" + this.sex + "_" + index_status + "_" + i + ".png";
			} else {
				name_file_status = Application.persistentDataPath + "/status_" + this.sex + "_" + index_status + "_" + i + ".png";
			}

			if (System.IO.File.Exists (name_file_status)) {
				Texture2D load_s01_texture;
				byte[] bytes;
				if (Application.isEditor) {
					bytes = System.IO.File.ReadAllBytes (name_file_status);
				} else {
					bytes = System.IO.File.ReadAllBytes (name_file_status);
				}

				load_s01_texture = new Texture2D (1, 1);
				load_s01_texture.LoadImage (bytes);
				arrSp [i] = Sprite.Create(load_s01_texture,new Rect(0.0f,0.0f,load_s01_texture.width,load_s01_texture.height),new Vector2(0.5f,0.5f),100.0f);
			} else {
				return false;
			}
		}
		return true;
	}

	public bool load_copyright(){
		string name_file_status;
		if (Application.isEditor) {
			name_file_status = Application.dataPath + "/copyright" + this.sex + ".png";
		} else {
			name_file_status = Application.persistentDataPath + "/copyright" + this.sex + ".png";
		}

		if (System.IO.File.Exists (name_file_status)) {
			Texture2D load_s01_texture;
			byte[] bytes;
			if (Application.isEditor) {
				bytes = System.IO.File.ReadAllBytes (name_file_status);
			} else {
				bytes = System.IO.File.ReadAllBytes (name_file_status);
			}

			load_s01_texture = new Texture2D (1, 1);
			load_s01_texture.LoadImage (bytes);
			arrSp_coppyright[0] = Sprite.Create(load_s01_texture,new Rect(0.0f,0.0f,load_s01_texture.width,load_s01_texture.height),new Vector2(0.5f,0.5f),100.0f);
		} else {
			return false;
		}
		return true;
	}
}