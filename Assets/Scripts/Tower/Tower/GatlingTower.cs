using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTower : HitScanTower
{
    [SerializeField] Transform RotateBarral = null;

    [SerializeField] GameObject HitEffect = null;

    private Camera cam = null;
    bool isatk = false;
    bool soundPlay = false;

    protected override void Start()
    {
        base.Start();
        AS.Play();
    }

    protected override void RotateTurret()
    {
        base.RotateTurret();
        RotateBarral.Rotate(0, 0, 720*Time.deltaTime);
        cam = Camera.main;
    }

    protected override void Update()
    {
        base.Update();

        if (SetTowerCanWork)
        {
            if (FinalTarget != null)
            {
                    Vector3 Insight = cam.transform.position - FinalTarget.position;
                    HitEffect.transform.position = FinalTarget.position + Insight.normalized;
                //if (FinalTarget.GetComponentInChildren<FlyEnemy>() == null)
                //{
                //}
                //else
                //{
                //    Vector3 pos = FinalTarget.GetComponentInChildren<FlyEnemy>().GetBody().position;
                //    //pos.y += FinalTarget.position.y;

                //    //Debug.Log(pos.y);

                //    Vector3 Insight = cam.transform.position - pos;
                //    HitEffect.transform.position = pos + Insight.normalized;

                //    //Debug.Log((pos + Insight.normalized).y + "2");
                //}
                //HitEffect.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                HitEffect.SetActive(false);
                AtkParticle.SetActive(false);
            }
        }

    }

    Transform target = null;
    protected override void Attack()
    {
        if (target != FinalTarget)
        {
            AS.Play();
            target = FinalTarget;
        }

        if (FinalTarget != null)
        {
            AtkParticle.SetActive(true);
            HitEffect.SetActive(true);
            FinalTarget.GetComponent<IEnumyAttacked>().Attacked(towerinfo.towerdamage);
        }

    }

}
