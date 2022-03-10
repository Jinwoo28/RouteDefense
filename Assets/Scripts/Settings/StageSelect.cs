using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
   
    public void SelectStage(string _stagename)
    {
        LoadSceneControler.LoadScene(_stagename);
    }
}
