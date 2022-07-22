using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public int[] enemyKind;
    public float spawnTime;
    public int spawnCount;
}

[System.Serializable]
public class StageDataFrame
{
    public int stageCode;
    public int stageCount;
    public RoundData[] roundData;
}

[CreateAssetMenu(fileName = "StageDataTest", menuName = "Scriptable Object/Stage Data", order = int.MaxValue)]
public class SetStageEnemyData : ScriptableObject
{
    public StageDataFrame[] stageDataFrame;
}
