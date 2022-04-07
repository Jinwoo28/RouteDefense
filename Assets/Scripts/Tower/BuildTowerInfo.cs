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
        infoText.text = $"기관총" +
            "==========\n" +
            $"단일타겟을 대상으로 데미지는 낮지만 빠른 속도로 공격한다.";
    }

    public void ShowBuildTower_Mortar()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"박격포" +
            "==========\n" +
            $"포물선으로 날아가는 포탄을 발사.\n"+
            "맞은 곳을 기점으로 주위의 적에게 데미지를 준다.";
    }

    public void ShowBuildTower_CrossBow()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"석궁" +
            "==========\n" +
            $"단일 타겟 화살을 발사.\n" +
            $"준수한 공격력과 준수한 공격속도를 지님.";
    }

    public void ShowBuildTower_Tesla()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"레이저" +
            "==========\n" +
            $"단일 타겟을 대상으로 같은 적을 공격하면\n" +
            $"점점 강한 공격을 한다.";
    }

    public void ShowBuildTower_Laser()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"레이저" +
            "==========\n" +
             $"단일 타겟을 대상으로 같은 적을 공격하면\n" +
            $"점점 강한 공격을 한다.";
    }

    public void ShowBuildTowerOff()
    {
        buildtowerInfo.SetActive(false);
    }
}
