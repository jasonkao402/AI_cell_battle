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
    }
    private void Update() {
        Camera.main.orthographicSize += -3f * Input.GetAxisRaw("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 20);
        if(Input.GetMouseButtonDown(0))
        {
            col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1, 1|1<<8|1<<9|1<<10);
            if(col != null) tgt = col.gameObject;
        }
        else if(Input.GetMouseButtonDown(1))
        {
            tgt = FindObjectOfType<predator_ctrl>().gameObject;
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log(ml_ctrl.population);
        }
    }
}
