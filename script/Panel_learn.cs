using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Panel_learn : MonoBehaviour {

	[Header("Main Obj")]
	public mygirl app;

	[Header("Learn Obj")]
	public Text question_show_Text;
	public InputField inp_question;
	public InputField inp_answer;
	private int status=0;
	private int effect = 0;
	public Panel_input_voice inp_voice;
	public GameObject btn_done;
	public GameObject panel_question_show;

	private string id_question="";
	private string type_question="";

	public Image[] btn_statu;
	public Image[] btn_effect;

	public Transform body;

	private void reset_btns(Image[] img){
		for (int i = 0; i < img.Length; i++) {
			img [i].color = Color.black;	
		}
	}

	public void sel_status(int sel_index){
		this.status = sel_index;
		this.reset_btns (this.btn_statu);
		this.btn_statu [sel_index].color = Color.red;	
	}

	public void sel_effects(int sel_index){
		this.effect = sel_index;
		this.reset_btns (this.btn_effect);
		this.btn_effect [sel_index].color = Color.red;	
	}

	public void set_question_show(String s_show,string id_questions,string type_questions){
		if (s_show == "") {
			this.panel_question_show.SetActive (false);
			this.id_question = "";
			this.type_question = "";
		} else {
			this.panel_question_show.SetActive (true);
			this.id_question = id_questions;
			this.type_question = type_questions;
			this.question_show_Text.text = s_show;
		}
		this.sel_status (1);
		this.sel_effects (0);
		this.body.GetComponent<ScrollRect> ().verticalNormalizedPosition =1f;
	}

	public void delete_question(){
		GameObject.Find("mygirl").GetComponent<mygirl>().carrot.Show_msg(PlayerPrefs.GetString("learn","learn"),PlayerPrefs.GetString("learn_delete_question","learn_delete_question"),Carrot.Msg_Icon.Alert);
		this.panel_question_show.SetActive (false);
		this.id_question = "";
		this.type_question = "";
	}

	[Obsolete]
	public void submit(){
		if (GameObject.Find ("mygirl").GetComponent<Brain> ().add_brain (this.inp_question.text, this.inp_answer.text,this.effect, this.status) == true) {
			if (this.inp_voice.myAudioRecord.clip != null) {
				Panel_learn.Save ("voice/" + (GameObject.Find ("mygirl").GetComponent<Brain> ().get_length () - 1) + ".wav", this.inp_voice.myAudioRecord.clip);
				PlayerPrefs.SetString ("brain_audio_" + (GameObject.Find ("mygirl").GetComponent<Brain> ().get_length () - 1),GameObject.Find ("mygirl").GetComponent<Brain> ().get_length ()-1+".wav");
			} else {
				PlayerPrefs.SetString ("brain_audio_" + (GameObject.Find ("mygirl").GetComponent<Brain> ().get_length () - 1), "0");
			}
			GameObject.Find ("mygirl").GetComponent<Brain> ().check();
			if (GameObject.Find("mygirl").GetComponent<mygirl>().carrot.is_online()) {
				WWWForm frm = GameObject.Find("mygirl").GetComponent<mygirl>().frm_action("teaching");
				frm.AddField("id", PlayerPrefs.GetString("id"));
				frm.AddField("question", this.inp_question.text);
				frm.AddField("answer", this.inp_answer.text);
				frm.AddField("status", this.status);
				frm.AddField("effect", this.effect);
				frm.AddField("character", PlayerPrefs.GetInt("sel_nv", 0).ToString());
				frm.AddField("id_question", this.id_question);
				frm.AddField("type_question", this.type_question);
				//app.carrot.send_hide(frm, this.act_submit_data);
				this.btn_done.SetActive (false);
			} else {
				this.done_learn_and_close ();
			}
		}
	}

	[Obsolete]
	private void act_submit_data(string s_data){
		this.inp_voice.stop_record();
		this.done_learn_and_close();
	}

	[Obsolete]
	private void done_learn_and_close(){
		this.inp_voice.stop_record ();
		this.inp_voice.delete_voice ();
		this.inp_question.text = "";
		this.inp_answer.text = "";
		GameObject.Find ("mygirl").GetComponent<mygirl> ().show_panel_learn (false);
	}
		
		

        static void ConvertAndWrite (FileStream fileStream, AudioClip clip)
        {
                var samples = new float[clip.samples];
                clip.GetData (samples, 0);
                Int16[] intData = new Int16[samples.Length];
                //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]
                Byte[] bytesData = new Byte[samples.Length * 2];
                //bytesData array is twice the size of
                //dataSource array because a float converted in Int16 is 2 bytes.
                int rescaleFactor = 32767; //to convert float to Int16

                for (int i = 0; i<samples.Length; i++) {
                        intData [i] = (short)(samples [i] * rescaleFactor);
                        Byte[] byteArr = new Byte[2];
                        byteArr = BitConverter.GetBytes (intData [i]);
                        byteArr.CopyTo (bytesData, i * 2);
                }
                fileStream.Write (bytesData, 0, bytesData.Length);
   
        }
 
        static void WriteHeader (FileStream fileStream, AudioClip clip)
        {
                var hz = clip.frequency;
                var channels = clip.channels;
                var samples = clip.samples;

                fileStream.Seek (0, SeekOrigin.Begin);
                Byte[] riff = System.Text.Encoding.UTF8.GetBytes ("RIFF");
   
                fileStream.Write (riff, 0, 4);
                Byte[] chunkSize = BitConverter.GetBytes (fileStream.Length - 8);
   
                fileStream.Write (chunkSize, 0, 4);

                Byte[] wave = System.Text.Encoding.UTF8.GetBytes ("WAVE");
                fileStream.Write (wave, 0, 4);

                Byte[] fmt = System.Text.Encoding.UTF8.GetBytes ("fmt ");
                fileStream.Write (fmt, 0, 4);
 
                Byte[] subChunk1 = BitConverter.GetBytes (16);
                fileStream.Write (subChunk1, 0, 4);
   
                UInt16 one = 1;
                Byte[] audioFormat = BitConverter.GetBytes (one);
                fileStream.Write (audioFormat, 0, 2);
                Byte[] numChannels = BitConverter.GetBytes (channels);
                fileStream.Write (numChannels, 0, 2);
                Byte[] sampleRate = BitConverter.GetBytes (hz);
                fileStream.Write (sampleRate, 0, 4);
                Byte[] byteRate = BitConverter.GetBytes (hz * channels * 2);
                fileStream.Write (byteRate, 0, 4);
                UInt16 blockAlign = (ushort)(channels * 2);
                fileStream.Write (BitConverter.GetBytes (blockAlign), 0, 2);
                UInt16 bps = 16;
                Byte[] bitsPerSample = BitConverter.GetBytes (bps);
                fileStream.Write (bitsPerSample, 0, 2);
                Byte[] datastring = System.Text.Encoding.UTF8.GetBytes ("data");
                fileStream.Write (datastring, 0, 4);
                Byte[] subChunk2 = BitConverter.GetBytes (samples * channels * 2);
                fileStream.Write (subChunk2, 0, 4);
        }


    public static bool Save(string filename, AudioClip clip) {

        if (!filename.ToLower().EndsWith(".wav")) {
            filename += ".wav";
        }

		var filepath = Path.Combine (Application.persistentDataPath, filename);
		if (Application.isEditor) {
			filepath = Path.Combine (Application.dataPath, filename);
		} 
        Debug.Log(filepath);
 
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));
 
		using (var filestream = CreateEmpty(filepath)) {
			ConvertAndWrite(filestream, clip);
			WriteHeader(filestream, clip);
        }
        return true;
    }
        static FileStream CreateEmpty (string filepath)
        {
                var fileStream = new FileStream (filepath, FileMode.Create);
                byte emptyByte = new byte ();
       
                for (int i = 0; i < 0; i++) {
                        fileStream.WriteByte (emptyByte);
                }
                return fileStream;
        }


	public void voice_conmand_question(){

	}

	public void voice_command_answer()
	{

	}


}
