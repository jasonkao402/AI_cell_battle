using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class envMaid : MonoBehaviour
{
    public GameObject food, prey, predator;
    objPool pooli;
    public float food_maxcd, food_range, foodValue;
    public int food_init, prey_init, predator_init;
    public int food_pop, prey_pop, predator_pop;
    float food_curcd;
    // Update is called once per frame
    private void Start() {
        
        pooli = objPool.Instance;
        predator_pop = predator_init;
        prey_pop = prey_init;
        for(int i = 0; i<prey_init; i++)
        {
            pooli.TakePool("prey", transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
        }
        for(int i = 0; i<predator_init; i++)
        {   
            pooli.TakePool("wolf", transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
        }
    }
    void Update()
    {
        if(food_curcd < 0)
        {
            food_pop += food_init;
            food_curcd = food_maxcd;
            for(int i = 0; i<food_init; i++)
            {
                pooli.TakePool("food", transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
            }
        }
        food_curcd -= Time.deltaTime;
    }
}
