using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StageInfo
{
    [SerializeField] private int StageNum;
    [SerializeField] private int EnemyCount;
    [SerializeField] private GameObject[] enemykind;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private StageInfo[] stageinfo = null;

    [SerializeField] private GameObject[] Enemy_Test = null;

    [SerializeField] private PlayerState playerstate = null;



    private int StageNum = 1;
    private Vector3[] WayPoint = null;
    private bool boolGameStart = false;
    private bool boolGameEnd = false;

    private bool SpawnFinish = false;

    Vector3[] waypoint;
    Vector3 SpawnPos;

    List<Enemy> EnemyCount = null;

    private void Start()
    {
        EnemyCount = new List<Enemy>();

    }

    public void gameStartCourtain(Vector3[] _waypoint, Vector3 _SpawnPos)
    {
        waypoint = _waypoint;
        SpawnPos = _SpawnPos;
        StartCoroutine("GameStart");
    }

    IEnumerator GameStart()
    {
        StartCoroutine("GameClear");
        for (int i = 0; i < 10; i++)
        {
            GameObject enemy = Instantiate(Enemy_Test[0], SpawnPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetUpEnemy(this,waypoint);

            EnemyCount.Add(enemy.GetComponent<Enemy>());
            yield return new WaitForSeconds(1.5f);
        }

        SpawnFinish = true;
    }

    public void StageStop()
    {
        SpawnFinish = false;
        StopCoroutine("GameStart");
        StopCoroutine("GameClear");
        for (int i = 0; i < EnemyCount.Count; i++)
        {
            Destroy(EnemyCount[i].gameObject);
        }
        EnemyCount.Clear();
    }

    

    IEnumerator GameClear()
    {
        while (true)
        {
            if(SpawnFinish&&EnemyCount.Count == 0)
            {
                Debug.Log("스테이지 클리어");
                StageNum++;
                break;
            }
            yield return null;
        }
    }

    //출현한 적이 체력이 다 되서 죽을 때
    public void EnemyDie(Enemy enemy,int coin)
    {
        playerstate.PlayerCoinUp(coin);
        EnemyCount.Remove(enemy);
    }

    public void EnemyArriveDestination(Enemy enemy)
    {
        playerstate.PlayerLifeDown();
        EnemyCount.Remove(enemy);
       
    }




}
