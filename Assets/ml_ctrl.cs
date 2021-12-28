using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ml_ctrl : Agent
{
    // Start is called before the first frame update
    const int NumRay = 24, deg = 15; 
    ml_ctrl tmp;
    geneData otherData;
    public geneData data = new geneData();
    Rigidbody2D rb;
    SpriteRenderer sr;
    Collider2D tcol;
    RaycastHit2D[] ray2D = new RaycastHit2D[NumRay];
    string ID;
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        data.maxhp = data.curhp = Random.Range(500f, 1000f);
        data.cur_killCD = data.max_killCD;
        data.consume = Random.Range(1f, 1.5f);
        sr.color = Random.ColorHSV(0,1,1,1,1,1,1,1);
        for(int i = 0; i<4; i++)
            ID += (char)('A'+Random.Range(0, 26));
        gameObject.name = ID;
    }

    public override void OnEpisodeBegin()
    {
        data.curhp = data.maxhp;
        transform.localPosition = 7.5f*Random.insideUnitCircle;
    }
    private void FixedUpdate() {
        for(int i = 0; i<NumRay; i++)
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(i * deg, Vector3.forward) * Vector2.right);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localScale.x);
        for(int i = 0; i<NumRay; i++)
        {
            ray2D[i] = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(i * deg, Vector3.forward) * Vector2.right);
            sensor.AddObservation(Mathf.Min(3f, ray2D[i].distance));
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //rb.AddTorque(data.turn * actions.ContinuousActions[0]);
        rb.AddForce(data.consume * new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]) );
        transform.localScale = Vector3.one * (data.curhp+500f) * 0.002f;
        data.curhp -= data.consume;
        data.cur_killCD -= 1;
        if(data.curhp < 0){
            //starve
            AddReward(-data.maxhp/data.consume);
            EndEpisode();
        }
        else if(data.curhp > 3*data.maxhp){
            //split
            data.cur_killCD = data.max_killCD;
            AddReward(data.curhp);
            data.curhp = data.maxhp;
            Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent).gameObject.name+=ID;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> cActions = actionsOut.ContinuousActions;
        cActions[0] = Input.GetAxisRaw("Horizontal");
        cActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject.name);
        data.curhp += other.transform.localScale.x * 200f;
        AddReward(other.transform.localScale.x * 200f);
        Destroy(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        tmp = other.gameObject.GetComponent<ml_ctrl>();
        if(tmp != null)
        {
            otherData = tmp.data;
            if(data.curhp >  otherData.curhp && data.cur_killCD < 0)
            {
                data.cur_killCD = data.max_killCD;
                data.curhp += otherData.curhp*0.2f;
                AddReward(otherData.curhp);
                //Destroy(other.gameObject);
            }
            else if(otherData.cur_killCD < 0)
            {
                otherData.curhp += data.curhp*0.2f;
                SetReward(0);
                EndEpisode();
                //Destroy(gameObject);
            }
        }
    }
}
