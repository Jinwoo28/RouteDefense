using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyState
{
    public string Name;
    public int UnitCode;
    public float Hp;
    public float Speed;
    public float Damage;
    public int coin;
    public int Amour;
    public int avoidance;
}

[CreateAssetMenu(fileName = "EnemyDataTest", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyDataTest : ScriptableObject
{
   
    public EnemyState[] enemydata;


}
