using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class envMaid : MonoBehaviour
{
    public GameObject food, prey, predator;
    public Gradient colorg;
    public SpriteRenderer sr;
    StreamWriter psr, wsr;
    objPool pooli;

    [Header("season & food")]
    public int food_amt;
    public float food_maxcd;
    public float food_range;
    public float foodBase;
    public float foodValue;
    public float season_intv;
    public bool enableSeason;

    [Header("Animal Limits")]
    public int predator_init;
    public int prey_init;

    [Header("population")]
    public int predator_pop;
    public int prey_pop;

    [Header("record result")]
    public bool isRecord;
    public int scan_max;
    int scan_cur, i;
    float food_curcd, season_now;
    string ID = "";
    private void Start() {
        pooli = objPool.Instance;
        predator_pop = predator_init;
        prey_pop = prey_init;
        foodValue = foodBase;
        if(isRecord)
        {
            for(i = 0; i<3; i++)
                ID += (char)('A'+Random.Range(0, 26));
            psr = File.CreateText($"venv/output_p_{ID}.txt");
            wsr = File.CreateText($"venv/output_w_{ID}.txt");
        }
        for(i = 0; i<prey_init; i++)
        {
            pooli.TakePool("prey", transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
        }
        for(i = 0; i<predator_init; i++)
        {   
            pooli.TakePool("wolf", transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
        }
    }
    void Update()
    {
        if(food_curcd < 0){
            food_curcd = food_maxcd;
            if(enableSeason)
            {
                season_now += 1f/season_intv;
                foodValue = (Mathf.Sin(season_now)*0.15f+1f)*foodBase;
                sr.color = colorg.Evaluate(Mathf.Sin(season_now)*0.5f+0.5f);
            }
            for(i = 0; i < food_amt; i++){
                pooli.TakePool("food", transform.position+utilFunc.RandSq(food_range), Quaternion.identity, transform);
            }
        }
        food_curcd -= Time.deltaTime;
    }
    private void FixedUpdate() {
        if(scan_cur % scan_max == 0)
        {
            Debug.Log($"gym: {gameObject.name}\np : {prey_pop}, w : {predator_pop}");
            if(isRecord)
            {
                psr.Write($"{prey_pop} ");
                wsr.Write($"{predator_pop} ");
            }
        }
        scan_cur++;
    }
    private void OnApplicationQuit() {
        if(isRecord)
        {
            psr.Close();
            wsr.Close();
        }
    }
}
