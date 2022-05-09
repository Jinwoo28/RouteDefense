using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultipleSpeed : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI multipletext = null;

    public delegate void SpeedUp(int x);
    public static SpeedUp speedup;


    private int speednum = 1;

    private void Start()
    {

        OnClickStart(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
 
        }
    }
    public void OnClickSpeedUp()
    {
        speednum++;
        if (speednum > 3) { speednum = 1; }
        speedup(speednum);
        multipletext.text = "X" + speednum.ToString();
    }

    public void OnClickStart(int x)
    {
        speedup(x);
        multipletext.text = "X" + x.ToString();
    }

    public void OnClcikCanCel()
    {
        speedup(speednum);
        multipletext.text = "X" + speednum.ToString();
    }

    public void StopGame()
    {
        speedup(0);
    }



 
}
