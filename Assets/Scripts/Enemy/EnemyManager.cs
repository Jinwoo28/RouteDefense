using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class StageInfo
{
    public int StageNum;
    public int EnemyCount;
    public GameObject[] enemykind;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject unituiParent = null;

    //스테이지 정보
    [SerializeField] private StageInfo[] stageinfo = null;

    //적이 죽거나 도착지에 도착했을 때, 골드획득이나 라이프 감소를 위한 플레이어 정보
    [SerializeField] private PlayerState playerstate = null;

    //enemy의 체력바와 공격시 데미지를 띄우는 UI정보를 위한 canvas
    [SerializeField] private Transform canvas = null;

    [SerializeField] private GameObject hpbar = null;
    [SerializeField] private GameObject damagenum = null;

    private int StageNum = 0;

    //스테이지가 실행중인지 판단
    private bool gameongoing = false;

    //적 스폰이 끝났는지 여부
    private bool SpawnFinish = false;

    private Vector3[] waypoint;
    private Vector3 SpawnPos;

    //소환되는 적들의 정보를 담을 List
    List<Enemy> EnemyCount = null;

    public int Getmaxstage => stageinfo.Length;
    public int Getcurrentstage => StageNum + 1;

    private void Start()
    {
        EnemyCount = new List<Enemy>();
    MultipleSpeed.speedup += SpeedUP;
    }

    

private void SpeedUP(int x)
{
    Time.timeScale = x;
}

//게임 시작 될 때 enemy의 루트와 스폰 위치를 받아서 게임 시작
public void gameStartCourtain(Vector3[] _waypoint, Vector3 _SpawnPos)
    {
        waypoint = _waypoint;
        SpawnPos = _SpawnPos;
        StartCoroutine("GameStart");
    }

    IEnumerator GameStart()
    {
        gameongoing = true;

        //적이 나올 개수
        int count = stageinfo[StageNum ].EnemyCount;
        int stagenum = StageNum;
        //적 종류
        GameObject[] EnemyList = stageinfo[StageNum].enemykind;

        int enemykind = EnemyList.Length;

        for (int i = 0; i < count; i++)
        {
            int enemynum = Random.Range(0, enemykind);
            GameObject enemy = Instantiate(EnemyList[enemynum], SpawnPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetUpEnemy(this,waypoint,canvas,hpbar,damagenum);

            //소환되는 enemy를 list에 추가
            EnemyCount.Add(enemy.GetComponent<Enemy>());


                yield return new WaitForSeconds(1.5f);
            
        }
        SpawnFinish = true;


        while (true)
        {
            if (SpawnFinish && EnemyCount.Count == 0)
            {
                gameongoing = false;
                StageNum++;
                break;
            }
            yield return null;
        }
    }

    public bool GetGameOnGoing => gameongoing;


    //출현한 적이 체력이 다 되서 죽을 때
    public void EnemyDie(Enemy enemy,int coin)
    {
        playerstate.PlayerCoinUp(coin);
        EnemyCount.Remove(enemy);
    }

    //출현한 적이 도착지에 도착했을 때
    public void EnemyArriveDestination(Enemy enemy)
    {
        playerstate.PlayerLifeDown();
        EnemyCount.Remove(enemy);
    }

    public bool GameOnGoing
    {
        get
        {
            return gameongoing;
        }
    }




}
