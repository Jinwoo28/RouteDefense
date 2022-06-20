using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeslaBullet : MonoBehaviour
{
    [SerializeField] private LayerMask enemylayer;
    private float Range = 0.5f;

    private Transform target = null;

    private TeslaTower tesla = null;

    private float Damage = 0;
    private int Count = 3;
    private int OriginCount = 0;
    private int MaxCount = 3;
    private int MaxOriginCount = 0;

    List<Enemy> enemylist = new List<Enemy>();

    [SerializeField]
    private GameObject BoltPrefab = null;

    Queue<TeslaEffect> boltqueue = new Queue<TeslaEffect>();

    //초기화
    public void InitSetUp(int _Count, TeslaTower _teslaTower,int _MaxCount)
    {
        Count = _Count;
        OriginCount = Count;
        tesla = _teslaTower;
        MaxCount = _MaxCount;
        MaxOriginCount = MaxCount;
        MakeQueue();
    }

    //번개 이펙트 오브젝트 생성
    private void MakeQueue()
    {
        for(int i = 0; i < MaxCount+Count; i++)
        {
            var bolt = Instantiate(BoltPrefab, this.transform).GetComponent<TeslaEffect>();
            bolt.gameObject.transform.position = tesla.GetShootPos().position;
            bolt.GetTB = this;
            bolt.gameObject.SetActive(false);
            boltqueue.Enqueue(bolt);
        }
    }

    //큐에서 오브젝트 가져오기
    private TeslaEffect DequeueBolt()
    {
        var obj = boltqueue.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    //큐에 오브젝트 집어넣기
    public void ReturnEffect(TeslaEffect effect)
    {
        boltqueue.Enqueue(effect);
        effect.gameObject.SetActive(false);
        effect.gameObject.transform.position = tesla.GetShootPos().position;
    }


    //공격이 시작할 때 처음 시작될 함수
    public void SetUp(float _Damage, Transform _target,Transform Startpos)
    {        
        //이펙트를 하나 가져와서 실행
        var obj = DequeueBolt();

        obj.SetPos(Startpos, _target);

        //List에 활성화 이펙트 넣어두기
        ActiveEffect.Add(obj);

        target = _target;
        Damage = _Damage;

        StartCoroutine("Trigger");
    }

    /*
     * 1. 날아갈 타겟 받기
     * 2. 날아가기
     * 3. 도착했으면 데미지를 주고 다시 타겟찾기 시작
     * 3-1 이미 공격한 적이 있난 타겟은 제외
     * 3-2 공격횟수가 남았는지 확인
     * 4. 찾았으면 1번부터 반복
     * 5. 없으면 돌아가기
     
     */


    private List<TeslaEffect> ActiveEffect = new List<TeslaEffect>();

    IEnumerator Trigger()
    {

        yield return new WaitForSeconds(0.15f);
        AtkCharactor(Damage);
    }
    
    public void AtkCharactor(float damage)
    {



        if (target.GetComponent<Enemy>().GetShock())
        {
            if (MaxCount > 1)
            {
                Count++;
                MaxCount--;
            }
        }

        //데미지 주기
       
        //카운터 감소, 데미지 감소
        Count--;
        
       

        if (!target.GetComponent<Enemy>().GetWet&&Range > 0.2f)
        {
            Range -= 0.1f;
        }

        //공격했던 적의 리스트 보관
        enemylist.Add(target.GetComponent<Enemy>());

        //현재 위치를 기준으로 적을 검색
        Collider[] E_collider = Physics.OverlapSphere(target.position, Range, enemylayer);


        Transform ShortestTarget = null;

        target.GetComponent<Enemy>().ElectricDamage(Damage);

        Damage--;

        if (Damage < 1)
        {
            Damage = 1;
        }

        if (Count == 0)
        {
            ReturnBullet();
            return;
        }

        //이미 맞은 적 + 새로운 적이 한 마리 이상일 때
        if (E_collider.Length > 1)
        {
            float S_ShortestTarget = Mathf.Infinity;
            // 거리계산에 사용할 변수 선언.

            foreach (Collider EC in E_collider)
            {
                //검사 대상이 이미 공격한 적이라면 건너뛰기
                if(enemylist.Contains(EC.GetComponent<Enemy>()))
                {
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

            //인근의 적이 이미 공격한 대상밖에 없을 경우 종료
            if(ShortestTarget == null)
            {
                ReturnBullet();
                return;
            }
            //대상이 있다면 그 대상을 target으로 공격함수 실행
            else
            {
                SetUp(Damage, ShortestTarget,target);
            }
            
            //가장 거리가 짧은 대상을 최종 타겟으로 설정.

        }
        //인근의 대상이 없다면 종료
        else
        {
            ReturnBullet();
            return;
        }
    }

    public void ReturnBullet()
    {
        ActiveEffect.Clear();
        this.transform.position = tesla.GetShootPos().position;
        target = null;
        Range = 2.0f;
        enemylist.Clear();
        Count = OriginCount;
        MaxCount = MaxOriginCount;
        //tesla.ReturnBullet(this);        
    }
    
}
