using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDataFrame
{
    public string name;
    public int unitcode;
    public float hp;
    public float speed;
    public float damage;
    public int coin;
    public int amour;
    public int avoidance;
    public float feature;
    public int enemytype;
}

[CreateAssetMenu(fileName = "EnemyDataTest", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class SetEnemyData : ScriptableObject
{
    public EnemyDataFrame[] enemyData;
}
