using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class envMaid : MonoBehaviour
{
    public GameObject food, prey, predator;
    StreamWriter psr, wsr;
    objPool pooli;
    public float food_maxcd, food_range, foodValue;
    public int food_init, prey_init, predator_init;
    public int food_pop, prey_pop, predator_pop;
    float food_curcd;
    // Update is called once per frame
    private void Start() {
        string ID = "";
        for(int i = 0; i<3; i++)
            ID += (char)('A'+Random.Range(0, 26));
        psr = File.CreateText($"output_p_{ID}.txt");
        wsr = File.CreateText($"output_w_{ID}.txt");

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
            psr.Write($"{prey_pop} ");
            wsr.Write($"{predator_pop} ");
        }
        food_curcd -= Time.deltaTime;
    }
    private void OnApplicationQuit() {
        psr.Close();
        wsr.Close();
    }
}
