using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodSpawn : MonoBehaviour
{
    public GameObject food, agent;
    public float food_maxcd, food_range;
    public int food_amt, agent_amt;
    float food_curcd;
    // Update is called once per frame
    private void Start() {
        for(int i = 0; i<agent_amt; i++)
        {
            Instantiate(agent, transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
        }
    }
    void Update()
    {
        if(food_curcd < 0)
        {
            food_curcd = food_maxcd;
            for(int i = 0; i<food_amt; i++)
            {
                Instantiate(food, transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
            }
        }
        food_curcd -= Time.deltaTime;
    }
}
