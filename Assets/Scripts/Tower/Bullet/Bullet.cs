using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform target;
    protected float damage = 0;
    protected float bullspeed = 0;

    public void SetBulletTest(Transform _target, float _damage)
    {
        Debug.Log(_target);
        target = _target;
        damage = _damage;
    }

    private void Start()
    {
        MultipleSpeed.speedup += SpeedUP;
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }


    private void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Vector3 distance = target.position - this.transform.position;

            this.transform.position += distance.normalized * Time.deltaTime * bullspeed;

            if (Vector3.SqrMagnitude(distance) < 0.1f)
            {
                AtkCharactor();
            }
        }
    }

    protected virtual void AtkCharactor()
    {

    }


}
