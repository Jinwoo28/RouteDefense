using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private SphereCollider Range;
    private void Start()
    {
        Range = this.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Range.radius = 1.5f;
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().ElectricDamage(10);
        }
    }
}
