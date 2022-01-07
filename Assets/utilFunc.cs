using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class geneData
{
    public float maxhp, curhp, consume, sensor;
    //public int max_killCD, cur_killCD;
}
public static class utilFunc
{
    static Vector3 tmppos;
    public static float spawnRange = 80;
    public static Vector3 RandSq(float rg)
    {
        tmppos.x = Random.Range(-rg, rg);
        tmppos.y = Random.Range(-rg, rg);
        return tmppos;
    }
}
