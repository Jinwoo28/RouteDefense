using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : AtkTower
{
    private LineRenderer lR = null;
    private Transform existingTarget = null;
    private float atkdamage = 0;
    private bool islaserAtking = false;

    protected override void Start()
    {
        base.Start();
        lR = this.GetComponent<LineRenderer>();
        lR.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (islaserAtking)
        {
            if (FinalTarget != null)
            {
                lR.SetPosition(0, shootPos.position);
                lR.SetPosition(1, existingTarget.position);

                AtkParticle.transform.position = existingTarget.position/*FinalTarget.position + new Vector3(0, FinalTarget.localScale.y / 2, 0)*/;
                SoundStop = false;
            }
            else
            {
                AtkParticle.SetActive(false);
                lR.enabled = false;
                islaserAtking = false;
                SoundStop = true;
            }
        }
    }

  

    protected override void Attack()
    {
        AS.Play();
        islaserAtking = true;

        if (existingTarget != FinalTarget)
        {
            existingTarget = FinalTarget;
            atkdamage = towerinfo.towerdamage;
        }

        lR.enabled = true;
        AtkParticle.SetActive(true);
        //Debug.Log(existingTarget);
        //Debug.Log(existingTarget.GetComponent<IEnumyAttacked>());
        existingTarget.GetComponent<IEnumyAttacked>().Attacked(atkdamage);
        atkdamage += (GetStep);
    }

}
