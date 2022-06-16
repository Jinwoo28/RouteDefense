using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBullet : MonoBehaviour
{
    private float Damage = 0;
    private float Delay = 1;
    private float Timer = 0;

    public float GetDamage() => Damage;

    private List<Enemy_Creture> enemylist = new List<Enemy_Creture>();

    private BoxCollider Bcollider;
    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
        Bcollider.enabled = false;
    }

    public void SetUp(float _damage, float _delay)
    {
        Damage = _damage;
        Delay = _delay;

        StartCoroutine("fireDamage");
    }
    


    IEnumerator fireDamage()
    {
        yield return new WaitForSeconds(0.1f);
        Bcollider.enabled = true;

        yield return null;
        Bcollider.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy_Creture>() != null)
            {
                other.GetComponent<Enemy_Creture>().FireAttacked(Damage);
            }
            else if(other.GetComponent <Enemy>() != null)
            {
                other.GetComponent<Enemy>().EnemyAttacked(Damage);
            }
        }
    }

}
