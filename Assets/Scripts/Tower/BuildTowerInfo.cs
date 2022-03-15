using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildTowerInfo : MonoBehaviour
{
    [SerializeField] private GameObject buildtowerInfo = null;
    [SerializeField] private TextMeshProUGUI infoText = null;
    
    public void ShowBuildTower_Gatling()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"Gatling Tower\n " +
            "===================\n"+
            $"It has low damage but hight Attack speed.";
    }

    public void ShowBuildTower_Mortar()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"Mortar Tower\n " +
            "===================\n" +
            $"It has splash damage but low Attack speed.\n"+
            "It fires bullets with a howitzer.";
    }

    public void ShowBuildTower_CrossBow()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"CrossBow Tower\n " +
            "===================\n" +
            $"This is a powerful single attack.";
    }

    public void ShowBuildTower_Tesla()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"Tesla Tower\n " +
            "===================\n" +
            $"This is a chain of electric attacks.";
    }

    public void ShowBuildTowerOff()
    {
        buildtowerInfo.SetActive(false);
    }
}
