using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool{
    public string ID;
    public GameObject blueprint;
    public int amt;
}
public class objPool : MonoBehaviour{
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();
    public Dictionary<string, GameObject> poolobjDict = new Dictionary<string, GameObject>();
    public static objPool Instance{get; private set;}
    private void Reset() {
        foreach(Pool cpool in pools)
        {
            Debug.Log("cleared " + cpool.ID);
            poolDict[cpool.ID].Clear();
        }
    }
    private void Awake() {
        Instance = this;
        foreach(Pool cpool in pools)
        {
            Queue<GameObject> poolq = new Queue<GameObject>();
            for(int i = 0; i < cpool.amt; i++)
            {
                GameObject obj = Instantiate(cpool.blueprint, transform);
                obj.SetActive(false);
                poolq.Enqueue(obj);
            }
            poolDict.Add(cpool.ID, poolq);
            poolobjDict.Add(cpool.ID, cpool.blueprint);
        }
    }
    private void addintoPool(string ID)
    {
        for(int i = 0; i < 30; i++)
        {
            GameObject obj = Instantiate(poolobjDict[ID], transform);
            obj.SetActive(false);
            poolDict[ID].Enqueue(obj);
        }
    }
    public GameObject TakePool(string ID, Vector3 p, Quaternion q, Transform t)
    {
        if(!poolDict.ContainsKey(ID))
        {
            Debug.LogWarning("ID : " + ID + " not found");
            return null;
        }
        else if(poolDict[ID].Count == 0)
        {
            Debug.LogWarning("ID : " + ID + " not available");
            addintoPool(ID);
            return null;
        }
        GameObject obj = poolDict[ID].Dequeue();
        obj.transform.position = p;
        obj.transform.rotation = q;
        obj.transform.parent = t;
        obj.SetactivateForAllChildren(true);
        poolDict[ID].Enqueue(obj);
        return obj;
    }
}
