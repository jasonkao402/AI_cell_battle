using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ml_ctrl : Agent
{
    // Start is called before the first frame update
    public static int population;
    const int deg = 15; 
    public GameObject selfCopy;
    ml_ctrl tmp;
    geneData otherData;
    public geneData data = new geneData();
    Rigidbody2D rb;
    SpriteRenderer sr;
    Collider2D tcol, esccol;
    //RaycastHit2D[] ray2D = new RaycastHit2D[NumRay];
    Vector3 t;
    RayPerceptionSensorComponentBase sen;
    [SerializeField]
    //float[] vision = new float[NumRay];
    string ID;
    private void Awake() {
        population = 0;
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

        transform.localScale = Vector3.one * (data.curhp+1000f)/data.maxhp;
        data.curhp -= data.consume * 0.1f;
        AddReward(-1f/MaxStep);
        if(data.curhp < 0){
            //starve
            EndEpisode();
        }
        else if(data.curhp > 3*data.maxhp){
            //split
            Instantiate(selfCopy, transform.position, Quaternion.identity, transform.parent);
            population++;
            AddReward(data.curhp);
            data.curhp = data.maxhp;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> cActions = actionsOut.ContinuousActions;
        float turn = 0;
        tcol = Physics2D.OverlapCircle(transform.position, data.sensor*transform.localScale.x, 1<<10);
        esccol = Physics2D.OverlapCircle(transform.position, data.sensor*transform.localScale.x, 1<<9);
        if(tcol){
            turn += Vector3.Cross(tcol.transform.position-transform.position, transform.up).normalized.z;
            cActions[1] = 0.75f;
        }
        else if(esccol){
            turn -= Vector3.Cross(esccol.transform.position-transform.position, transform.up).normalized.z;
            cActions[1] = 1;
        }
        else
        {
            cActions[1] = 0.5f;
        }
        cActions[0] = turn;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag("wall_tag"))
        {
            transform.localPosition = utilFunc.RandSq(60);
            AddReward(-10);
        }
        else if(other.gameObject.CompareTag("food_tag"))
        {
            data.curhp += other.transform.localScale.x * 150f;
            AddReward(10);
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        tmp = other.gameObject.GetComponent<ml_ctrl>();
        if(tmp != null)
        {
            //share food
            otherData = tmp.data;
            data.curhp = (data.curhp+otherData.curhp)*0.5f;
            AddReward((data.curhp+otherData.curhp)*0.1f);
        }
    }
}
