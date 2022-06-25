using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    Nomal,
    UnOrderCheckPoint,
    OrderCheckPoint
}

public class GameManager : MonoBehaviour
{
    private static int GameLevel = 3;
    public static int SetGameLevel { get => GameLevel; set => GameLevel = value; }

    private static string StageShape = null;

    public static string SetStageShape { get => StageShape; set => StageShape = value; }

    private static int GetMoney;
    public static int SetMoney { get => GetMoney; set => GetMoney = value; }

    public delegate void ButtonOff();
    public static ButtonOff buttonOff;

    private static int PointCount = 2;
    public static int GetSetPointCount { get => PointCount; set => PointCount = value; }
    private static StageType stagetype = StageType.OrderCheckPoint;
    public static StageType GetSetStageType { get => stagetype; set => stagetype = value; }

    public static void OffFunc()
    {
        buttonOff();
    }

}
