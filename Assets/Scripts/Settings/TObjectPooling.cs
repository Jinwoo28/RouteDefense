using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TObjectPooling<T> where T : Component
{
    [SerializeField]
    private GameObject poolObject = null;

    private Queue<T> poolqueue = new Queue<T>();

    private Transform trans = null;

    public void PoolSettings(Transform thistransform)
    {
        trans = thistransform;
    }

    public T CreatePool()
    {
        var newobj = GameObject.Instantiate(poolObject);
        newobj.transform.SetParent(trans);
        newobj.SetActive(false);
        poolqueue.Enqueue(newobj.GetComponent<T>());

        return newobj.GetComponent<T>();
    }

    public T GetPool(Vector3 _spawnPos)
    {
        if (poolqueue.Count > 0)
        {
            var obj = poolqueue.Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.position = _spawnPos;
            return obj;
        }
        else
        {
            CreatePool();
            return GetPool(_spawnPos);
        }
    }

    public void ReturnPool(T t)
    {
        t.gameObject.SetActive(false);
        poolqueue.Enqueue(t);
    }

 

}
