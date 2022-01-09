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
}
