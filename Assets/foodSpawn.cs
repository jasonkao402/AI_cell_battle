using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodSpawn : MonoBehaviour
{
    public GameObject food;
    public float maxcd, food_range;
    float curcd;
    // Update is called once per frame
    void Update()
    {
        if(curcd < 0)
        {
            curcd = maxcd;
            for(int i = 0; i<8; i++)
            {
                Instantiate(food, food_range*Random.insideUnitCircle, Quaternion.identity);
            }
        }
        curcd -= Time.deltaTime;
    }
}
