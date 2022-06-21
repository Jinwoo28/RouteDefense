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


    private float FireDamage = 0;
    public void FireAttacked(float damage)
    {
        FireDamage = damage;
        if (this.gameObject.activeInHierarchy)
        {
            if (!fired)
            {
                SpeedChange(1.25f);
                fired = true;
            }
            StopCoroutine("DotDamage");
            StartCoroutine("DotDamage");
        }
    }

    private bool fired = false;


  
    public IEnumerator DotDamage()
    {
        hpbarprefab.GetComponent<EnemyHpbar>().StateChange(enemyState.Fire);
        int damagecount = 5;

        while (damagecount > 0)
        {
            damagecount--;

            float realdamage = underTheSea?FireDamage/2:FireDamage;

            if (realdamage < GetHp)
            {
                realDamage(realdamage);
            }
            else
            {
                fired = false;
                hpbarprefab.GetComponent<EnemyHpbar>().ReturnIcon(enemyState.Fire);
                returnSpeed();
                EnemyDie();
            }

            if (underTheSea)
            {

                break;
            }

            yield return new WaitForSeconds(0.4f);
        }

        returnSpeed();
        hpbarprefab.GetComponent<EnemyHpbar>().ReturnIcon(enemyState.Fire);

        fired = false;
       
    }



}
