using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private Transform TowerBody;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletpos;

    [SerializeField] private LayerMask enemylayer;

    [SerializeField] private float AtkDelay = 0;
    private float atkdelay = 0;
    private float AtkRange = 10;
    private float AtkDamage = 0;

    private float rotationspeed = 180;

    private Transform FinalTarget = null;

    void Start()
    {
        atkdelay = AtkDelay;
        InvokeRepeating("AutoSearch",0,0.5f);
    }

    private void Update()
    {
        if (FinalTarget == null)
        {
            Debug.Log("적 없음");
        }
        else
        {
             RotateToTarget();
            Debug.Log("적 발견");
        }
    }

    
    //타워 자동 탐색 함수
    public void AutoSearch()
    {
        Debug.Log("탐색 시작");

            //OverlapSphere : 객체 주변의 Collider를 검출
            //검출한 collider를 배열형 변수에 저장
            Collider[] E_collider = Physics.OverlapSphere(this.transform.position, AtkRange, enemylayer);

            //가장 짧은 거리의 오브젝트 위치를 담을 변수
            Transform ShortestTarget = null;

            if (E_collider.Length > 0)
            {
                float S_ShortestTarget = Mathf.Infinity;
                // 거리계산에 사용할 변수 선언.

                foreach (Collider EC in E_collider)
                {
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



    }

    

    private void RotateToTarget()
    {
        Vector3 relativePos = FinalTarget.position - transform.position;
        //현재 위치에서 타겟위치로의 방향값
        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        
        //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
        Vector3 TowerDir = Quaternion.RotateTowards(TowerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

        Debug.DrawLine(this.transform.position, FinalTarget.position, Color.red);


        //현재의 rotation값에 Vector3형태로 저장한 값 사용
        TowerBody.rotation = Quaternion.Euler(TowerDir.x, TowerDir.y, 0);




        if (Quaternion.Angle(TowerBody.rotation, rotationtotarget) < 3.0f)
        {
            atkdelay -= Time.deltaTime;
            if (atkdelay <= 0)
            {
                atkdelay = AtkDelay;
                GameObject BT = Instantiate(bullet, bulletpos.position, Quaternion.identity);
                BT.GetComponent<BulletTest>().SetTarget=FinalTarget.position;
            }
        }

    }
}
