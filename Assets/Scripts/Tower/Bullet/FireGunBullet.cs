using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBullet : MonoBehaviour
{
    float Damage = 0;
    private float Delay = 1;
    private float Timer = 0;
    private bool InjectDamage = true;

    public float GetDamage() => Damage;

    private List<Enemy_Creture> enemylist = new List<Enemy_Creture>();

    private BoxCollider Bcollider;
    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
    }

    public void SetUp(float _damage, float _delay)
    {
        Damage = _damage;
        Delay = _delay;
    }

    private void Update()
    {
        Debug.Log(enemylist.Count);

        if (Timer < Delay)
        {
            Timer += Time.deltaTime;
            Bcollider.enabled = false;
        }
        else
        {
            Timer = 0;

            Bcollider.enabled = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enter");
            other.GetComponent<Enemy>().EnemyAttacked(Damage);
        }
    }


}
