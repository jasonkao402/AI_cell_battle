using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void SetactivateForAllChildren(this GameObject go, bool state)
    {
        setChildActive_recur(go, state);
    }
 
    public static void setChildActive_recur(GameObject go, bool state)
    {
        go.SetActive(state);
 
        foreach (Transform child in go.transform)
        {
            setChildActive_recur(child.gameObject, state);
        }
    }
    public static Collider2D ClosestCollider(Vector3 unitPosition, Collider2D[] tgtColliders)
    {
        float bestdstc = 999999.0f, tmpdstc;
        Collider2D bestCollider = null;

        foreach (Collider2D tgt in tgtColliders)
        {
            tmpdstc = Vector3.SqrMagnitude(unitPosition - tgt.transform.position);
            if (tmpdstc < bestdstc)
            {
                bestdstc = tmpdstc;
                bestCollider = tgt;
            }
        }
        return bestCollider;
    } 
}
