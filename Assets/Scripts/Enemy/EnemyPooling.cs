using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pooling
{
    public GameObject enemy;
    public Queue<Enemy> enemypooling = new Queue<Enemy>();
}

[System.Serializable]
public class Coinpooling
{
    public GameObject coin;
    public Queue<Coin> coinPool = new Queue<Coin>();
}

public class EnemyPooling : MonoBehaviour
{

    [SerializeField] private List<pooling> enemyPooling;

    [SerializeField] private List<Coinpooling> coinPooling;



    public string GetName(int num)
    {
        return enemyPooling[num].enemy.name;
    }

    public bool GetBoss(int num)
    {
        return enemyPooling[num].enemy.GetComponent<Enemy>().GetBoss();
    }

    public Enemy CreatEnemy(int _enemyNum)
    {
        var newenemy = Instantiate(enemyPooling[_enemyNum].enemy, this.transform).GetComponent<Enemy>();
        newenemy.gameObject.SetActive(false);
        enemyPooling[_enemyNum].enemypooling.Enqueue(newenemy);
        return newenemy;
    }

    public Enemy GetEnemy(int _enemyNum,Vector3 _spawnPos)
    {
        if (enemyPooling[_enemyNum].enemypooling.Count > 0)
        {
            var obj = enemyPooling[_enemyNum].enemypooling.Dequeue();
            obj.gameObject.transform.position = _spawnPos;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {

            CreatEnemy(_enemyNum);
            var newobj = GetEnemy(_enemyNum, _spawnPos);
            
            newobj.gameObject.transform.position = _spawnPos;
            newobj.gameObject.SetActive(true);
            
            return newobj;
        }
    }

    public void ReturnEnemy(Enemy _enemy,int _enemyNum)
    {
        _enemy.gameObject.layer = 2;
        _enemy.gameObject.SetActive(false);
        _enemy.ResetHp();

        _enemy.StopAllCoroutines();
        //_enemy.transform.position = Vector3.zero;
        enemyPooling[_enemyNum].enemypooling.Enqueue(_enemy);
    }


    public Coin CreatCoin(int _enemyNum)
    {
        var newCoin = Instantiate(coinPooling[_enemyNum].coin, this.transform).GetComponent<Coin>();
        newCoin.SetEp = this;
        newCoin.InitSC();
        newCoin.gameObject.SetActive(false);
        coinPooling[_enemyNum].coinPool.Enqueue(newCoin);
        return newCoin;
    }

    public Coin GetCoin(int _enemyNum, Vector3 _spawnPos)
    {
        if (coinPooling[_enemyNum].coinPool.Count > 0)
        {
            var obj = coinPooling[_enemyNum].coinPool.Dequeue();
            obj.gameObject.transform.position = _spawnPos;
            obj.gameObject.SetActive(true);
            obj.SetUP(_spawnPos);
            return obj;
        }
        else
        {

            CreatCoin(_enemyNum);
            var newobj = GetCoin(_enemyNum, _spawnPos);

            newobj.gameObject.transform.position = _spawnPos;
            newobj.gameObject.SetActive(true);
            newobj.SetUP(_spawnPos);

            return newobj;
        }
    }

    public void ReturnCoin(Coin coin,int num)
    {
        coin.gameObject.SetActive(false);
        coin.transform.position = this.transform.position;
        coinPooling[num].coinPool.Enqueue(coin);
    }


}
