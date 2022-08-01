using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkTower : Tower
{
    [SerializeField] protected Transform shootPos = null;
    [SerializeField] protected GameObject AtkParticle = null;

    private Transform targetCollider = null;
    protected override void Awake()
    {
        base.Awake();
        SearchLayer = 1<<6;
    }

    protected virtual void Update()
    {
        if (TowerCanWork)
        {
            if (FinalTarget != null)
            {
                RotateTurret();
            }
            else
            {
                if (SoundStop)
                {
                    AS.Stop();
                }
            }
        }
    }

    protected override IEnumerator AutoSearch()
    {
        while (true)
        {
            if (FinalTarget != null)
            {

                if (!FinalTarget.transform.gameObject.activeInHierarchy||Vector3.Distance(FinalTarget.GetComponent<IEnumyAttacked>().GetPos().gameObject.transform.position, this.transform.position) > towerinfo.towerrange)
                {
                    FinalTarget = null;
                }
            }
            else
            {

                //OverlapSphere : ��ü �ֺ��� Collider�� ����
                //������ collider�� �迭�� ������ ����
                Collider[] E_collider = Physics.OverlapSphere(this.transform.position, towerinfo.towerrange, SearchLayer);

                //���� ª�� �Ÿ��� ������Ʈ ��ġ�� ���� ����
                Transform ShortestTarget = null;


                if (E_collider.Length > 0)
                {
                    float S_ShortestTarget = Mathf.Infinity;
                    // �Ÿ���꿡 ����� ���� ����.

                    foreach (Collider EC in E_collider)
                    {
                        if(towerinfo.CanAtk == 1)
                        {
                            if (EC.GetComponent<Enemy>().GetEnemyType == 1)
                            {
                                continue;
                            }
                        }
                        else if(towerinfo.CanAtk == 2)
                        {
                            if (EC.GetComponent<Enemy>().GetEnemyType == 0)
                            {
                                continue;
                            }
                        }

                        float CalDistance = Vector3.SqrMagnitude(EC.transform.position - this.transform.position);
                        // �ͷ��� ����� collider���� �Ÿ��� ���� ��������
                        // Vector3.Distance�� Vector3.magnitude�� �Ÿ��񱳸� �� �� ������ �� ���� Root�� ���� ���� �Ÿ��� ����ϱ� ������ ������ �� ����.
                        //SqrMagnitude�� �����Ÿ�*�����Ÿ��� Root�� ������ �ʴ� �Լ��� �ܼ� �Ÿ����� ���� �̰��� ���� �� ���� �ӵ��� ������.

                        if (CalDistance < S_ShortestTarget)
                        {
                            S_ShortestTarget = CalDistance;
                            ShortestTarget = EC.transform;
                        }
                    }

                    if (ShortestTarget != null)
                    {
                        if (ShortestTarget.GetComponent<Enemy>().GetEnemyType == 1)
                        {
                            FinalTarget = ShortestTarget.GetComponentInChildren<FlyEnemy>().GetBody();
                        }
                        else
                        {
                            FinalTarget = ShortestTarget;
                        }
                    }
                    //���� �Ÿ��� ª�� ����� ���� Ÿ������ ����.
                }
                else
                {
                    FinalTarget = null;
                }
            }


            yield return null;
        }
    }

    protected virtual void RotateTurret()
    {
        Vector3 relativePos = Vector3.zero;


            relativePos = FinalTarget.position - transform.position;


        
        //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
        Vector3 TowerDir = Quaternion.RotateTowards(towerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;
        Vector3 TowerDir2 = Quaternion.RotateTowards(towerTurret.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

        //������ rotation���� Vector3���·� ������ �� ���
         towerBody.rotation = Quaternion.Euler(0, TowerDir.y, 0);

 
            towerTurret.rotation = Quaternion.Euler(TowerDir2.x + (FinalTarget.localScale.y / 2), TowerDir2.y, 0);

        if (FinalTarget != null)
        {
            if (Quaternion.Angle(towerTurret.rotation, rotationtotarget) < 1.0f)
            {

                isAtking = true;
                atkDelay -= Time.deltaTime;
                if (atkDelay <= 0)
                {
                    atkDelay = towerinfo.atkdelay;
                    int critical = Random.Range(1, 101);
                    float origindamage = towerinfo.towerdamage;
                    if (critical <= towerinfo.towercritical * 100)
                    {
                        towerinfo.towerdamage *= 2;
                    }
                    Attack();

                    towerinfo.towerdamage = origindamage;
                }
            }
            else
            {
                isAtking = false;
            }
        }
    }

    protected virtual void Attack()
    {

    }
}
