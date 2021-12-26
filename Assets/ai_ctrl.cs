using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class geneData
{
    public float maxhp, curhp, consume;
}

public class ai_ctrl : MonoBehaviour{
    public geneData d = new geneData();
    void Start(){
        d.maxhp = d.curhp = Random.Range(50, 100);
        d.consume = Random.Range(0.01f, 0.075f);
    }
    private void FixedUpdate() {
        if(d.curhp > 0)
        {
            transform.position += d.consume * (Vector3)Random.insideUnitCircle;
            d.curhp -= d.consume;
        }
        else
        {
            Destroy(gameObject, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        d.curhp += other.transform.localScale.x * 50f;
    }
}
