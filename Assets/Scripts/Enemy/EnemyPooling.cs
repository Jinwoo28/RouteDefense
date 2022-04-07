using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pooling
{
    public GameObject enemy;
    public Queue<Enemy> enemypooling = new Queue<Enemy>();
}


public class EnemyPooling : MonoBehaviour
{

    [SerializeField] private List<pooling> enemyPooling;



    public Enemy CreatEnemy(int _enemyNum)
    {
        var newenemy = Instantiate(enemyPooling[_enemyNum].enemy, this.transform).GetComponent<Enemy>();
        newenemy.gameObject.SetActive(false);
        newenemy.SetPooling(this, _enemyNum);
        enemyPooling[_enemyNum].enemypooling.Enqueue(newenemy);
        return newenemy;
    }

    public Enemy GetEnemy(int _enemyNum,Vector3 _spawnPos)
    {
        Debug.Log(enemyPooling[_enemyNum].enemypooling.Count);
        if (enemyPooling[_enemyNum].enemypooling.Count > 0)
        {
            Debug.Log("호출");
            var obj = enemyPooling[_enemyNum].enemypooling.Dequeue();
            obj.gameObject.transform.position = _spawnPos;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Debug.Log("생성");
            CreatEnemy(_enemyNum);
            var newobj = GetEnemy(_enemyNum, _spawnPos);
            newobj.gameObject.transform.position = _spawnPos;
            newobj.gameObject.SetActive(true);
            
            return newobj;
        }
    }

    public void ReturnEnemy(Enemy _enemy,int _enemyNum)
    {
        Debug.Log("Return");
        _enemy.gameObject.SetActive(false);
        _enemy.ResetHp();
        enemyPooling[_enemyNum].enemypooling.Enqueue(_enemy);
    }




}
