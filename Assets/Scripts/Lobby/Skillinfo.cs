using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class skill
{
    public GameObject buyButton;
    public GameObject upgradeButton;
}

public class Skillinfo : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI coin = null;
    [SerializeField] private GameObject[] SkillInfo = null;
    [SerializeField] private skill[] skill = null;

    [SerializeField] private TextMeshProUGUI[] showstate = null;

    [SerializeField] private UserInformation userinfo = null;

    [SerializeField] private GameObject pricePanel = null;
    [SerializeField] private TextMeshProUGUI price = null;

    private int ShowNum = 0;
    private int PriceValue = 0;

    private void Update()
    {
        coin.text = "코인 : " + UserInformation.userDataStatic.userCoin;

 
    }


    private void ActiveSkillState(int num)
    {
        if (UserInformation.userDataStatic.skillSet[num].skilltype == SkillSet.skillType.Active)
        {
            if (!UserInformation.userDataStatic.skillSet[num].skillUnLock)
            {
                showstate[num].text = $"현재 레벨 : 구입안됨\n" +
                        $"현재 쿨타임 : --초\n" +
                        $"[다음 레벨 쿨타임] : --초\n\n" +
                        $"현재 데미지 : --초\n" +
                        $"[다음 레벨 데미지] : --";
            }
            else
            {
                showstate[num].text = $"현재 레벨 : {UserInformation.userDataStatic.skillSet[num].skillLevel}\n" +
                        $"현재 쿨타임 : {UserInformation.userDataStatic.skillSet[num].skillcooltime}초\n" +
                        $"[다음 레벨 쿨타임] : {UserInformation.userDataStatic.skillSet[num].skillcooltime - UserInformation.userDataStatic.skillSet[num].cooltimeDecrease}초\n\n" +
                        $"현재 데미지 : {UserInformation.userDataStatic.skillSet[num].damage}\n" +
                        $"[다음 레벨 데미지] : {UserInformation.userDataStatic.skillSet[num].damage + UserInformation.userDataStatic.skillSet[num].damageIncrease}";
            }
        }

        else if(UserInformation.userDataStatic.skillSet[num].skilltype == SkillSet.skillType.Passive)
        {
            if (!UserInformation.userDataStatic.skillSet[num].skillUnLock)
            {
                showstate[num].text = $"현재 레벨 : 구입안됨\n" +
                        $"현재 추가 퍼센트 : --%\n" +
                        $"[다음 레벨 퍼센트] : --%\n\n";
            }
            else
            {
                showstate[num].text = $"현재 레벨 : {UserInformation.userDataStatic.skillSet[num].skillLevel}\n" +
                        $"현재 추가 퍼센트 : {UserInformation.userDataStatic.skillSet[num].damage}%\n" +
                        $"[다음 레벨 데미지] : {UserInformation.userDataStatic.skillSet[num].damage + UserInformation.userDataStatic.skillSet[num].damageIncrease}%\n\n";
            }
        }

    }


    public void ShowSkillInfo(int num)
    {
        ShowNum = num;
        for (int i = 0; i < SkillInfo.Length; i++)
        {
            SkillInfo[i].SetActive(false);
        }

        SkillInfo[num].SetActive(true);

        ActiveSkillState(num);

        if (UserInformation.userDataStatic.skillSet[num].skillUnLock)
        {
            skill[num].buyButton.SetActive(false);
            if (UserInformation.userDataStatic.skillSet[num].skillLevel < 5)
            {
                skill[num].upgradeButton.SetActive(true);
            }
            else
            {
                skill[num].upgradeButton.SetActive(false);
                OffShowPrice();
            }
            PriceValue = UserInformation.userDataStatic.skillSet[num].upgradeprice;
            Debug.Log(PriceValue);
        }
        else
        {
            
            skill[num].buyButton.SetActive(true);
            skill[num].upgradeButton.SetActive(false);
            PriceValue = UserInformation.userDataStatic.skillSet[num].skillprice;
        }
    }

    public void UnLockSkill(string _skillname)
    {
        UserInformation.userDataStatic.UnLockSkill(_skillname);
        ShowSkillInfo(ShowNum);
    }

    public void LevelUpSkill(string _skillname)
    {
        UserInformation.userDataStatic.SkillLevelUp(_skillname);
        OffShowPrice();
        ShowSkillInfo(ShowNum);
        OnShowPrice();
    }


    public void OnShowPrice()
    {
        pricePanel.SetActive(true);
        price.text = PriceValue.ToString() + "원";
    }

    public void OffShowPrice()
    {
        pricePanel.SetActive(false);
    }







}
