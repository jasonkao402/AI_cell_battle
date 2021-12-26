using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    Collider2D col;
    public GameObject tgt;
    public float l;
    void FixedUpdate()
    {
        if(tgt)
            transform.position = Vector3.Lerp(transform.position, tgt.transform.position, l);
        else{
            tgt = FindObjectOfType<ai_ctrl>().gameObject;
        }
    }
    private void Update() {
        Camera.main.orthographicSize += Input.GetAxisRaw("Mouse ScrollWheel");
        if(Input.GetMouseButtonDown(0))
        {
            col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 2);
            if(col != null) tgt = col.gameObject;
        }
    }
}
