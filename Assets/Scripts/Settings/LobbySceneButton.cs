using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneButton : MonoBehaviour
{
    [SerializeField] private GameObject settingPanal = null;

    [SerializeField] private GameObject ShopPanel = null;

    public void SettingPanalOn()
    {
        settingPanal.SetActive(true);
    }
    public void SettingPanalOff()
    {
        settingPanal.SetActive(false);
    }

    public void ShopOpen()
    {
        ShopPanel.SetActive(true);
    }

    public void ShopClose()
    {
        ShopPanel.SetActive(false);
    }
}
