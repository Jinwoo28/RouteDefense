using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSpeed : MonoBehaviour
{


    public delegate void SpeedUp(int x);
    public static SpeedUp speedup;

    private int speednum = 1;
    public void OnClickSpeedUp()
    {
        speednum++;
        if (speednum > 3) { speednum = 1; }
        speedup(speednum);
        Debug.Log(speednum);
    }

    public void OnClickStart(int x)
    {
        speedup(x);
    }

}
