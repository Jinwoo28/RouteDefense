using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : SkillParent
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
            if (other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().ElectricDamage(10);
            }
        }
    }
}
