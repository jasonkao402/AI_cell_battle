using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    Collider2D col;
    public GameObject tgt;
    public float panSpeed;
    bool focus;
    public float l;
    void FixedUpdate()
    {
        if(tgt)
            transform.position = Vector3.Lerp(transform.position, tgt.transform.position, l);
        if(!focus)
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))*panSpeed;
    }
    private void Update() {
        Camera.main.orthographicSize *= (-Input.GetAxisRaw("Mouse ScrollWheel")+1);
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, 50);
        if(Input.GetMouseButtonDown(0))
        {
            col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 2, 1|1<<8|1<<9|1<<10);
            if(col != null){
                tgt = col.gameObject;
                focus = true;
            }
            else{
                tgt = null;
                focus = false;
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            tgt = FindObjectOfType<predator_ctrl>().gameObject;
            focus = true;
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 0.333f;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1f;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2.5f;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 5f;
        }
    }
}
