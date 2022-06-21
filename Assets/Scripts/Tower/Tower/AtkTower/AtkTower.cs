using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkTower : Tower
{
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
                
                if (Vector3.Distance(FinalTarget.position, this.transform.position) > towerinfo.towerrange || !FinalTarget.transform.gameObject.activeInHierarchy)
                {
                    FinalTarget = null;
                }
            }
            else
            {

                //OverlapSphere : 객체 주변의 Collider를 검출
                //검출한 collider를 배열형 변수에 저장
                Collider[] E_collider = Physics.OverlapSphere(this.transform.position, towerinfo.towerrange, SearchLayer);

                //가장 짧은 거리의 오브젝트 위치를 담을 변수
                Transform ShortestTarget = null;


                if (E_collider.Length > 0)
                {
                    float S_ShortestTarget = Mathf.Infinity;
                    // 거리계산에 사용할 변수 선언.

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
                        // 터렛과 검출된 collider와의 거리를 담을 변수선언
                        // Vector3.Distance와 Vector3.magnitude도 거리비교를 할 수 있지만 이 둘은 Root을 통해 실제 거리를 계산하기 때문에 연산이 더 들어간다.
                        //SqrMagnitude는 실제거리*실제거리로 Root가 계산되지 않는 함수로 단순 거리비교일 때는 이것을 쓰는 게 연산 속도가 빠르다.

                        if (CalDistance < S_ShortestTarget)
                        {
                            S_ShortestTarget = CalDistance;
                            ShortestTarget = EC.transform;
                        }
                    }
                    FinalTarget = ShortestTarget;
                    //가장 거리가 짧은 대상을 최종 타겟으로 설정.
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

        if (FinalTarget.GetComponent<Enemy>().GetEnemyType == 0) 
        {
            relativePos = FinalTarget.position - transform.position;
        }
        else
        {
            relativePos = FinalTarget.GetComponentInChildren<BirdHitBox>().gameObject.transform.position - transform.position;
        }

        
        //현재 위치에서 타겟위치로의 방향값
        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
       // Vector3 TowerDir = Quaternion.RotateTowards(towerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;
        Vector3 TowerDir2 = Quaternion.RotateTowards(towerTurret.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

        //현재의 rotation값에 Vector3형태로 저장한 값 사용
        // towerBody.rotation = Quaternion.Euler(0, TowerDir.y, 0);

        if (FinalTarget.GetComponent<Enemy>().GetEnemyType == 0)
        {
            towerTurret.rotation = Quaternion.Euler(TowerDir2.x + (FinalTarget.localScale.y / 2), TowerDir2.y, 0);
        }
        else
        {
            towerTurret.rotation = Quaternion.Euler(TowerDir2.x + (FinalTarget.GetComponentInChildren<BirdHitBox>().gameObject.transform.localScale.y / 2), TowerDir2.y, 0);
        }


        if (FinalTarget != null)
        {
               
            if (Quaternion.Angle(towerTurret.rotation, rotationtotarget) < 1.0f)
            {

                Atking = true;
                atkspeed -= Time.deltaTime;
                if (atkspeed <= 0)
                {
                    atkspeed = towerinfo.atkdelay;
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
                Atking = false;
            }
        }
    }

    protected virtual void Attack()
    {

    }
}
