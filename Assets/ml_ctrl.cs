using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ml_ctrl : Agent
{
    // Start is called before the first frame update
    ml_ctrl tmp;
    geneData otherData;
    public geneData data = new geneData();
    Rigidbody2D rb;
    Collider2D tcol;
    void Start()
    {
        
    }
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        data.maxhp = data.curhp = Random.Range(1500f, 2000f);
        data.consume = Random.Range(1f, 1.5f);
    }
    public override void OnEpisodeBegin()
    {
        data.curhp = data.maxhp;
        transform.localPosition = 7.5f*Random.insideUnitCircle;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localScale.x);
        tcol = Physics2D.OverlapCircle(transform.position, 4);
        if(tcol) {
            //sensor.AddObservation(true);
            sensor.AddObservation(tcol.transform.localPosition);
            sensor.AddObservation(tcol.transform.localScale.x);
        }
        else{
            //sensor.AddObservation(false);
            sensor.AddObservation(Vector3.forward);
            sensor.AddObservation(0);
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //rb.AddTorque(data.turn * actions.ContinuousActions[0]);
        rb.AddForce(data.consume * new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]) );
        transform.localScale = Vector3.one * (data.curhp+300f) * 0.0015f;
        data.curhp -= data.consume;
        if(data.curhp < 0)
            EndEpisode();
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
            if(data.curhp >  otherData.curhp)
            {
                data.curhp += otherData.curhp*0.2f;
                AddReward(otherData.curhp);
                //Destroy(other.gameObject);
            }
            else
            {
                otherData.curhp += data.curhp*0.2f;
                AddReward(-0.333f*data.curhp);
                EndEpisode();
                //Destroy(gameObject);
            }
        }
    }
}
