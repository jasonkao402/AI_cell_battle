using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prey_ctrl : MonoBehaviour
{
    public static int population;
    public GameObject selfCopy;
    prey_ctrl tmp;
    geneData otherData;
    public geneData data = new geneData();
    Rigidbody2D rb;
    SpriteRenderer sr;
    Collider2D tcol, esccol;
    public int scanCD;
    int scan;
    string ID;
    private void Awake() {
        population = 0;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        for(int i = 0; i<2; i++)
            ID += (char)('A'+Random.Range(0, 26));
        gameObject.name = $"s_{ID}";
    }

    void OnRespawn()
    {
        data.curhp = data.maxhp*Random.Range(0.9f, 1);
        transform.localPosition = utilFunc.RandSq(utilFunc.spawnRange);
        transform.up = Random.insideUnitCircle;
    }
    void tryscan()
    {
        if(scan % scanCD == 0)
        {
            tcol = Physics2D.OverlapCircle(transform.position, data.sensor*transform.localScale.x, 1<<10);
            esccol = Physics2D.OverlapCircle(transform.position, data.sensor*transform.localScale.x, 1<<9);
        }
        scan++;
    }
    private void FixedUpdate() {
        float turn = 0, fwrd = 0;
        tryscan();
        if(tcol){
            turn += Vector3.Cross(tcol.transform.position-transform.position, transform.up).normalized.z;
            fwrd = 0.75f;
        }
        else if(esccol){
            turn -= Vector3.Cross(esccol.transform.position-transform.position, transform.up).normalized.z;
            fwrd = 1;
        }
        else{
            fwrd = 0.5f;
        }

        transform.rotation *= Quaternion.AngleAxis(data.turnRate*turn, Vector3.forward);
        rb.AddForce(data.consume * fwrd * transform.up);

        transform.localScale = Vector3.one * (data.curhp+2000f)/data.maxhp;
        data.curhp -= data.consume * 0.1f;
        if(data.curhp < 0){
            OnRespawn();
        }
        else if(data.curhp > 3*data.maxhp){
            //split
            Instantiate(selfCopy, transform.position, Quaternion.identity, transform.parent);
            population++;
            data.curhp = data.maxhp;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag("wall_tag"))
        {
            transform.localPosition = utilFunc.RandSq(60);
        }
        else if(other.gameObject.CompareTag("food_tag"))
        {
            data.curhp += other.transform.localScale.x * 150f;
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        float tmpf;
        tmp = other.gameObject.GetComponent<prey_ctrl>();
        if(tmp != null)
        {
            //share food
            otherData = tmp.data;
            tmpf = data.curhp+otherData.curhp;
            data.curhp = tmpf * 0.5f;
        }
    }
}
