using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private int InsCount = 0;

    private Queue<Bullet> poolingQueue = new Queue<Bullet>();

    private void Awake()
    {
        IniPool();
    }

    private Bullet CreateBullet()
    {
        var newObj = Instantiate(bullet, this.transform).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void IniPool()
    {
        for(int i = 0; i < InsCount; i++)
        {
            poolingQueue.Enqueue(CreateBullet());
        }
    }

    public Bullet GetObject(Vector3 _insPos)
    {
        if (poolingQueue.Count > 0)
        {
            var obj = poolingQueue.Dequeue();
            obj.gameObject.transform.position = _insPos;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newobj = CreateBullet();
            newobj.gameObject.transform.position = _insPos;
            newobj.gameObject.SetActive(true);
            Debug.Log("생성");
            return newobj;
        }
    }

    public void ReturnObject(Bullet _object)
    {
        Debug.Log("회수");
        _object.gameObject.SetActive(false);
        poolingQueue.Enqueue(_object);
    }

}
