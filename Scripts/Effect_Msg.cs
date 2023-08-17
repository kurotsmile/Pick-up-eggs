using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect_Msg : MonoBehaviour
{
    public Text txt;
    public Animator anim;

    public void play_anim_type(item_eggs_type type)
    {
        if (type == item_eggs_type.eggs_gold)
            this.anim.Play("msg_eggs_gold");
        else if(type==item_eggs_type.eggs_white)
            this.anim.Play("msg");
        else
            this.anim.Play("msg_eggs_shit");
    }

    public void done_effect()
    {
        Destroy(this.gameObject);
    }
}
