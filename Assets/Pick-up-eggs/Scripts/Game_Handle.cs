using System.Collections.Generic;
using Carrot;
using UnityEngine;
using UnityEngine.UI;

public class Game_Handle : MonoBehaviour
{
    public Carrot.Carrot carrot;
    [Header("Game Obj")]
    public BK_handle bk;
    public Camera cam;
    public IronSourceAds ads;
    private int highest_score;

    [Header("Game UI")]
    public Text txt_your_highest_score;
    public GameObject panel_menu;
    public GameObject panel_play;
    public GameObject panel_gameover;

    [Header("UI GameOver")]
    public Text txt_gameover_scores;
    public Text txt_highest_score;
    public Text txt_gameover_eggs_white;
    public Text txt_gameover_eggs_gold;
    public Text txt_gameover_shit;
    public Text txt_gameover_timer;

    [Header("Sounds")]
    public AudioSource[] sound;

    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_game);
        this.ads.On_Load();
        this.panel_menu.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_gameover.SetActive(false);
        this.highest_score = PlayerPrefs.GetInt("highest_score", 0);
        this.txt_your_highest_score.text = "Your highest score : "+this.highest_score.ToString();
        this.carrot.game.load_bk_music(this.sound[6]);
        this.carrot.game.act_click_watch_ads_in_music_bk=this.ads.ShowRewardedVideo;
        this.carrot.act_buy_ads_success=this.ads.RemoveAds;
        this.ads.onRewardedSuccess=this.carrot.game.OnRewardedSuccess;
    }

    public void btn_play_now()
    {
        this.cam.GetComponent<Animator>().enabled = false;
        this.cam.fieldOfView = 68;
        this.carrot.play_sound_click();
        this.bk.reset();
        this.bk.play();
        this.panel_menu.SetActive(false);
        this.panel_play.SetActive(true);
        this.panel_gameover.SetActive(false);
    }

    public void check_exit_game()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_menu();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_back_menu()
    {
        this.ads.show_ads_Interstitial();
        this.cam.GetComponent<Animator>().enabled = true;
        this.carrot.play_sound_click();
        this.bk.stop();
        this.panel_menu.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_gameover.SetActive(false);
    }

    public void play_sound(int index_sound)
    {
        if(this.carrot.get_status_sound())this.sound[index_sound].Play();
    }

    public void btn_show_setting()
    {
        this.ads.show_ads_Interstitial();
        this.bk.stop();
        Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(this.act_after_close_setting);
    }

    private void act_after_close_setting(List<string> items_change)
    {
        if (this.panel_play.activeInHierarchy) this.bk.play();
    }

    public void show_gameover()
    {
        int your_scores = this.bk.get_scores();
        if (your_scores > this.highest_score)
        {
            this.highest_score = your_scores;
            PlayerPrefs.SetInt("highest_score", this.highest_score);
        }
        this.carrot.play_vibrate();
        this.play_sound(4);
        this.bk.stop();
        this.panel_gameover.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_menu.SetActive(false);
        this.txt_gameover_eggs_gold.text = this.bk.get_eggs_gold().ToString();
        this.txt_gameover_eggs_white.text = this.bk.get_eggs_white().ToString();
        this.txt_gameover_shit.text = this.bk.get_eggs_shit().ToString();
        this.txt_gameover_timer.text = this.FormatTime(this.bk.get_timer_game()).ToString();
        this.txt_highest_score.text = this.highest_score.ToString();
        this.txt_gameover_scores.text = your_scores.ToString();
        this.carrot.game.update_scores_player(your_scores);
        this.txt_your_highest_score.text = "Your highest score : " + this.highest_score.ToString();
        this.ads.show_ads_Interstitial();
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
    }

    public void btn_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_share()
    {
        this.carrot.show_share();
    }

    public void btn_login()
    {
        this.carrot.user.show_login();
    }

    public void btn_top_player()
    {
        this.ads.show_ads_Interstitial();
        this.carrot.game.Show_List_Top_player();
    }
}
