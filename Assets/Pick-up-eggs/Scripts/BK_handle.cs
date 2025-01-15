using UnityEngine;
using UnityEngine.UI;

public enum item_eggs_type {eggs_white,eggs_gold,shit}

public class BK_handle : MonoBehaviour
{
    public Game_Handle game;
    public Storage_Tray tray;
    public Chicken[] chicken;
    public GameObject []eggs_prefab;
    public GameObject []eggs_broken_prefab;
    public Transform Tr_area_effect_msg;
    public GameObject effect_msg_prefab;

    public Sprite sp_icon_eggs_life;
    public Sprite sp_icon_eggs_die;

    [Header("UI Game Play")]
    public Image[] img_icon_life;
    public Text txt_play_scores;
    public Text txt_play_egg_white;
    public Text txt_play_egg_gold;
    public GameObject panel_tap_tip;
    public GameObject panel_info_eggs;

    private int life = 5;
    private bool is_play = false;
    private int game_scores = 0;
    private int game_eggs_white = 0;
    private int game_eggs_gold= 0;
    private int game_eggs_shit= 0;
    float timer_create_eggs = 0f;
    float speed_create = 1f;
    float timer_game = 0f;

    public void reset()
    {
        this.panel_tap_tip.SetActive(true);
        this.panel_info_eggs.SetActive(false);
        for (int i = 0; i < this.chicken.Length; i++) this.chicken[i].anim.Play("Idle");
        this.timer_create_eggs = 0;
        this.speed_create = 1f;
        this.game_scores = 0;
        this.game_eggs_gold = 0;
        this.game_eggs_white = 0;
        this.game_eggs_shit = 0;
        this.timer_game = 0;
        this.txt_play_scores.text = "0";
        this.txt_play_egg_white.text = "0";
        this.txt_play_egg_gold.text = "0";
        this.life = 4;
        this.update_status_life();
    }

    public void hide_tip()
    {
        this.panel_tap_tip.SetActive(false);
        this.panel_info_eggs.SetActive(true);
    }

    public void play()
    {
        this.is_play = true;
    }

    public void stop()
    {
        this.is_play = false;
    }

    public void create_eggs_broken(Vector3 pos,int type=0)
    {
        if (type == 0)
        {
            this.subtraction_life();
            this.game.play_sound(3);
        }
        else
        {
            this.game.play_sound(5);
        }
        GameObject eggs_broken_obj = Instantiate(this.eggs_broken_prefab[type]);
        eggs_broken_obj.transform.SetParent(this.transform);
        eggs_broken_obj.transform.position = new Vector3(Random.Range(pos.x-0.1f, pos.x+ 0.1f), 0f, Random.Range(pos.z - 0.1f, pos.z + 0.1f));
        Destroy(eggs_broken_obj, 10f);
    }

    private void create_random_eggs()
    {
        this.game.play_sound(Random.Range(0,2));
        int index_chicken = Random.Range(0, this.chicken.Length);
        var index_c = index_chicken;
        this.chicken[index_chicken].Lay_egg();
        this.game.carrot.delay_function(0.5f,()=>act_Lay_egg(index_c));
    }

    private void act_Lay_egg(int index_chicken)
    {
        int rand_eggs = Random.Range(0, this.eggs_prefab.Length);
        GameObject eggs_obj = Instantiate(this.eggs_prefab[rand_eggs]);
        eggs_obj.transform.SetParent(this.transform);
        eggs_obj.transform.position = this.chicken[index_chicken].pos_create_eggs.position;
        Destroy(eggs_obj, 2f);
    }

    private void create_effect_msg(string s_msg,Vector3 pos,item_eggs_type type)
    {
        GameObject msg_obj = Instantiate(this.effect_msg_prefab);
        msg_obj.transform.SetParent(this.Tr_area_effect_msg);
        msg_obj.transform.position = new Vector3(pos.x,pos.y+220f,pos.z);
        msg_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        msg_obj.GetComponent<Effect_Msg>().txt.text = s_msg;
        msg_obj.GetComponent<Effect_Msg>().play_anim_type(type);
        Destroy(msg_obj, 2f);
    }

    void Update()
    {
        if (this.is_play)
        {
            this.timer_game += 1f * Time.deltaTime;
            this.timer_create_eggs += this.speed_create * Time.deltaTime;
            if (this.timer_create_eggs > 03f)
            {
                this.speed_create += 0.05f;
                this.create_random_eggs();
                this.timer_create_eggs = 0;
            }
        }
    }

    public void subtraction_life()
    {
        if (this.is_play)
        {
            this.life -= 1;
            this.update_status_life();
            if (this.life < 0)
            {
                this.is_play = false;
                this.game.panel_play.SetActive(false);
                this.game.carrot.delay_function(1f, this.game.show_gameover);
            }
            else
            {
                this.game.carrot.play_vibrate();
            }
        }
    }

    private void update_status_life()
    {
        for(int i = 0; i < this.img_icon_life.Length; i++)
        {
            if(i<=this.life)
                this.img_icon_life[i].sprite = this.sp_icon_eggs_die;
            else
                this.img_icon_life[i].sprite = this.sp_icon_eggs_life;

        }
    }

    public void add_scores(Eggs eggs)
    {
        string s_msg;
        if (eggs.type == item_eggs_type.eggs_white) this.game_eggs_white++;
        if (eggs.type == item_eggs_type.eggs_gold) this.game_eggs_gold++;
        if (eggs.type == item_eggs_type.shit)
        {
            this.game.play_sound(5);
            this.game_scores -= eggs.scores;
            this.game_eggs_shit++;
            s_msg="-"+ eggs.scores;
        }
        else
        {
            this.game.play_sound(2);
            this.game_scores += eggs.scores;
            s_msg = "+" + eggs.scores;
        }
        this.create_effect_msg(s_msg, this.tray.get_pos_mouse(),eggs.type);
        this.txt_play_scores.text = this.game_scores.ToString();
        this.txt_play_egg_gold.text = this.game_eggs_gold.ToString();
        this.txt_play_egg_white.text = this.game_eggs_white.ToString();
    }

    public int get_scores()
    {
        return this.game_scores;
    }

    public int get_eggs_gold()
    {
        return this.game_eggs_gold;
    }

    public int get_eggs_white()
    {
        return this.game_eggs_white;
    }

    public int get_eggs_shit()
    {
        return this.game_eggs_shit;
    }

    public float get_timer_game()
    {
        return this.timer_game;
    }
}
