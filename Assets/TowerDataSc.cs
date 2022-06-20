using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerData
{
    public string Name;
    public int TowerStep;
    public int TowerCode;
    public float Damage;
    public float Delay;
    public float Range;
    public float Critical;
    public float UpgradeAtk;
    public float UpgradeCri;
    public int TowerPrice;
    public int UpgradePrice;
}

[CreateAssetMenu(fileName = "TowerDataTest", menuName = "Scriptable Object/Tower Data")]
public class TowerDataSc : ScriptableObject
{
    public TowerData[] towerdata;
}
