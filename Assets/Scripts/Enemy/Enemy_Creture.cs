using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Creture : Enemy
{
    private float timevalue = 0;
    private float originHP = 0;
    [SerializeField] private int HpUpValue = 0;

    private bool iseating = false;
    public bool GetSetEating { get => iseating; set => iseating = value; }

    protected override void Start()
    {
        base.Start();
        originHP = GetHp;
    }

    // Update is called once per frame
    void Update()
    {
        UnitCharacteristic();
    }

    protected override void UnitCharacteristic()
    {
        timevalue += Time.deltaTime;

        if (timevalue >= 3.0f)
        {
            timevalue = 0;
            if (GetHp < originHP)
            {
                GetHp = HpUpValue;
            }
        }
    }

    public void HealHp(int num)
    {
        float Hp = GetHp;
       if(Hp+num >= originHP)
        {
            SetOriginHp = originHP;
        }
        else
        {
            SetOriginHp = Hp = num;
        }

        Debug.Log(GetHp);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    StopCoroutine("eatingfruit");
    //    if (other.CompareTag("Fruit"))
    //    {
    //        StartCoroutine("eatingfruit", other.GetComponent<Fruit>());
    //    }
    //}

    //IEnumerator eatingfruit(Fruit fruit)
    //{
    //    int eatvalue = (int)(originHP / 10)+1;

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.3f);
    //        if (fruit != null)
    //        {
    //            if (fruit.GetRemains - eatvalue >= 0)
    //            {
    //                fruit.CountDown(eatvalue);
    //                if (GetHp + eatvalue >= originHP) SetOriginHp = originHP;
    //                else GetHp = eatvalue;
    //            }
    //            else
    //            {
    //                fruit.ReturnObj();
    //                if (GetHp + eatvalue >= originHP) SetOriginHp = originHP;
    //                else GetHp = fruit.GetRemains;
    //            }


    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //}

    public void FireAttacked()
    {
        Debug.Log("불");
        StopCoroutine("DotDamage");
        StartCoroutine("DotDamage");
    }

    public float firedamage = 0;
    public int GetFireDamage { set => firedamage = value; }

    IEnumerator DotDamage()
    {
        int damagecount = 5;
        while (damagecount > 0)
        {
            Debug.Log("데미지");
            damagecount--;
            if (firedamage < GetHp)
            {
                firedamage(firedamage);
            }
            else
            {
                EnemyDie();
            }
            yield return new WaitForSeconds(0.5f);
        }
       
    }
}
