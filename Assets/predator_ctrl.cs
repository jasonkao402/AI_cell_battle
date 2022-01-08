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
    ml_ctrl tmp;
    geneData otherData;
    public geneData data = new geneData();
    Rigidbody2D rb;
    SpriteRenderer sr;
    Collider2D tcol;
    //RaycastHit2D[] ray2D = new RaycastHit2D[NumRay];
    Vector3 t;
    RayPerceptionSensorComponentBase sen;
    [SerializeField]
    //float[] vision = new float[NumRay];
    string ID;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sen = GetComponent<RayPerceptionSensorComponentBase>();
    }
    public override void Initialize()
    {
        data.maxhp = data.curhp = 1024;
        data.consume = 1;
        sen.RayLength = data.sensor;
        //sr.color = Random.ColorHSV(0,1,1,1,1,1,1,1);
        for(int i = 0; i<3; i++)
            ID += (char)('A'+Random.Range(0, 26));
        gameObject.name = ID;
    }

    public override void OnEpisodeBegin()
    {
        data.curhp = data.maxhp;
        transform.localPosition = utilFunc.RandSq(utilFunc.spawnRange);
    }
    // private void FixedUpdate() {
        
    //     for(int i = 0; i<NumRay; i++){
    //         t = Quaternion.AngleAxis(i * deg, Vector3.forward) * Vector2.right;
    //         Debug.DrawRay(transform.position + t*sightOs*transform.localScale.x, t * vision[i]);
    //     }
    // }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localScale.x);
        // for(int i = 0; i<NumRay; i++)
        // {
        //     t = Quaternion.AngleAxis(i * deg, Vector3.forward) * Vector2.right;
        //     vision[i] = Physics2D.Raycast(transform.position + t*sightOs*transform.localScale.x, t, 4).distance;
        //     sensor.AddObservation(vision[i]);
        // }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //rb.AddTorque(data.turn * actions.ContinuousActions[0]);
        rb.AddForce(data.consume * new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]) );
        transform.localScale = Vector3.one * (data.curhp+400f) * 0.004f;
        data.curhp -= data.consume * 0.25f;
        AddReward(-1f/MaxStep);
        if(data.curhp < 0){
            //starve
            EndEpisode();
        }
        else if(data.curhp > 5*data.maxhp){
            //split
            AddReward(data.curhp);
            data.curhp = data.maxhp;
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
        data.curhp += other.transform.localScale.x * 20f;
        AddReward(other.transform.localScale.x * 10f);
        Destroy(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("wall_tag"))
        {
            transform.localPosition = new Vector3(-transform.localPosition.x, -transform.localPosition.y, 0);
            AddReward(-1);
            //SetReward(0);
            //EndEpisode();
        }
        else
        {
            tmp = other.gameObject.GetComponent<ml_ctrl>();
            if(tmp != null)
            {
                //eat
                tmp.data.curhp = 0;
                data.curhp += tmp.data.curhp*0.5f;
                AddReward(tmp.data.curhp*0.5f);
            }
        }
    }
}
