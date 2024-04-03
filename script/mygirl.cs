using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using Carrot;

public class mygirl : MonoBehaviour
{
    [Header("Main Obj")]
    public Carrot.Carrot carrot;

    [Header("Panel App")]
    public GameObject panel_button;
    public GameObject panel_msg_func;
    public GameObject panel_msg_menu;
    public GameObject panel_fnc_music;
    public GameObject panel_btn_main;
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
    private float count_time_next_chat = 0f;
    private float count_byebye = 0f;
    private int count_ads = 0;
    private int count_ads_2 = 0;
    private bool byebye = false;
    private int sex = 0;
    public Text txt_show_inp_chat;
    private string id_question = "";
    private string str_effect;

    [Header("Object Game")]
    public GameObject Magic_tocuh;
    public Skybox bk;
    public Person person;
    public GameObject area_effect;


    [Header("Object Default")]
    public Texture2D bk_default;
    public Sprite avatar_default;
    public Sprite[] icon;
    public GameObject prefab_effect_customer;
    public GameObject[] effect_temp;


    private bool is_waiting_voice = false;
    public Image icon_avatar;

    public string parameter_link = "";

    void Start()
    {
        this.set_sex(PlayerPrefs.GetInt("sex", 0));
        this.person.load_status(PlayerPrefs.GetInt("sex", 0));
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;

        this.carrot.Load_Carrot(check_ext_app);
        this.carrot.act_after_close_all_box = this.act_after_close_all_box;
        
        this.GetComponent<Brain>().check();
        this.GetComponent<Music_offline>().check();

        this.load_background();

        this.panel_button.SetActive(true);
        this.panel_chat_me.SetActive(false);
        this.panel_music.gameObject.SetActive(false);
        this.panel_msg_func.SetActive(false);
        this.panel_msg_menu.SetActive(false);
        this.panel_fnc_music.SetActive(false);
        this.icon_loading.SetActive(false);
        this.panel_btn_main.SetActive(true);
    }

    private void check_ext_app()
    {
        if (this.panel_msg_func.activeInHierarchy)
        {
            this.panel_msg_func.GetComponent<Panel_msg_box_func>().btn_close();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_msg_menu.activeInHierarchy)
        {
            this.panel_msg_menu.SetActive(false);
            this.carrot.set_no_check_exit_app();
        }
    }

    public void load_app_online()
    {
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
        this.txt_girl_chat.text = carrot.L("lost_connection_msg", "Lost connection! please check the network connection or 3g, wifi");
    }

    private void act_after_close_all_box()
    {
        this.show_magic_touch(true);
    }

    public void act_no_magic_touch()
    {
        this.carrot.delay_function(1f, no_magic_touch);
    }

    private void no_magic_touch() { this.show_magic_touch(false);}


    private void load_msg_start(string s_data)
    {
        StructuredQuery q = new StructuredQuery("chat-" + carrot.lang.Get_key_lang());
        q.Add_where("key", Query_OP.EQUAL,"hi_"+DateTime.Now.Hour);
        q.Set_limit(1);
        carrot.server.Get_doc(q.ToJson(), Act_get_chat_done, Act_server_fail);
    }

    private void Act_get_chat_done(string s_data)
    {
        Fire_Collection fc = new(s_data);
        if (!fc.is_null)
        {
            IDictionary data_chat = fc.fire_document[0].Get_IDictionary();
            this.act_chat_girl(data_chat);
        }
    }

    public void Act_server_fail(string s_error)
    {
        carrot.Show_msg("Error", s_error, Msg_Icon.Error);
        carrot.play_vibrate();
    }

    public void check_show_ads()
    {
        this.show_ads();
    }

    public void show_ads()
    {
        this.carrot.ads.show_ads_Interstitial();
    }

    public void act_hit()
    {
        this.count_time_next_chat = 0;
        StructuredQuery q = new("chat-" + carrot.lang.Get_key_lang());
        q.Add_where("key", Query_OP.EQUAL, "hit");
        q.Set_limit(1);
        carrot.server.Get_doc(q.ToJson(),Act_get_chat_done,Act_server_fail);
    }

    void Update()
    {
        if (
            this.str_effect != "2" &&
            this.panel_msg_func.activeInHierarchy == false &&
            this.panel_msg_menu.activeInHierarchy == false&&
            this.carrot.is_online()
            )
        {
            if (this.speech.isPlaying == false)
            {
                this.count_time_next_chat += 1f * Time.deltaTime;
                if (this.count_time_next_chat > 23f)
                {
                    //WWWForm frm_chat = this.frm_action("bat_chuyen");
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

            if (carrot.get_status_sound())
            {
                if (this.panel_recoding.panel_voice.activeInHierarchy)
                {
                    if (this.speech.isPlaying == false && this.panel_recoding.auto_chat == true && this.is_waiting_voice == true)
                    {
                        this.is_waiting_voice = false;
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

    public void play_s()
    {
        if (this.panel_msg_func.activeInHierarchy == true)
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
                        //WWWForm frm_chat = this.frm_action("chat");
                        //frm_chat.AddField("func_server", this.func_server);
                        //frm_chat.AddField("text", this.inpText.text.ToLower());
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

    public void act_chat_girl(IDictionary chat)
    {
        this.txt_girl_chat.text = chat["msg"].ToString().ToLower();
        this.chat_box.set_show_text();

        this.person.set_status(chat["status"].ToString());

        if (chat["color"]!=null)
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

        string s_chat = chat["msg"].ToString().ToLower();

        this.id_question = chat["id"].ToString();

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
            Application.OpenURL(chat["link"].ToString());
        }

        if (chat["vibrate"] != null)
        {
            if (chat["vibrate"].ToString() != "") carrot.play_vibrate();
        }
 
        if (chat["icon"].ToString() != "")
        {
            StartCoroutine(downloadEffectCustomer(chat["icon"].ToString()));
        }

        if (carrot.get_status_sound())
        {
            if (chat["mp3"] != null)
            {
                if (chat["mp3"].ToString() != "")
                {
                    string url_voice_mp3;
                    if (chat["mp3"].ToString() == "google")
                        url_voice_mp3 = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=" + s_chat.Length + "&client=tw-ob&q=" + s_chat + "&tl=" + chat["lang"].ToString();
                    else
                        url_voice_mp3 = chat["mp3"].ToString();
                    StartCoroutine(downloadAudio(url_voice_mp3, AudioType.MPEG));
                }
            }
        }

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

        if (effect_str == "8")
        {

        }
        //tac den pin
        if (effect_str == "9")
        {

        }
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

    public void set_sex(int intsex)
    {
        if (intsex == 0)
        {
            this.sex = 0;
        }
        else
        {
            this.sex = 1;
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

    public void setting_reset_all_data()
    {
        this.GetComponent<Music_offline>().delete_all();
        this.carrot.get_tool().delete_file("bk.png");
        this.set_skybox_Texture(this.bk_default);
        this.load_app_online();
    }

    public void Btn_show_setting()
    {
        this.Magic_tocuh.SetActive(false);
        carrot.Create_Setting();
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
        }

        if (is_bool == true)
        {
            if (this.panel_music.is_offline == true && this.panel_music.is_radio == false)
            {
                this.panel_btn_main.SetActive(false);
            }
        }
    }

    public void set_fnc_music(bool is_bool)
    {
        this.panel_fnc_music.SetActive(is_bool);
    }

    public void view_url_copyright()
    {
        Application.OpenURL(carrot.L("setting_copyright_url", "http://carrotstore.com/privacy_policy"));
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
        this.panel_msg_menu.SetActive(false);
    }
    public void show_learn_question_customer(string msg_chat)
    {
        this.Magic_tocuh.SetActive(false);
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

    }

    public void show_chat_by_id_lang(string id, string lang)
    {
        this.show_btn_main(false);
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
            this.button_login_in_home.SetActive(false);
        }
        else
        {
            this.panel_btn_main.SetActive(true);
            this.panel_msg_menu.SetActive(false);
            if(this.carrot.is_online())this.button_login_in_home.SetActive(true);
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

}