using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodSpawn : MonoBehaviour
{
    public GameObject food, agent;
    public float food_maxcd, agent_maxcd, food_range;
    float food_curcd, agent_curcd;
    // Update is called once per frame
    void Update()
    {
        if(food_curcd < 0)
        {
            food_curcd = food_maxcd;
            for(int i = 0; i<8; i++)
            {
                Instantiate(food, food_range*Random.insideUnitCircle, Quaternion.identity);
            }
        }
        if(agent_curcd < 0)
        {
            agent_curcd = agent_maxcd;
            for(int i = 0; i<16; i++)
            {
                Instantiate(agent, food_range*Random.insideUnitCircle, Quaternion.identity);
            }
        }
        food_curcd -= Time.deltaTime;
        agent_curcd -= Time.deltaTime;
    }
}
