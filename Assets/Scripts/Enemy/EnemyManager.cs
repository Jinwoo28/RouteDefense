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
    [SerializeField] private StageInfo[] stageinfo = null;

    [SerializeField] private GameObject[] Enemy_Test = null;

    [SerializeField] private PlayerState playerstate = null;
    [SerializeField] private Transform canvas = null;

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

        int count = stageinfo[StageNum - 1].EnemyCount;
       

        for (int i = 0; i < 1; i++)
        {
            GameObject enemy = Instantiate(Enemy_Test[0], SpawnPos, Quaternion.identity);

            enemy.GetComponent<Enemy>().SetUpEnemy(this,waypoint,canvas);
            EnemyCount.Add(enemy.GetComponent<Enemy>());

            

            yield return new WaitForSeconds(1.5f);
        }

        SpawnFinish = true;
        StageNum++;
    }
   

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




}
