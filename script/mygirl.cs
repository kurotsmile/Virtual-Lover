using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using KKSpeech;
using UnityEngine.Advertisements;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using Carrot;

public class mygirl : MonoBehaviour
{
    public Carrot.Carrot carrot;

    [Header("Panel App")]
    public GameObject panel_button;
    public GameObject panel_setting;
    public GameObject panel_learn;
    public GameObject panel_report;
    public GameObject panel_msg_func;
    public GameObject panel_msg_menu;
    public GameObject panel_fnc_music;
    public GameObject panel_btn_main;
    public GameObject panel_btn_music_emotions;
    public GameObject panel_chat_me;
    public panel_chat_box chat_box;
    public Panel_music_player panel_music;
    public Recording_comand panel_recoding;

    [Header("Object chat")]
    public AudioSource speech;
    public InputField inpText;
    public Text txt_girl_chat;
    public Color32 color_chat;
    public Color32 color_sel_nomal;
    public Color32 color_sel_select;
    public Image sp_chat;
    public GameObject button_login_in_home;
    public GameObject icon_loading;
    public Text txt_chat_me;
    public AudioSource sound_chat;
    public Image img_sound;
    private int sound = 0;
    private float count_time_next_chat = 0f;
    private float count_byebye = 0f;
    private int count_ads = 0;
    private int count_ads_2 = 0;
    private bool byebye = false;
    private int sex = 0;
    public Text txt_show_inp_chat;
    private string id_question = "";
    private string type_question = "";
    private string func_server = "";
    private string str_effect;

    [Header("Object Game")]
    public GameObject Magic_tocuh;
    public Skybox bk;
    public Person person;
    public GameObject area_effect;

    [Header("Setting")]
    public Sprite[] icon_on_off;
    public Image icon_lang;
    public Image setting_img_icon_lang;
    public Text setting_txt_name_lang;
    public Slider slider_setting_voice_speed;
    public Slider slider_setting_limit_chat;
    public InputField inputField_character_name;
    public Text txt_setting_limit_chat_tip;
    public AudioSource audio_setting_test;
    public Image[] btn_sel_sex;
    public Text txt_setting_effect;
    public Text txt_setting_color_chat;
    public Image icon_setting_effect;
    public Image icon_setting_color_chat;

    private AudioClip sex0Audio_setting_test;
    private AudioClip sex1Audio_setting_test;
    private int setting_effect_chat = 1;
    private int setting_color_chat = 1;
    private int val_setting_sex;
    private int val_setting_effect_chat;
    private int val_setting_color_chat;
    public GameObject panel_setting_removeAds;
    public GameObject button_removeads_box_msg;
    public GameObject button_viewads_box_msg;

    [Header("Object Default")]
    public Texture2D bk_default;
    public Sprite avatar_default;
    public Sprite[] icon;
    public GameObject prefab_effect_customer;
    public GameObject[] effect_temp;


    private bool is_waiting_voice = false;
    public Image icon_avatar;

#if UNITY_IOS
	private string gameId = "1647930";
#elif UNITY_ANDROID
    private string gameId = "1647929";
    private AndroidJavaObject camera1;
#else
    private string gameId = "1647929";
#endif

    public string parameter_link = "";
    public bool is_seach_music_list = false;

    void Start()
    {
        this.set_sex(PlayerPrefs.GetInt("sex", 0));
        this.person.load_status(PlayerPrefs.GetInt("sex", 0));
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;

        this.carrot.Load_Carrot(check_ext_app);
        this.carrot.shop.onCarrotPaySuccess = this.onBuySuccessPayCarrot;
        this.carrot.shop.onCarrotRestoreSuccess = this.onRestoreSuccessPayCarrot;
        this.carrot.act_after_close_all_box = this.act_after_close_all_box;

        this.GetComponent<Tip_chat>().is_active = false;
        
        this.GetComponent<Brain>().check();
        this.GetComponent<Music_offline>().check();

        this.sound = PlayerPrefs.GetInt("sound", 0);
        this.setting_effect_chat = PlayerPrefs.GetInt("setting_effect_chat", 1);
        this.setting_color_chat = PlayerPrefs.GetInt("setting_color_chat", 1);
        this.check_sound();

        this.load_background();

        this.panel_button.SetActive(true);
        this.panel_setting.SetActive(false);
        this.panel_chat_me.SetActive(false);
        this.panel_learn.SetActive(false);
        this.panel_report.SetActive(false);
        this.panel_music.gameObject.SetActive(false);
        this.panel_msg_func.SetActive(false);
        this.panel_msg_menu.SetActive(false);
        this.panel_fnc_music.SetActive(false);
        this.icon_loading.SetActive(false);
        this.panel_btn_main.SetActive(true);
        this.panel_btn_music_emotions.SetActive(false);
    }

    private void check_ext_app()
    {
        if (this.panel_msg_func.activeInHierarchy)
        {
            this.panel_msg_func.GetComponent<Panel_msg_box_func>().btn_close();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_setting.activeInHierarchy)
        {
            this.panel_setting.SetActive(false);
            this.Magic_tocuh.SetActive(true);
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_recoding.panel_voice_maximize.activeInHierarchy)
        {
            this.panel_recoding.stop_recoding();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_msg_menu.activeInHierarchy)
        {
            this.panel_msg_menu.SetActive(false);
            this.carrot.set_no_check_exit_app();
        }
    }

    private void load_ads()
    {
        if (PlayerPrefs.GetInt("is_buy_ads", 0) == 0)
        {
            if (Advertisement.isSupported&&Advertisement.isInitialized==false) Advertisement.Initialize(gameId);
            this.panel_setting_removeAds.SetActive(true);
            if (this.carrot.is_online())
            {
                this.button_removeads_box_msg.SetActive(true);
                this.button_viewads_box_msg.SetActive(true);
            }
        }
        else
        {
            this.panel_setting_removeAds.SetActive(false);
            if (this.carrot.is_online())
            {
                this.button_removeads_box_msg.SetActive(false);
                this.button_viewads_box_msg.SetActive(false);
            }
        }
    }

    public void load_app_online()
    {
        this.load_ads();
        if (PlayerPrefs.GetString("lang") == "")
        {
            this.no_magic_touch();
            this.carrot.show_list_lang(this.load_msg_start);
        }
        else
            this.load_msg_start("");
    }

    public void load_app_offline()
    {
        this.txt_girl_chat.text = PlayerPrefs.GetString("lost_connection_msg", "Lost connection! please check the network connection or 3g, wifi");
    }

    private void act_after_close_all_box()
    {
        if(this.panel_learn.activeInHierarchy==false&&this.panel_report.activeInHierarchy==false)
        this.show_magic_touch(true);
    }

    public void act_no_magic_touch()
    {
        this.carrot.delay_function(1f, no_magic_touch);
    }

    private void no_magic_touch() { this.show_magic_touch(false);}


    public void load_msg_start(string s_data)
    {
        if (SpeechRecognizer.ExistsOnDevice())
        {
            SpeechRecognizer.SetDetectionLanguage(PlayerPrefs.GetString("key_voice", "en-US"));
            SpeechRecognizer.RequestAccess();
            SpeechRecognizer.SetDetectionLanguage(PlayerPrefs.GetString("key_voice", "en-US"));
        }

    }

    public WWWForm frm_action(string str_func)
    {
        WWWForm frm_chat =new WWWForm();
        frm_chat.AddField("id_question", this.id_question);
        frm_chat.AddField("type_question", this.type_question);
        frm_chat.AddField("sex", PlayerPrefs.GetInt("sex", 0));
        frm_chat.AddField("lang", PlayerPrefs.GetString("key", "vi"));
        frm_chat.AddField("version", "1");
        frm_chat.AddField("dates", DateTime.Now.Date.Day.ToString());
        frm_chat.AddField("year", DateTime.Now.Date.Year.ToString());
        frm_chat.AddField("months", DateTime.Now.Month.ToString());
        frm_chat.AddField("hour", DateTime.Now.Hour.ToString());
        frm_chat.AddField("minute", DateTime.Now.Minute.ToString());
        frm_chat.AddField("id_device", SystemInfo.deviceUniqueIdentifier);
        frm_chat.AddField("day_week", DateTime.Now.DayOfWeek.ToString());
        frm_chat.AddField("limit_chat", PlayerPrefs.GetInt("limit_chat", 4).ToString());
        if (this.sex == 0)
            frm_chat.AddField("character_sex", "1");
        else
            frm_chat.AddField("character_sex", "0");
        return frm_chat;
    }

    public void check_show_ads()
    {
        this.show_ads();
    }

    public void show_ads()
    {
#if UNITY_STANDALONE
        this.no_magic_touch();
        this.carrot.ads.show_ads_Interstitial();
#else
        Advertisement.Show();
#endif
    }

    public void act_hit()
    {
        this.count_time_next_chat = 0;
        WWWForm frm_chat = this.frm_action("hit");
        //this.carrot.send(frm_chat, act_chat_girl);
    }

    void Update()
    {
        if (
            this.panel_setting.activeInHierarchy == false &&
            this.panel_learn.activeInHierarchy == false &&
            this.str_effect != "2" &&
            this.panel_msg_func.activeInHierarchy == false &&
            this.panel_report.activeInHierarchy == false &&
            this.panel_msg_menu.activeInHierarchy == false&&
            this.carrot.is_online()
            )
        {
            if (this.speech.isPlaying == false)
            {
                this.count_time_next_chat += 1f * Time.deltaTime;
                if (this.count_time_next_chat > 23f)
                {
                    WWWForm frm_chat = this.frm_action("bat_chuyen");
                    //this.carrot.send(frm_chat,act_chat_girl);
                    this.count_time_next_chat = 0f;
                    this.panel_chat_me.SetActive(false);
                }
            }

            if (this.byebye == true)
            {
                this.count_byebye += 1f * Time.deltaTime;
                if (this.count_byebye > 6f)
                {
                    Application.Quit();
                    this.count_byebye = 0f;
                    this.byebye = false;
                    Debug.Log("Exit application");
                }
            }

            if (sound == 0)
            {
                if (this.panel_recoding.panel_voice.activeInHierarchy)
                {
                    if (this.speech.isPlaying == false && this.panel_recoding.auto_chat == true && SpeechRecognizer.IsRecording() == false && this.is_waiting_voice == true)
                    {
                        this.is_waiting_voice = false;
                        this.panel_recoding.play_recoding();
                    }
                }
            }

        }
    }

    IEnumerator downloadAudio(string s_chat, AudioType type_audio)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(s_chat, type_audio))
        {
            yield return www.SendWebRequest();

            if (www.result==UnityWebRequest.Result.Success)
            {
                speech.clip = DownloadHandlerAudioClip.GetContent(www);
                speech.Play();
                if (this.str_effect != "2")
                {
                    if (this.sex == 0)
                        this.speech.pitch = PlayerPrefs.GetFloat("voice_speed" + PlayerPrefs.GetInt("sex", 0), 1.2f);
                    else
                        this.speech.pitch = PlayerPrefs.GetFloat("voice_speed" + PlayerPrefs.GetInt("sex", 0), 1f);
                    this.is_waiting_voice = true;
                }
            }
        }
    }

    public IEnumerator downloadEffectCustomer(string effect_str)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(effect_str);
        yield return www.SendWebRequest();
        if (www.result==UnityWebRequest.Result.Success) this.create_effect_customer(((DownloadHandlerTexture)www.downloadHandler).texture);
    }

    public void create_effect_customer(Texture txt)
    {
        GameObject customer_effect = Instantiate(this.prefab_effect_customer);
        customer_effect.transform.SetParent(this.area_effect.transform);
        customer_effect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.mainTexture = txt;
        customer_effect.GetComponent<ParticleSystem>().collision.SetPlane(0, this.area_effect.transform);
        customer_effect.name = "customer_effect";
        Destroy(customer_effect, 6f);
    }

    IEnumerator downloadAudio_setting(string s_chat, int type_audi)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(s_chat, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result==UnityWebRequest.Result.Success)
            {
                if (type_audi == 0)
                {
                    this.sex0Audio_setting_test = DownloadHandlerAudioClip.GetContent(www);
                }
                else
                {
                    this.sex1Audio_setting_test = DownloadHandlerAudioClip.GetContent(www);
                    if (this.sex == 0)
                    {
                        this.slider_setting_voice_speed.value = PlayerPrefs.GetFloat("voice_speed0", 1.2f);
                        this.audio_setting_test.clip = this.sex0Audio_setting_test;
                    }
                    else
                    {
                        this.slider_setting_voice_speed.value = PlayerPrefs.GetFloat("voice_speed1", 1f);
                        this.audio_setting_test.clip = this.sex1Audio_setting_test;
                    }
                }
            }
        }
    }

    public void play_s()
    {
        if (this.is_seach_music_list == true && this.panel_msg_func.activeInHierarchy == true)
        {
            this.parameter_link = this.inpText.text.ToLower();
            this.panel_msg_func.GetComponent<Panel_msg_box_func>().show(0);
        }
        else
        {
            if (this.inpText.text != "")
            {
                int index_chat = this.GetComponent<Brain>().search_brain(this.inpText.text);
                if (index_chat == (-1))
                {
                    if (this.carrot.is_online())
                    {
                        this.icon_loading.SetActive(true);
                        WWWForm frm_chat = this.frm_action("chat");
                        frm_chat.AddField("func_server", this.func_server);
                        frm_chat.AddField("text", this.inpText.text.ToLower());
                       //this.carrot.send(frm_chat, act_chat_girl);

                        this.txt_chat_me.text = this.inpText.text;
                        this.panel_chat_me.SetActive(true);

                        if (PlayerPrefs.GetInt("is_buy_ads", 0) == 0)
                        {
                            this.count_ads++;
                            if (this.count_ads > 10)
                            {
                                this.check_show_ads();
                                this.count_ads = 0;
                            }
                        }
                    }
                    else
                        this.chat_offline(index_chat);
                }
                else
                    this.chat_offline(index_chat);
            }
            this.panel_msg_func.SetActive(false);
            this.show_btn_main(false);
        }
        this.count_time_next_chat = 0f;
    }

    public void act_chat_girl(string s_data)
    {
        if (this.carrot.model_app == Carrot.ModelApp.Develope) Debug.Log("Chat:" + s_data);

        IDictionary data = (IDictionary)Carrot.Json.Deserialize(s_data);
        IDictionary chat = (IDictionary)data["chat"];

        if (chat["effect"].ToString() != "2") this.txt_girl_chat.text = chat["chat"].ToString().ToLower();
        this.chat_box.set_show_text();

        this.person.set_status(chat["status"].ToString());

        if (this.setting_color_chat == 1)
        {
            if (chat["color"].ToString() == "")
                this.sp_chat.color = this.color_chat;
            else
            {
                Color myColor = new Color();
                ColorUtility.TryParseHtmlString(chat["color"].ToString(), out myColor);
                this.sp_chat.color = myColor;
            }
        }

        string s_chat = chat["chat"].ToString().ToLower();

        this.id_question = chat["id_chat"].ToString();
        this.type_question = chat["type_chat"].ToString();

        if (chat["func_sever"] != null)
            this.func_server = chat["func_sever"].ToString();
        else
            this.func_server = "";

        if (s_chat.Contains("{gio}")) s_chat = s_chat.Replace("{gio}", DateTime.Now.Hour.ToString());
        if (s_chat.Contains("{phut}")) s_chat = s_chat.Replace("{phut}", DateTime.Now.Minute.ToString());
        if (s_chat.Contains("{thu}")) s_chat = s_chat.Replace("{thu}", DateTime.Now.DayOfWeek.ToString());
        if (s_chat.Contains("{ngay}")) s_chat = s_chat.Replace("{ngay}", DateTime.Now.Date.Day.ToString());
        if (s_chat.Contains("{thang}")) s_chat = s_chat.Replace("{thang}", DateTime.Now.Month.ToString());
        if (s_chat.Contains("{nam}")) s_chat = s_chat.Replace("{nam}", DateTime.Now.Year.ToString());
        if (s_chat.Contains("{ngaycuanam}")) s_chat = s_chat.Replace("{ngaycuanam}", (360 - DateTime.Now.DayOfYear).ToString());
        if (s_chat.Contains("{ngaycuanam2}")) s_chat = s_chat.Replace("{ngaycuanam2}", DateTime.Now.DayOfYear.ToString());
        if (s_chat.Contains("{key_chat}")) s_chat = s_chat.Replace("{key_chat}", this.inpText.text);

        if (s_chat.Contains("{ten_nv}")) s_chat = s_chat.Replace("{ten_nv}",this.get_name_char(this.sex));

        if (s_chat.Contains("{ten_user}"))
        {
            if (this.carrot.user.get_id_user_login() != "")
            {
                string s_name_user = "\""+this.carrot.user.get_data_user_login("name")+"\"";
                s_chat=s_chat.Replace("{ten_user}", s_name_user);
            }
            else
                s_chat= s_chat.Replace("{ten_user}","");
        }
        this.txt_girl_chat.text = s_chat;

        this.icon_loading.SetActive(false);

        if (chat["link"]!=null&&chat["link"].ToString()!="")
        {
            this.parameter_link = chat["link"].ToString();
            if (chat["effect"].ToString() != "29" && chat["effect"].ToString() != "31")
            {
                Application.OpenURL(chat["link"].ToString());
            }
        }

#if !UNITY_STANDALONE
        if (chat["vibrate"].ToString() != "")Handheld.Vibrate();
#endif

        if (chat["effect"].ToString() != "")
        {
            if (chat["effect"].ToString() != "2") this.create_effect(chat["effect"].ToString());


            if (chat["effect"].ToString() == "44")
            {
                if (data["all_tip"] != null)
                {
                    GameObject.Find("mygirl").GetComponent<Sub_menu>().act_sub_function_one_on_list((IList)data["all_tip"]);
                    data["all_tip"] = "";
                }
            }
        }

        if (chat["effect_customer"].ToString() != "")
        {
            if (this.setting_effect_chat == 1) StartCoroutine(downloadEffectCustomer(chat["effect_customer"].ToString()));
        }


        if (this.sound == 0)
        {
            if (chat["mp3"].ToString() != "" && chat["effect"].ToString() != "2")
            {
                string url_voice_mp3;
                if (chat["mp3"].ToString() == "google")
                    url_voice_mp3 = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=" + s_chat.Length + "&client=tw-ob&q=" + s_chat + "&tl=" + chat["lang"].ToString();
                else
                    url_voice_mp3 = chat["mp3"].ToString();
                StartCoroutine(downloadAudio(url_voice_mp3, AudioType.MPEG));
            }
        }

        if (chat["mp3"].ToString() != "" && chat["effect"].ToString() == "2")
        {
            this.GetComponent<Music_offline>().set_action(false);
            if(chat["data_text"]!=null) this.chat_box.Text_lyric.text = "\n\n" + chat["data_text"].ToString() + "\n";
            this.panel_music.set_download_wating(chat["chat"].ToString(), chat["mp3"].ToString(), this.sp_chat.color, true, chat["data_text"].ToString(), chat["video"].ToString(), chat["artist"].ToString(), chat["album"].ToString(), chat["genre"].ToString(), chat["year"].ToString(), chat["avatar_music"].ToString(), chat["lang"].ToString());
            this.panel_music.id_chat_music = this.id_question;
        }

        if (chat["field_chat"] != null)
        {
            IList all_field_chat = (IList)chat["field_chat"];
            if (all_field_chat.Count > 0)
                this.GetComponent<Sub_menu>().show_menu_sub(all_field_chat);
            else
                this.GetComponent<Sub_menu>().close();
        }
        else this.GetComponent<Sub_menu>().close();

        if (data["tip_chat"]!=null) this.GetComponent<Tip_chat>().set_list_tip((IList)data["tip_chat"]);
        if (data["list_contact"]!= null) this.panel_msg_func.GetComponent<Panel_msg_box_func>().show_list_contact_full((IList)data["list_contact"]);

            this.inpText.text = "";
        this.parameter_link = "";
        this.Magic_tocuh.SetActive(true);
    }

    public void create_effect(string effect_str)
    {
        this.str_effect = effect_str;
        foreach (Transform child_effect in this.area_effect.transform)
        {
            if (child_effect.name == "music_effect") Destroy(child_effect.gameObject);
            if (child_effect.name == "rain") Destroy(child_effect.gameObject);
            if (child_effect.name == "snow") Destroy(child_effect.gameObject);
        }
        //love
        if (effect_str == "1")
        {
            GameObject effect_nv = Instantiate(this.effect_temp[0]);
            effect_nv.transform.SetParent(this.area_effect.transform);
            Destroy(effect_nv, 6f);
        }
        //music
        if (effect_str == "2")
        {
            GameObject effect_nv = Instantiate(this.effect_temp[1]);
            effect_nv.name = "music_effect";
            effect_nv.transform.SetParent(this.area_effect.transform);
        }
        //flower
        if (effect_str == "3")
        {
            GameObject effect_nv = Instantiate(this.effect_temp[2]);
            effect_nv.transform.SetParent(this.area_effect.transform);
            Destroy(effect_nv, 6f);
        }
        //bye bye -star
        if (effect_str == "4")
        {
            GameObject effect_nv = Instantiate(this.effect_temp[3]);
            effect_nv.transform.SetParent(this.area_effect.transform);
            Destroy(effect_nv, 6f);
            this.byebye = true;
        }
        //rain
        if (effect_str == "5")
        {
            GameObject effect_nv = Instantiate(this.effect_temp[4]);
            effect_nv.name = "rain";
            effect_nv.transform.SetParent(this.area_effect.transform);
        }
        //camera
        if (effect_str == "6")this.carrot.camera_pro.Show_camera();
        //snow
        if (effect_str == "7")
        {
            GameObject effect_nv = Instantiate(this.effect_temp[5]);
            effect_nv.name = "snow";
            effect_nv.transform.SetParent(this.area_effect.transform);
        }
#if !UNITY_STANDALONE
        //bat den pin
        if (effect_str == "8")
        {
            AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
            WebCamDevice[] devices = WebCamTexture.devices;
            camera1 = cameraClass.CallStatic<AndroidJavaObject>("open", 0);

            if (camera1 != null)
            {
                AndroidJavaObject cameraParameters = camera1.Call<AndroidJavaObject>("getParameters");
                cameraParameters.Call("setFlashMode", "torch");
                camera1.Call("setParameters", cameraParameters);
                camera1.Call("startPreview");
            }
        }
        //tac den pin
        if (effect_str == "9")
        {
            if (camera1 != null)
            {
                camera1.Call("stopPreview");
                camera1.Call("release");
            }
        }
#endif
        //off music
        if (effect_str == "10") this.panel_music.btn_delete_music();
        //List music
        if (effect_str == "11") this.show_list_music();
        //List background
        if (effect_str == "12") this.show_list_background();
        if (effect_str == "13") this.show_learn_question();
        //read load background
        if (effect_str == "16")this.load_background();
        //close object
        if (effect_str == "22")this.panel_msg_func.GetComponent<Panel_msg_box_func>().btn_close();
        //brain
        if (effect_str == "23")this.show_panel_learn(true);
        //show ads 1
        if (effect_str == "26")this.view_ads_btn();
        //show ads 2
        if (effect_str == "27")this.check_show_ads();
        //share
        if (effect_str == "28")this.app_share();

        //Show chat imager
        if (effect_str == "29")
        {
            this.panel_msg_menu.SetActive(false);
            this.chat_box.set_show_image(this.parameter_link);
        }
        //next nhac
        if (effect_str == "30")this.panel_music.btn_next_music();
        //Show me
        if (effect_str == "32") this.show_user();
        //Show list radio
        if (effect_str == "33") this.show_list_radio();
        //show list person
        if (effect_str == "35") this.show_list_person();
        //open Rate app
        if (effect_str == "45")this.rate_app();
        //open app other
        if (effect_str == "46")this.show_app_other();

    }

    public void view_ads_btn()
    {
        if (this.count_ads_2 == 0) this.show_ads();

        if (this.count_ads_2 == 1) this.show_ads();

        if (this.count_ads_2 == 2)
        {
            this.show_ads();
            this.button_viewads_box_msg.SetActive(false);
        }
        count_ads_2++;
    }

    public void load_background()
    {
        string name_file_bk = "";
        if (Application.isEditor)
        {
            name_file_bk = Application.dataPath + "/bk.png";
        }
        else
        {
            name_file_bk = Application.persistentDataPath + "/bk.png";
        }

        if (System.IO.File.Exists(name_file_bk))
        {
            Texture2D load_s01_texture;
            byte[] bytes;
            if (Application.isEditor)
            {
                bytes = System.IO.File.ReadAllBytes(name_file_bk);
            }
            else
            {
                bytes = System.IO.File.ReadAllBytes(name_file_bk);
            }

            load_s01_texture = new Texture2D(1, 1);
            load_s01_texture.LoadImage(bytes);
            this.set_skybox_Texture(load_s01_texture);
        }
        else
        {
            this.set_skybox_Texture(this.bk_default);
        }
    }

    public void set_sound()
    {
        if (this.sound == 0)
            this.sound = 1;
        else
        {
            this.sound = 0;
            this.sound_chat.Play();
        }
        PlayerPrefs.SetInt("sound", this.sound);
        this.check_sound();
    }

    public void check_sound()
    {
        if (this.sound == 0)
        {
            this.img_sound.sprite = this.icon[2];
            this.speech.mute = false;
        }
        else
        {
            this.img_sound.sprite = this.icon[3];
            this.speech.mute = true;
        }
    }


    public void set_sex(int intsex)
    {
        this.btn_sel_sex[0].color = this.color_sel_nomal;
        this.btn_sel_sex[1].color = this.color_sel_nomal;

        if (intsex == 0)
        {
            this.btn_sel_sex[1].color = this.color_sel_select;
            this.sex = 0;
        }
        else
        {
            this.btn_sel_sex[0].color = this.color_sel_select;
            this.sex = 1;
        }

        if (this.sex == 0)
        {
            this.slider_setting_voice_speed.value = PlayerPrefs.GetFloat("voice_speed0", 1.2f);
            this.audio_setting_test.clip = this.sex0Audio_setting_test;
        }
        else
        {
            this.slider_setting_voice_speed.value = PlayerPrefs.GetFloat("voice_speed1", 1f);
            this.audio_setting_test.clip = this.sex1Audio_setting_test;
        }
    }

    public void set_sex_setting(int intsex)
    {
        this.btn_sel_sex[0].color = this.color_sel_nomal;
        this.btn_sel_sex[1].color = this.color_sel_nomal;
        this.val_setting_sex = intsex;
        this.inputField_character_name.text = this.get_name_char(intsex);
        if (intsex == 0)
        {
            this.btn_sel_sex[1].color = this.color_sel_select;
            this.slider_setting_voice_speed.value = PlayerPrefs.GetFloat("voice_speed0", 1.2f);
            this.audio_setting_test.clip = this.sex0Audio_setting_test;
        }
        else
        {
            this.btn_sel_sex[0].color = this.color_sel_select;
            this.slider_setting_voice_speed.value = PlayerPrefs.GetFloat("voice_speed1", 1f);
            this.audio_setting_test.clip = this.sex1Audio_setting_test;
        }
    }

    public void reset_count_next()
    {
        if (this.inpText.text.ToString() == "")
        {
            this.txt_show_inp_chat.text = "";
            this.txt_show_inp_chat.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            this.txt_show_inp_chat.text = this.inpText.text + "_";
            this.txt_show_inp_chat.transform.parent.gameObject.SetActive(true);
        }
        this.count_time_next_chat = 0f;
    }

    public void rate_app()
    {
        this.carrot.show_rate();
    }

    public void set_messager()
    {
        this.inpText.text = this.GetComponent<Tip_chat>().txt_tip_chat.text;
    }

    public void setting_reset_all_data()
    {
        this.GetComponent<Music_offline>().delete_all();
        this.carrot.get_tool().delete_file("bk.png");
        this.set_skybox_Texture(this.bk_default);
        this.panel_setting.SetActive(false);
        this.load_app_online();
    }

    public void btn_menu()
    {
        this.val_setting_color_chat = this.setting_color_chat;
        this.val_setting_effect_chat = this.setting_effect_chat;
        this.setting_img_icon_lang.sprite = this.icon_lang.sprite;
        this.setting_txt_name_lang.text = PlayerPrefs.GetString("lang_name", "English");
        if (this.audio_setting_test.clip != null)
        {
            this.audio_setting_test.Stop();
            this.audio_setting_test.clip = null;
        }
        this.Magic_tocuh.SetActive(false);
        this.panel_setting.SetActive(true);
        this.slider_setting_limit_chat.value = PlayerPrefs.GetInt("limit_chat", 4);

        this.load_audio_voice_test_setting();
        this.check_limit_chat();
        this.set_sex_setting(this.sex);
        this.check_setting_on_off_effect_chat();
        this.check_setting_on_off_color_chat();
    }

    public void load_audio_voice_test_setting()
    {
        string str_audio_0 = PlayerPrefs.GetString("setting_url_sound_test_sex0");
        string str_audio_1 = PlayerPrefs.GetString("setting_url_sound_test_sex1");
        if (str_audio_0 != "") StartCoroutine(downloadAudio_setting(str_audio_0, 0));
        if (str_audio_1 != "") StartCoroutine(downloadAudio_setting(str_audio_1, 1));
    }

    public void btn_done_setting()
    {
        this.id_question = "";
        this.type_question = "";
        this.sex = this.val_setting_sex;
        PlayerPrefs.SetInt("sex", this.val_setting_sex);
        PlayerPrefs.SetInt("setting_effect_chat", this.val_setting_effect_chat);
        PlayerPrefs.SetInt("setting_color_chat", this.val_setting_color_chat);

        if (this.val_setting_sex == 0)
            PlayerPrefs.SetFloat("voice_speed0", this.slider_setting_voice_speed.value);
        else
            PlayerPrefs.SetFloat("voice_speed1", this.slider_setting_voice_speed.value);

        PlayerPrefs.SetInt("limit_chat", (int)this.slider_setting_limit_chat.value);
        this.person.load_status(this.val_setting_sex);
        this.set_name_char(this.inputField_character_name.text, this.sex);
        this.load_msg_start("");
        this.panel_msg_menu.SetActive(false);
        this.panel_msg_func.SetActive(false);
        this.panel_setting.SetActive(false);
    }

    public void show_panel_learn(bool is_show)
    {
        if (is_show)
        {
            this.Magic_tocuh.SetActive(false);
        }
        else
        {
            this.Magic_tocuh.SetActive(true);
        }
        this.panel_learn.GetComponent<Panel_learn>().btn_done.SetActive(is_show);
        this.panel_learn.SetActive(is_show);
        this.panel_learn.GetComponent<Panel_learn>().set_question_show("", "", "");
    }


    public void chat_offline(int index_brain)
    {
        this.icon_loading.SetActive(false);
        this.inpText.text = "";
        this.txt_girl_chat.text = PlayerPrefs.GetString("brain_answer_" + index_brain).ToString().ToLower();
        Debug.Log("Chat offline:" + PlayerPrefs.GetString("brain_answer_" + index_brain));
        if (PlayerPrefs.GetInt("brain_action_" + index_brain).ToString() != "")
        {
            this.create_effect(PlayerPrefs.GetInt("brain_action_" + index_brain).ToString());
        }

        this.person.set_status(PlayerPrefs.GetInt("brain_face_" + index_brain).ToString());

        if (PlayerPrefs.GetString("brain_audio_" + index_brain) != "0")
        {
            if (Application.isEditor)
                StartCoroutine(this.downloadAudio("file://" + Application.dataPath + "/voice/" + PlayerPrefs.GetString("brain_audio_" + index_brain), AudioType.WAV));
            else
                StartCoroutine(this.downloadAudio("file://" + Application.persistentDataPath + "/voice/" + PlayerPrefs.GetString("brain_audio_" + index_brain), AudioType.WAV));
        }
        this.Magic_tocuh.SetActive(true);
    }

    public void show_panel_list_brain()
    {
        this.Magic_tocuh.SetActive(false);
        this.GetComponent<Brain>().show_data_brain();
    }

    public void show_panel_tip_chat()
    {
        this.Magic_tocuh.SetActive(false);
        Carrot_Box box=this.carrot.Create_Box(carrot.L("tip_chat", "Tip chat"), this.GetComponent<Tip_chat>().icon);
        this.GetComponent<Tip_chat>().show_list_tip(box.area_all_item);
    }

    public void delete_brain_book(int index)
    {
        this.GetComponent<Brain>().delete_brain(index);
        this.GetComponent<Brain>().show_data_brain();
    }

    public void delete_music_offline(int index)
    {
        this.GetComponent<Music_offline>().delete_brain(index);
        this.GetComponent<Music_offline>().show_list_music_offline();
    }

    public void app_share()
    {
        this.carrot.show_share();
    }

    public void check_limit_chat()
    {
        int limit_chat = (int)slider_setting_limit_chat.value;
        this.txt_setting_limit_chat_tip.text = PlayerPrefs.GetString("limit_chat_" + limit_chat, "limit_chat_" + limit_chat);
    }

    public void setting_play_audio_test()
    {
        if (this.audio_setting_test.clip != null)
        {
            this.audio_setting_test.Play();
        }
        this.audio_setting_test.pitch = this.slider_setting_voice_speed.value;
    }

    public void set_skybox_Texture(Texture textT)
    {
        this.panel_msg_func.SetActive(false);
        this.show_btn_main(false);
        Material result = new Material(Shader.Find("RenderFX/Skybox"));
        result.SetTexture("_FrontTex", textT);
        result.SetTexture("_BackTex", textT);
        result.SetTexture("_LeftTex", textT);
        result.SetTexture("_RightTex", textT);
        result.SetTexture("_UpTex", textT);
        result.SetTexture("_DownTex", textT);
        this.bk.material = result;
    }


    public void play_music_and_dance(bool is_player)
    {
        if (is_player)
            this.create_effect("2");
        else
            this.str_effect = "0";
    }

    public void set_bk_music(bool is_bool)
    {
        if (is_bool == false)
        {
            this.load_background();
            this.panel_btn_music_emotions.SetActive(false);
        }

        if (is_bool == true)
        {
            if (this.panel_music.is_offline == true && this.panel_music.is_radio == false)
            {
                this.panel_btn_main.SetActive(false);
                this.panel_btn_music_emotions.GetComponent<Panel_select_music_emotions>().show(this.panel_music.id_chat_music);
            }
        }
    }

    public void set_fnc_music(bool is_bool)
    {
        this.panel_fnc_music.SetActive(is_bool);
    }

    public void view_url_copyright()
    {
        Application.OpenURL(PlayerPrefs.GetString("setting_copyright_url", "http://carrotstore.com/privacy_policy"));
    }

    [ContextMenu("List Music")]
    public void show_list_music()
    {
        this.panel_msg_menu.SetActive(false);
        this.panel_msg_func.SetActive(true);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().show(0);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().panel_tip_seach_list_music.SetActive(true);
    }

    [ContextMenu("List Background")]
    public void show_list_background()
    {
        this.panel_msg_menu.SetActive(false);
        this.panel_msg_func.SetActive(true);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().show(1);
    }


    [ContextMenu("List contact")]
    public void show_list_contact()
    {
        this.parameter_link = "";
        this.panel_msg_menu.SetActive(false);
        this.panel_msg_func.SetActive(true);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().show(2);
    }

    [ContextMenu("show Learn")]
    public void show_learn_question()
    {
        this.Magic_tocuh.SetActive(false);
        this.panel_learn.gameObject.SetActive(true);
        this.panel_learn.GetComponent<Panel_learn>().set_question_show(this.txt_girl_chat.text, this.id_question, this.type_question);
        this.panel_learn.GetComponent<Panel_learn>().btn_done.SetActive(true);
        this.panel_msg_menu.SetActive(false);
    }
    public void show_learn_question_customer(string msg_chat)
    {
        this.Magic_tocuh.SetActive(false);
        this.panel_learn.gameObject.SetActive(true);
        this.panel_learn.GetComponent<Panel_learn>().set_question_show("", "", "");
        this.panel_learn.GetComponent<Panel_learn>().inp_question.text = msg_chat;
        this.panel_learn.GetComponent<Panel_learn>().btn_done.SetActive(true);
        this.panel_msg_menu.SetActive(false);
    }

    [ContextMenu("List Radio")]
    public void show_list_radio()
    {
        this.panel_msg_menu.SetActive(false);
        this.panel_msg_func.SetActive(true);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().show(3);
    }

    [ContextMenu("List Person")]
    public void show_list_person()
    {
        this.panel_msg_menu.SetActive(false);
        this.panel_msg_func.SetActive(true);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().show(4);
    }


    public void show_report(bool is_show)
    {
        if (is_show)
        {
            this.Magic_tocuh.SetActive(false);
            this.panel_report.SetActive(true);
            if (this.str_effect == "2")
            {
                this.panel_report.GetComponent<Panel_report>().set_view(this.txt_girl_chat.text, true, this.id_question, this.type_question);
            }
            else
            {
                this.panel_report.GetComponent<Panel_report>().set_view(this.txt_girl_chat.text, false, this.id_question, this.type_question);
            }
            this.panel_msg_menu.SetActive(false);
        }
        else
        {
            this.Magic_tocuh.SetActive(true);
            this.panel_report.GetComponent<Panel_report>().hide_report();
        }
    }

    public void show_chat_by_id(string id)
    {
        this.show_btn_main(false);
        WWWForm frm_chat = this.frm_action("show_chat");
        frm_chat.AddField("id_chat", id);
        //this.carrot.send(frm_chat, this.act_chat_girl);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().btn_close();
    }

    public void show_chat_by_id_lang(string id, string lang)
    {
        this.show_btn_main(false);
        WWWForm frm_chat = this.frm_action("show_chat");
        frm_chat.AddField("id_chat", id);
        frm_chat.AddField("lang_chat", lang);
        //this.carrot.send(frm_chat, this.act_chat_girl);
        this.panel_msg_func.GetComponent<Panel_msg_box_func>().btn_close();
    }


    public void show_magic_touch(bool is_show)
    {
        this.Magic_tocuh.SetActive(is_show);
        if (is_show)
            this.str_effect = "0";
        else
            this.str_effect = "2";
    }

    public void delete_all_act()
    {
        StopAllCoroutines();
    }

    public void show_btn_main(bool is_show)
    {
        if (is_show)
        {
            this.panel_btn_main.SetActive(false);
            this.panel_msg_menu.SetActive(true);
            this.panel_btn_music_emotions.SetActive(false);
            this.button_login_in_home.SetActive(false);
        }
        else
        {
            this.panel_btn_main.SetActive(true);
            this.panel_msg_menu.SetActive(false);
            if(this.carrot.is_online())this.button_login_in_home.SetActive(true);
            GameObject.Find("mygirl").GetComponent<Sub_menu>().close();
        }
    }

    public void show_user_info(string s_id, string s_lang)
    {
        this.carrot.user.show_user_by_id(s_id, s_lang);
        this.show_btn_main(false);
    }

    public void show_user()
    {
        this.Magic_tocuh.SetActive(false);
        this.carrot.show_login();
    }

    public void show_panel_list_music_offline()
    {
        this.Magic_tocuh.SetActive(false);
        this.GetComponent<Music_offline>().show_list_music_offline();
    }

    public void play_music_offline(int index, bool is_show_lyric)
    {
        if (is_show_lyric)this.chat_box.panel_lyric.SetActive(true);
        this.GetComponent<Music_offline>().index_music_offline = index;
        this.GetComponent<Music_offline>().set_action(true);
        this.GetComponent<Music_offline>().play_music(index);
        this.Magic_tocuh.SetActive(true);
        this.carrot.close();
    }

    public void play_radio(string name_radio, string url, Sprite icon_radio)
    {
        this.show_btn_main(false);
        this.str_effect = "2";
        this.panel_music.set_radio(name_radio, url, icon_radio);
    }

    public void app_exit(){Application.Quit();}

    public void show_app_other()
    {
        this.carrot.show_list_carrot_app();
        this.act_no_magic_touch();
    }

    public void show_list_sel_lang()
    {
        this.show_magic_touch(false);
        if (this.panel_setting.activeInHierarchy == true)
            this.carrot.show_list_lang(check_name_for_setting);
        else
            this.carrot.show_list_lang(this.load_msg_start);
    }

    private void check_name_for_setting(string s_data)
    {
        this.setting_txt_name_lang.text = PlayerPrefs.GetString("lang_name", "English");
        if (this.val_setting_sex == 0)
            this.inputField_character_name.text = PlayerPrefs.GetString("name_sex_0", PlayerPrefs.GetString("name_char_girl", "Thi"));
        else
            this.inputField_character_name.text = PlayerPrefs.GetString("name_sex_1", PlayerPrefs.GetString("name_char_boy", "Thắng"));
    }

    private string get_name_char(int char_sex)
    {
        string name_char;
        string s_lang = PlayerPrefs.GetString("lang", "en");
        if (char_sex == 0)
            name_char = PlayerPrefs.GetString("name_sex_0_"+ s_lang, PlayerPrefs.GetString("name_char_girl", "Thi"));
        else
            name_char = PlayerPrefs.GetString("name_sex_1_" + s_lang, PlayerPrefs.GetString("name_char_boy", "Thắng"));

        return name_char;
    }

    private void set_name_char(string new_name,int char_sex)
    {
        if (new_name.Trim() != "")
        {
            string name_old = get_name_char(char_sex);
            string s_lang = PlayerPrefs.GetString("lang", "en");
            if (new_name != name_old)
            {
                if (char_sex == 0)
                    PlayerPrefs.SetString("name_sex_0_" + s_lang, new_name);
                else
                    PlayerPrefs.SetString("name_sex_1_" + s_lang, new_name);
            }
        }
    }

    public void setting_on_off_effect_chat()
    {
        if (this.val_setting_effect_chat == 0)
            this.val_setting_effect_chat = 1;
        else
            this.val_setting_effect_chat = 0;

        this.check_setting_on_off_effect_chat();
    }

    private void check_setting_on_off_effect_chat()
    {
        this.icon_setting_effect.sprite = this.icon_on_off[this.val_setting_effect_chat];
        this.txt_setting_effect.text = PlayerPrefs.GetString("setting_effect_chat_" + this.val_setting_effect_chat, "Off chat effect");
    }

    public void setting_on_off_color_chat()
    {
        if (this.val_setting_color_chat == 0)
        {
            this.val_setting_color_chat = 1;
        }
        else
        {
            this.val_setting_color_chat = 0;
        }
        this.check_setting_on_off_color_chat();
    }

    private void check_setting_on_off_color_chat()
    {
        this.icon_setting_color_chat.sprite = this.icon_on_off[this.val_setting_color_chat];
        this.txt_setting_color_chat.text = PlayerPrefs.GetString("setting_color_chat_" + this.val_setting_color_chat, "Off chat effect");
    }

    public void buy_success(Product product)
    {
        this.onBuySuccessPayCarrot(product.definition.id);
    }

    private void onBuySuccessPayCarrot(string s_id_prorudct)
    {
        if(s_id_prorudct==this.carrot.shop.get_id_by_index(0))
        {
            this.carrot.Show_msg(carrot.L("remove_ads", "remove_ads"), carrot.L("buy_success", "buy_success"), Carrot.Msg_Icon.Success);
            this.act_inapp_removeAds();
        }

        if (s_id_prorudct == this.carrot.shop.get_id_by_index(1))
        {
            this.carrot.Show_msg(carrot.L("shop", "Shop"),carrot.L("buy_success", "buy_success"), Carrot.Msg_Icon.Success);
            this.panel_music.act_download_mp3();
        }
    }

    private void onRestoreSuccessPayCarrot(string[] arr_id)
    {
        for(int i = 0; i < arr_id.Length; i++)
        {
            string s_id_prorudct = arr_id[i];
            if (s_id_prorudct == this.carrot.shop.get_id_by_index(0)) this.act_inapp_removeAds();
        }
    }

    private void act_inapp_removeAds()
    {
        PlayerPrefs.SetInt("is_buy_ads", 1);
        this.panel_setting_removeAds.SetActive(false);
    }

    public void restore_product()
    {
        this.carrot.shop.restore_product();
    }

    public void buy_product(int index_p)
    {
        this.carrot.shop.buy_product(index_p);
    }
}