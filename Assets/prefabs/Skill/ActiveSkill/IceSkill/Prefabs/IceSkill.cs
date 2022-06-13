using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : MonoBehaviour
{
    List<Enemy> enemylist = new List<Enemy>();
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 180*Time.deltaTime, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemylist.Add(other.GetComponent<Enemy>());
            other.GetComponent<Enemy>().SlowDown();
        }
    }

    private void OnDestroy()
    {
        for(int i = 0; i < enemylist.Count; i++)
        {
            enemylist[i].returnSpeed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().returnSpeed();
        }
    }
}
