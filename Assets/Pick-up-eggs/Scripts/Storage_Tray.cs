using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Storage_Tray : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerMoveHandler
{

    public Vector3 tray_size_small;
    public Vector3 tray_size_lager;
    public Vector3 tray_mouse_pos;
    public Transform Obj_tray_eggs;
    public float speed_move = 1f;
    public BK_handle bk;
    private bool is_move = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.bk.hide_tip();
        this.is_move = true;
        this.Obj_tray_eggs.localScale = this.tray_size_lager;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (this.is_move)
        {
            this.tray_mouse_pos = eventData.position;
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, this.Obj_tray_eggs.position.y, Camera.main.nearClipPlane));
            this.Obj_tray_eggs.position = new Vector3(point.x * this.speed_move, this.Obj_tray_eggs.position.y, this.Obj_tray_eggs.position.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.is_move = false;
        this.Obj_tray_eggs.localScale = this.tray_size_small;

    }

    public Vector3 get_pos_mouse()
    {
        return this.tray_mouse_pos;
    }

}
