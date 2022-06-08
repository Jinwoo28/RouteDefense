using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerData
{
    public string Name;
    public int TowerStep;
    public int TowerCode;
    public int Damage;
    public float Delay;
    public float Range;
    public int Critical;
    public float UpgradeAtk;
    public int UpgradeCri;
    public int TowerPrice;
    public int UpgradePrice;
}

[CreateAssetMenu(fileName = "TowerDataTest", menuName = "Scriptable Object/Tower Data", order = int.MaxValue-1)]
public class TowerDataSc : MonoBehaviour
{
    TowerData[] towerdata;
}
