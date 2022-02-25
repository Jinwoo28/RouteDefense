using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemy_Test = null;

    [SerializeField] private PlayerState playerstate = null;

    private int StageNum = 0;
    private Vector3[] WayPoint = null;
    private bool boolGameStart = false;
    private bool boolGameEnd = false;

    private bool SpawnFinish = false;

    List<Enemy> EnemyCount = null;

    private void Start()
    {
        EnemyCount = new List<Enemy>();
    }

    public void gameStartCourtain(Vector3[] waypoint, Vector3 SpawnPos)
    {
        StartCoroutine(GameStart(waypoint,SpawnPos));
    }

    IEnumerator GameStart(Vector3[] waypoint, Vector3 SpawnPos)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject enemy = Instantiate(Enemy_Test[0], SpawnPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetUpEnemy(this,waypoint);

            EnemyCount.Add(enemy.GetComponent<Enemy>());
            yield return new WaitForSeconds(1.5f);
        }

        SpawnFinish = true;

        GameManager.canslestage += StageStop;
    }

    public void StageStop()
    {
        SpawnFinish = false;
        for(int i = 0; i < EnemyCount.Count; i++)
        {
            Destroy(EnemyCount[i].gameObject);
        }
    }

    

    IEnumerator GameClear()
    {
        while (true)
        {
            if(SpawnFinish&&EnemyCount.Count == 0)
            {

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
