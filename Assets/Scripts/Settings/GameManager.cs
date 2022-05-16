using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int GameLevel = 3;
    public static int SetGameLevel { get => GameLevel; set => GameLevel = value; }

    private static string StageShape = null;

    public static string SetStageShape { get => StageShape; set => StageShape = value; }

    public delegate void ButtonOff();
    public static ButtonOff buttonOff;

    private void Start()
    {
       // Debug.Log(GameLevel);
       // Debug.Log(StageShape);
    }

    public static void OffFunc()
    {
        buttonOff();
    }

}
