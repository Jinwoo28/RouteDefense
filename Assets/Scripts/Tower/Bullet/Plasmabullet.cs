using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasmabullet : MonoBehaviour
{
    private float Damage;

    private bool HitTile = false;

    private Vector3 OriginScale;

    private float DeltaValue = 0;

    private Transform parent;

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    private void Start()
    {
        OriginScale = this.transform.localScale;
    }

    private void Update()
    {
        float X = this.transform.localScale.x;
        float Z = this.transform.localScale.z;
        float Y = this.transform.localScale.y;

        

        if (HitTile)
        {
            this.transform.localScale = new Vector3(X - (Time.deltaTime * 0.5f), this.transform.localScale.y, Z - (Time.deltaTime * 0.5f));
        }
        else
        {
            DeltaValue += Time.deltaTime;



            this.transform.position += (-this.transform.up * Time.deltaTime*30);

            float Scale = Vector3.Distance(this.transform.localPosition, this.transform.parent.localPosition);
            this.transform.localScale = new Vector3(X - (Time.deltaTime * 0.75f), Scale, Z - (Time.deltaTime * 0.75f));
            
        }

    }

    public void ReturnScale()
    {
        this.transform.localScale = OriginScale;
        this.transform.position = this.transform.parent.position;
        HitTile = false;
        DeltaValue = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            HitTile = true;
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().EnemyAttacked(Damage);
        }
    }

}
