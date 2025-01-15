using UnityEngine;

public class Chicken : MonoBehaviour
{
    public Transform pos_create_eggs;
    public Animator anim;

    public void Lay_egg(){
        this.anim.Play("Run In Place");
    }

}
