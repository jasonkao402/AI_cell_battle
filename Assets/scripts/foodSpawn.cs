using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodSpawn : MonoBehaviour
{
    public GameObject food, prey, predator;
    public float food_maxcd, food_range;
    public int food_amt, prey_amt, predator_amt;
    float food_curcd;
    // Update is called once per frame
    private void Start() {
        for(int i = 0; i<prey_amt; i++)
        {
            ml_ctrl.population++;
            Instantiate(prey, transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
        }
        for(int i = 0; i<predator_amt; i++)
        {
            Instantiate(predator, transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
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
