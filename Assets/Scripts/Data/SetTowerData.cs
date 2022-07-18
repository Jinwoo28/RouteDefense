using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerDataFrame
{
    public string name;
    public int towerStep;
    public int towerCode;
    public float damage;
    public float delay;
    public float range;
    public float critical;
    public float upgradAtk;
    public float upgradCri;
    public int towerPrice;
    public int upgradPrice;
    public int atkType;  // 1_ 지상만 공격, 2_ 공중만 공격 3_ 모두 공격
    public string towerInfo;
    public string detailInformation;
}

[CreateAssetMenu(fileName = "TowerDataTest", menuName = "Scriptable Object/Tower Data")]
public class SetTowerData : ScriptableObject
{
    public TowerDataFrame[] towerData;
}
