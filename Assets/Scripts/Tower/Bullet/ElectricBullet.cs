using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : MonoBehaviour
{
    private TeslaTower tesla = null;

    private float Count = 3;
    private float Damage = 0;

    private Transform target = null;

    public void InitSetUp(int _Count, TeslaTower _teslaTower)
    {
        Count = _Count;
        tesla = _teslaTower;
    }

    private void Update()
    {
        Vector3 Dir = target.position - transform.position;

        this.transform.position += Dir.normalized * Time.deltaTime;
    }

    public void SetUp(float _Damage,Transform _target)
    {
        target = _target;
        Damage = _Damage;
    }

    


}
