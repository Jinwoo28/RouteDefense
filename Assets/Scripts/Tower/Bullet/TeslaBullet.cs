using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaBullet : Bullet
{
    [SerializeField] private LayerMask enemylayer;
    private float Range = 3.0f;
    private int Count = 3;

    HashSet<Transform> enemylist = new HashSet<Transform>();

    private void Start()
    {
        bullspeed = 10.0f;
    }

    protected override void AtkCharactor()
    {
        target.GetComponent<Enemy>().EnemyAttacked(damage);

        Collider Origin = target.GetComponent<Collider>();

        Count--;
        damage--;
        Range -= 1.0f;
        if (Count <= 0)
        {
            Destroy(this.gameObject);
        }

        Collider[] E_collider = Physics.OverlapSphere(this.transform.position, Range, enemylayer);
        Transform ShortestTarget = null;

        if (E_collider.Length > 1)
        {
            float S_ShortestTarget = Mathf.Infinity;
            // 거리계산에 사용할 변수 선언.

            foreach (Collider EC in E_collider)
            {
                if(EC == Origin)
                {
                    Debug.Log("돌아가");
                    continue;
                }

                else
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
            }
            
            //가장 거리가 짧은 대상을 최종 타겟으로 설정.
            
            SetBulletTest(ShortestTarget, damage);
        }
        else
        {
            Debug.Log("파괴");
            Destroy(this.gameObject);
        }
    }
    
}
