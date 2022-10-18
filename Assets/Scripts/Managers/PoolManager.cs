using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private Dictionary<string, Queue<GameObject>> poolDict;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        poolDict = new Dictionary<string, Queue<GameObject>>();
    }

    public GameObject ReleaseFromThePool(GameObject obj, Vector3 pos, Quaternion rotation)
    {
        if(poolDict.TryGetValue(obj.name, out Queue<GameObject> objQueue))
        {
            if(objQueue.Count > 0)
            {
                GameObject releasedObj = objQueue.Dequeue();
                releasedObj.transform.position = pos;
                releasedObj.transform.rotation = rotation;
                releasedObj.SetActive(true);
                return releasedObj;
            }
            else
            {
                return CreateGameobject(obj, pos, rotation);
            }
        }
        else
        {
            return CreateGameobject(obj, pos, rotation);
        }

    }

    private GameObject CreateGameobject(GameObject obj, Vector3 pos, Quaternion rotation)
    {
        GameObject objInstance = Instantiate(obj);
        objInstance.name = obj.name;
        objInstance.transform.position = pos;
        objInstance.transform.rotation = rotation;
        return objInstance;
    }

    public void ReturnToPool(GameObject obj)
    {
        if(poolDict.TryGetValue(obj.name, out Queue<GameObject> objQueue))
        {
            objQueue.Enqueue(obj);
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            newQueue.Enqueue(obj);
            poolDict.Add(obj.name, newQueue);
        }

        obj.SetActive(false);
    }
}
