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

   

    public void FireAttacked(float damage)
    {
        if (!fired)
        {
            fired = true;
            StartCoroutine(DotDamage(damage));
        }
    }

    private bool fired = false;


  
    public IEnumerator DotDamage(float damage)
    {
        
        int damagecount = 5;
        while (damagecount > 0)
        {
            damagecount--;
            if (damage < GetHp)
            {
                firedamage(damage);
            }
            else
            {
                EnemyDie();
                fired = false;
            }
            yield return new WaitForSeconds(0.5f);
        }

        fired = false;
       
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("FireBullet"))
        {
            firedamage(1);
        }
    }


}
