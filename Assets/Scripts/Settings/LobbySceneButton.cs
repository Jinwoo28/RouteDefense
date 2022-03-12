using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneButton : MonoBehaviour
{
    [SerializeField] private GameObject settingPanal = null;




    public void SettingPanalOn()
    {
        settingPanal.SetActive(true);
    }
    public void SettingPanalOff()
    {
        settingPanal.SetActive(false);
    }
}
