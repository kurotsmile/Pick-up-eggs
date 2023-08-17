using UnityEngine;

public class Eggs : MonoBehaviour
{
    public int scores=1;
    public item_eggs_type type;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name== "Storage_Tray")
        {
            GameObject.Find("Game").GetComponent<Game_Handle>().bk.add_scores(this);
        }
        else
        {
            if(this.type==item_eggs_type.shit)
                GameObject.Find("Game").GetComponent<Game_Handle>().bk.create_eggs_broken(this.transform.position,1);
            else
                GameObject.Find("Game").GetComponent<Game_Handle>().bk.create_eggs_broken(this.transform.position,0);
        }
        Destroy(this.gameObject);
    }
}
