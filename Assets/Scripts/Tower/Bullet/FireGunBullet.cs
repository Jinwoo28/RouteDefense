using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBullet : MonoBehaviour
{
    private float Damage = 0;
    private float Delay = 1;
    private float Timer = 0;

    [SerializeField] private bool isAtk = false;
    private int FCount = 0;
    public float GetDamage() => Damage;

    private List<Enemy_Creture> enemylist = new List<Enemy_Creture>();

    private BoxCollider Bcollider;
    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
        //Bcollider.enabled = false;
    }

    public void SetUp(float _damage,int Count)
    {
        Damage = _damage;
        FCount = Count;
        isAtk = true;
        Bcollider.enabled = true;
        Invoke("Off", 0.1f);
        // Debug.Log("공격1");
        // StartCoroutine("fireDamage");
    }
    
    private void Off()
    {
        Bcollider.enabled = false;
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
                other.GetComponent<Enemy_Creture>().FireAttacked(Damage, FCount);
            }
            else if (other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().EnemyAttacked(Damage);
            }
        }
    }


    //private void OnTriggerStay(Collider other)
    //{


    //    if (isAtk)
    //    {
            
    //        Debug.Log("1단계");
    //        if (other.CompareTag("Enemy"))
    //        {
    //            Debug.Log("2단계");
    //            if (other.GetComponent<Enemy_Creture>() != null)
    //            {
    //                Debug.Log("불 공격");
    //                other.GetComponent<Enemy_Creture>().FireAttacked(Damage);
    //            }
    //            else if (other.GetComponent<Enemy>() != null)
    //            {
    //                other.GetComponent<Enemy>().EnemyAttacked(Damage);
    //            }
    //        }
    //        isAtk = false;
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        if (other.GetComponent<Enemy_Creture>() != null)
    //        {
    //            other.GetComponent<Enemy_Creture>().FireAttacked(Damage);
    //        }
    //        else if(other.GetComponent <Enemy>() != null)
    //        {
    //            other.GetComponent<Enemy>().EnemyAttacked(Damage);
    //        }
    //    }
    //}

}
