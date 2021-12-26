using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class geneData
{
    public float maxhp, curhp, consume, randomSeed;
}

public class ai_ctrl : MonoBehaviour{
    ai_ctrl tmp;
    geneData otherData;
    public geneData data = new geneData();
    Rigidbody2D rb;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        data.randomSeed = Random.Range(-2048f, 2048f);
        data.maxhp = data.curhp = Random.Range(250, 1000);
        data.consume = Random.Range(0.1f, 1f);
    }
    private void FixedUpdate() {
        if(data.curhp > 0)
        {
            //transform.rotation *= Quaternion.AngleAxis(10f*(Mathf.PerlinNoise(data.randomSeed+Time.time, 0)-0.5f), Vector3.forward);
            rb.AddTorque(0.5f*(Mathf.PerlinNoise(data.randomSeed+Time.time, 0)-0.5f));
            rb.AddForce(transform.right * data.consume);// += transform.right;
            transform.localScale = Vector3.one * (data.curhp+200f) * 0.002f;
            data.curhp -= data.consume;
        }
        else
        {
            Destroy(gameObject, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject.name);
        data.curhp += other.transform.localScale.x * 50f;
        Destroy(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        tmp = other.gameObject.GetComponent<ai_ctrl>();
        if(tmp != null)
        {
            otherData = tmp.data;
            if(data.curhp >  otherData.curhp)
            {
                data.curhp += otherData.curhp*0.2f;
                Destroy(other.gameObject);
            }
            else
            {
                otherData.curhp += data.curhp*0.2f;
                Destroy(gameObject);
            }
        }
    }
}
