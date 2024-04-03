using Carrot;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Panel_report : MonoBehaviour {
	[Header("obj Main")]
	public mygirl app;

	[Header("Obj Report")]
	public Text question_show_Text;
	public Text txt_limit;
	public report_item[] report_type;
	public GameObject report_edit;
	public GameObject report_limit;
	public GameObject report_other;

	private int sel_type=0;
	private bool type;
	private string id_question;
	private string type_question;

	public Slider slider_limit_report;
	public InputField inp_value;
	public InputField inp_value1;
	public GameObject report_panel_sel;
	public GameObject btn_done;

	public Sprite icon_uncheck;
	public Sprite icon_check;

	public void reset_report(){
		this.report_edit.SetActive (false);
		this.report_limit.SetActive (false);
		this.report_other.SetActive (false);
		for (int i = 1; i < this.report_type.Length; i++) {
			this.report_type [i].icon.sprite = this.icon_uncheck;
		}
	}

	public void set_view(string s_show,bool is_music,string id_que,string type_que){
		this.question_show_Text.text = s_show;

		this.reset_report ();
		for (int i = 1; i < this.report_type.Length; i++) {
			this.report_type [i].gameObject.SetActive (false);
		}
		if (is_music) {
			this.report_type [5].gameObject.SetActive (true);
			this.report_type [6].gameObject.SetActive (true);
		} else {
			this.report_type [1].gameObject.SetActive (true);
			this.report_type [2].gameObject.SetActive (true);
			this.report_type [3].gameObject.SetActive (true);
		}
		this.report_type [4].gameObject.SetActive (true);
		this.show_limit_text ();
		this.type = is_music;
		this.id_question = id_que;
		this.type_question = type_que;
		this.report_panel_sel.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1;
		this.btn_done.SetActive (true);
	}

	public void sel_report(int sel)
	{
		this.reset_report();
		this.sel_type = sel;
		this.report_type[this.sel_type].icon.sprite = this.icon_check;
		this.report_panel_sel.SetActive(false);
		this.report_panel_sel.SetActive(true);

		if (sel_type == 1)
		{
			app.carrot.Show_msg(app.carrot.L("report", "Report"), app.carrot.L("report_1_msg", "report_1_msg"), Carrot.Msg_Icon.Alert);
			this.report_edit.SetActive(true);
		}

		if (sel_type == 3)
		{
			app.carrot.Show_msg(app.carrot.L("report", "Report"),app.carrot.L("report_3_msg", "report_3_msg"), Carrot.Msg_Icon.Alert);
			this.report_limit.SetActive(true);
		}

		if (sel_type == 4)
		{
			app.carrot.Show_msg(app.carrot.L("report", "Report"), app.carrot.L("report_4_msg", "report_4_msg"), Carrot.Msg_Icon.Alert);
			this.report_other.SetActive(true);
		}

		if (sel_type == 5) app.carrot.Show_msg(app.carrot.L("report", "Report"),app.carrot.L("report_5_msg", "report_5_msg"), Carrot.Msg_Icon.Alert);

	}


	public void show_limit_text(){
		int limit_chat =(int) slider_limit_report.value;
		this.txt_limit.text = PlayerPrefs.GetString ("limit_chat_"+limit_chat, "limit_chat_"+limit_chat);
	}

	public void submit(){
		string id_chat_report = "";
		IDictionary data_report = (IDictionary)Json.Deserialize("{}");
		data_report["id"] = id_chat_report;

		string s_data_json = app.carrot.server.Convert_IDictionary_to_json(data_report);
		app.carrot.server.Add_Document_To_Collection("chat",id_chat_report, s_data_json, act_submit_data_report,app.Act_server_fail);
	}

	private void act_submit_data_report(string s_data){
		app.carrot.Show_msg(app.carrot.L("report", "Report"), app.carrot.L("report_success", "Thank you for the error message for the developer. The error message will be reviewed shortly!"), Carrot.Msg_Icon.Success);
		app.show_report(false);
	}

	public void hide_report(){
		this.inp_value.text = "";
		this.sel_type = 0;
		this.gameObject.SetActive (false);
	}

	public void go_to(){
		this.report_panel_sel.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1;
	}

	public void go_bottom(){
		this.report_panel_sel.GetComponent<ScrollRect> ().verticalNormalizedPosition = -1;
	}

	public void btn_voice_command_report_1(){

	}

	public void btn_voice_command_report_other(){

	}
}