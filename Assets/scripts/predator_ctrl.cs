using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class predator_ctrl : Agent
{
    // Start is called before the first frame update
    const int deg = 15;
    public GameObject selfCopy, killeff;
    prey_ctrl tmp_p;
    public geneData data = new geneData();
    Rigidbody2D rb;
    SpriteRenderer sr;
    Collider2D tcol;
    RayPerceptionSensorComponentBase[] sen = new RayPerceptionSensorComponentBase[2];
    string ID;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sen = GetComponents<RayPerceptionSensorComponentBase>();
        for(int i = 0; i<2; i++)
            ID += (char)('A'+Random.Range(0, 26));
        gameObject.name = $"w_{ID}";
    }
    public override void Initialize()
    {
        sen[0].RayLength = data.sensor;
        sen[1].RayLength = data.sensor;
    }

    public override void OnEpisodeBegin()
    {
        data.curhp = data.maxhp;
        transform.localPosition = utilFunc.RandSq(utilFunc.spawnRange);
        transform.up = Random.insideUnitCircle;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(rb.velocity.normalized);
        sensor.AddObservation(transform.localScale.x);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //rb.AddTorque(data.turn * actions.ContinuousActions[0]);
        //rb.AddForce(data.consume * new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]) );
        transform.rotation *= Quaternion.AngleAxis(data.turnRate*actions.ContinuousActions[0], Vector3.forward);
        rb.AddForce(data.consume * actions.ContinuousActions[1] * transform.up);

        transform.localScale = Vector3.one * (data.curhp+3000f)/data.maxhp;
        data.curhp -= data.consume * 0.02f;
        AddReward(-1f/MaxStep);
        if(data.curhp < 0){
            //starve
            EndEpisode();
        }
        else if(data.curhp > 10f*data.maxhp){
            //split
            Instantiate(selfCopy, transform.position, Quaternion.identity, transform.parent);
            AddReward(data.curhp);
            data.curhp = data.maxhp;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> cActions = actionsOut.ContinuousActions;
        tcol = ClosestCollider(
            transform.position, 
            Physics2D.OverlapCircleAll(transform.position, data.sensor*transform.localScale.x, 1<<8)
            );
        if(!tcol)
        {
            cActions[0] = 0;
            cActions[1] = 0.333f;
        }
        else
        {
            cActions[0] = Vector3.Cross(tcol.transform.position-transform.position, transform.up).normalized.z;
            cActions[1] = 1;
        }
        //cActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("wall_tag"))
        {
            transform.localPosition = utilFunc.RandSq(60);
            AddReward(-10);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        tmp_p = other.gameObject.GetComponent<prey_ctrl>();
        if(tmp_p != null)
        {
            //eat
            data.curhp += tmp_p.data.curhp*0.17f;
            AddReward(tmp_p.data.curhp*0.17f);
            tmp_p.data.curhp = 0;
            Instantiate(killeff, transform.position, Quaternion.identity);
        }
    }
    Collider2D ClosestCollider(Vector3 unitPosition, Collider2D[] tgtColliders)
    {
        float bestdstc = 999999.0f, tmpdstc;
        Collider2D bestCollider = null;

        foreach (Collider2D tgt in tgtColliders)
        {
            tmpdstc = Vector3.SqrMagnitude(unitPosition - tgt.transform.position);
            if (tmpdstc < bestdstc)
            {
                bestdstc = tmpdstc;
                bestCollider = tgt;
            }
        }
        return bestCollider;
    } 
}
