using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class SKillText
{
    public TextMeshProUGUI level;
    public string skillname;
}


[System.Serializable]
public class PassiveSkillSet
{
    private string BundleName;
    public string GetName => BundleName;

    public List<PassiveForm> skillInfoList = new List<PassiveForm>();

    public PassiveSkillSet(string name)
    {
        BundleName = name;
    }
}

[System.Serializable]
public class ActiveSkillSet
{
    public string BundleName;
    //public string GetName => BundleName;

    public List<ActiveForm> skillInfoList = new List<ActiveForm>();

    public ActiveSkillSet(string name)
    {
        BundleName = name;
    }
}



public class SkillSettings : MonoBehaviour
{
    [SerializeField] private List<SKillText> MoneyBundle;
    [SerializeField] private List<SKillText> FireBundle;

    [SerializeField] private TextMeshProUGUI skillname = null;
    [SerializeField] private TextMeshProUGUI Infotext = null;
    [SerializeField] private TextMeshProUGUI Pricetext = null;
    [SerializeField] private TextMeshProUGUI ButtonText = null;

    [SerializeField] private TextMeshProUGUI Askillname = null;
    [SerializeField] private TextMeshProUGUI AInfotext = null;
    [SerializeField] private TextMeshProUGUI APricetext = null;
    [SerializeField] private TextMeshProUGUI AButtonText = null;


    [SerializeField] private GameObject NotTextPanal = null;
    [SerializeField] private TextMeshProUGUI NotText = null;

    [SerializeField] private ActiveBundle ActiveBundle = null;
    [SerializeField] private PassiveBundle passiveBundle = null;

    public static List<PassiveSkillSet> SPassiveSkill = new List<PassiveSkillSet>();
    public static List<ActiveSkillSet> SActiveSkill= new List<ActiveSkillSet>();

    //public static List<PassiveSkillSet> SPassiveSkill;
    //public static List<ActiveSkillSet> SActiveSkill;

    private PassiveSkillSet Passive_Money = new PassiveSkillSet("Money");

    private ActiveSkillSet Active_Fire = new ActiveSkillSet("Fire");
    private ActiveSkillSet Active_Ice = new ActiveSkillSet("Ice");
    private ActiveSkillSet Active_Electric = new ActiveSkillSet("Electric");
    private ActiveSkillSet Active_Obstacle = new ActiveSkillSet("Obstacle");

    private UserInformation userinfo = null;

    private void Awake()
    {

        userinfo = GetComponent<UserInformation>();

    }

    public void SkillSetUp(List<PassiveSkillSet> passiveSkillSets, List<ActiveSkillSet> activeSkillSets)
    {
        ASkillSetUp();

        SPassiveSkill = passiveSkillSets;
        SActiveSkill = activeSkillSets;
        ShowLevel();
        AShowLevel();
    }


    private IEnumerator ShowNotText(string text)
    {
        NotTextPanal.SetActive(true);
        NotText.text = text;
        yield return new WaitForSeconds(1.5f);
        NotTextPanal.SetActive(false);
    }

    #region ActiveSkill

    public void ALevelUpBtn()
    {
        ASkillLevelUP(_activeName);
        AShowLevel();
        AShowInfo(_activeName);
        userinfo.SaveASkill(SActiveSkill);
    }


    private void ASkillLevelUP(string skillname)
    {
        Debug.Log("레벨 업");
        for (int i = 0; i < SActiveSkill.Count; i++)
        {
            for (int j = 0; j < SActiveSkill[i].skillInfoList.Count; j++)
            {
                if (SActiveSkill[i].skillInfoList[j].SkillName == skillname)
                {
                    if (
                        (SActiveSkill[i].skillInfoList[j].PreSkill == "NULL"
                        || ASearchSkill(SActiveSkill[i].skillInfoList[j].PreSkill).UnLock == 1) &&
                        userinfo.userData.userCoin >= SActiveSkill[i].skillInfoList[j].Price)
                    {
                        if (SActiveSkill[i].skillInfoList[j].CurrentLevel < SActiveSkill[i].skillInfoList[j].MaxLevel)
                        {
                            userinfo.userData.userCoin -= SActiveSkill[i].skillInfoList[j].Price;

                            if (SActiveSkill[i].skillInfoList[j].GetCheckLock != 1)
                            {
                                SActiveSkill[i].skillInfoList[j].UnLockSkill();
                            }

                            SActiveSkill[i].skillInfoList[j].LevelUp();
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(ShowNotText("레벨이 Max입니다."));
                        }

                    }

                    else if (SActiveSkill[i].skillInfoList[j].PreSkill != "NULL" && SearchSkill(SActiveSkill[i].skillInfoList[j].PreSkill).UnLock == 0)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("선행스킬이 해제되지 않았습니다."));
                    }

                    else if (userinfo.userData.userCoin < SActiveSkill[i].skillInfoList[j].Price)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("코인이 부족합니다."));
                    }
                }
            }
        }
        ShowLevel();
    }


    public ActiveForm ASearchSkill(string _searchName)
    {
        for (int i = 0; i < SActiveSkill.Count; i++)
        {
            for (int j = 0; j < SActiveSkill[i].skillInfoList.Count; j++)
            {
                if (SActiveSkill[i].skillInfoList[j].SkillName == _searchName)
                {
                    return SActiveSkill[i].skillInfoList[j];
                }
            }
        }

        return null;
    }

    public static ActiveForm ActiveSkillSearch(string _skillname)
    {
        for (int i = 0; i < SActiveSkill.Count; i++)
        {
            for (int j = 0; j < SActiveSkill[i].skillInfoList.Count; j++)
            {
                if (SActiveSkill[i].skillInfoList[j].SkillName == _skillname)
                {
                    return SActiveSkill[i].skillInfoList[j];
                }
            }
        }
        return null;
    }

    public void ChangeSkillSlotNum(int num,string skillname)
    {
        ASearchSkill(skillname).Slot = num;
    }


    public void AShowInfo(string _searchname)
    {
        Debug.Log(_searchname);
        _activeName = _searchname;

        Askillname.text = _searchname;

        AInfotext.text =
            ASearchSkill(_searchname).SkillInformation + "\n" +
            "현재 수치 : " + ASearchSkill(_searchname).Value + "%\n" +
            "다음 업그레이드 수치 : " + (ASearchSkill(_searchname).Value + ASearchSkill(_searchname).UpValueRate) + "\n" +
            "현재 쿨타임 : " + (ASearchSkill(_searchname).CoolTime) + "초\n" +
            "다음 쿨타임 수치 : " + ((ASearchSkill(_searchname).CoolTime) - (ASearchSkill(_searchname).CoolTimeDown)) + "초";

        APricetext.text = "비용 : " + ASearchSkill(_searchname).Price.ToString() + "원";

        if (ASearchSkill(_searchname).UnLock == 0)
        {
            AButtonText.text = "스킬 해제";
        }
        else
        {
            if (ASearchSkill(_searchname).CurrentLevel < ASearchSkill(_searchname).MaxLevel)
            {
                AButtonText.text = "업그레이드";
            }
            else
            {
               AButtonText.text = "Max 레벨";
            }
        }
    }

    private void AShowLevel()
    {
       
       // for (int i = 0; i < SActiveSkill.Count; i++)
       // {
            for (int j = 0; j < SActiveSkill[0].skillInfoList.Count; j++)
            {

                FireBundle[j].level.text = ASearchSkill(FireBundle[ j].skillname).CurrentLevel.ToString() + "/" + ASearchSkill(FireBundle[ j].skillname).MaxLevel.ToString();
                
            }
        //}
    }

    #endregion


    #region PassiveSkill
    public void PSkillSetUp()
    {
        for (int i = 0; i < passiveBundle.PassiveSkill.Count; i++)
        {
            if (passiveBundle.PassiveSkill[i].BundleName == "Money")
            {
                Passive_Money.skillInfoList.Add(passiveBundle.PassiveSkill[i]);
            }
        }


        SPassiveSkill.Add(Passive_Money);
    }

    public void ASkillSetUp()
    {
        for (int i = 0; i < ActiveBundle.ActiveSkill.Count; i++)
        {
            switch (ActiveBundle.ActiveSkill[i].BundleName)
            {
                case "Fire":
                    Active_Fire.skillInfoList.Add(ActiveBundle.ActiveSkill[i]);
                    break;
                case "Ice":
                    Active_Ice.skillInfoList.Add(ActiveBundle.ActiveSkill[i]);
                    break;
                case "Electric":
                    Active_Electric.skillInfoList.Add(ActiveBundle.ActiveSkill[i]);
                    break;
                case "Obstacle":
                    Active_Obstacle.skillInfoList.Add(ActiveBundle.ActiveSkill[i]);
                    break;
            }
        }

        SActiveSkill.Add(Active_Fire);
        SActiveSkill.Add(Active_Ice);
        SActiveSkill.Add(Active_Electric);
        SActiveSkill.Add(Active_Obstacle);

        //Debug.Log(SActiveSkill.Count);

    }



    public static float PassiveValue(string _skillname)
    {
        for (int i = 0; i < SPassiveSkill.Count; i++)
        {
            for (int j = 0; j < SPassiveSkill[i].skillInfoList.Count; j++)
            {
                if (SPassiveSkill[i].skillInfoList[j].SkillName == _skillname)
                {
                    return SPassiveSkill[i].skillInfoList[j].Value;
                }
            }
        }
        return 0;
    }

    private string _upskillname;
    private string _activeName;

    public void PLevelUpBtn()
    {
        PSkillUnLock(_upskillname);
        ShowLevel();
        ShowInfo(_upskillname);
        userinfo.SavePSkill(SPassiveSkill);
    }

    public void PSkillUnLock(string skillname)
    {
        for(int i = 0; i < SPassiveSkill.Count; i++)
        {
            for(int j = 0; j < SPassiveSkill[i].skillInfoList.Count; j++)
            {
                if(SPassiveSkill[i].skillInfoList[j].SkillName == skillname)
                {
                    if(
                        (SPassiveSkill[i].skillInfoList[j].PreSkill == "NULL" 
                        || SearchSkill(SPassiveSkill[i].skillInfoList[j].PreSkill).UnLock == 1) &&
                        userinfo.userData.userCoin >= SPassiveSkill[i].skillInfoList[j].Price)
                    {
                        if (SPassiveSkill[i].skillInfoList[j].CurrentLevel < SPassiveSkill[i].skillInfoList[j].MaxLevel)
                        {
                            userinfo.userData.userCoin -= SPassiveSkill[i].skillInfoList[j].Price;

                            if (SPassiveSkill[i].skillInfoList[j].GetCheckLock != 1)
                            {
                                SPassiveSkill[i].skillInfoList[j].UnLockSkill();
                            }

                            SPassiveSkill[i].skillInfoList[j].LevelUp();
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(ShowNotText("레벨이 Max입니다."));
                        }

                    }

                    else if(SPassiveSkill[i].skillInfoList[j].PreSkill != "NULL" &&SearchSkill(SPassiveSkill[i].skillInfoList[j].PreSkill).UnLock == 0)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("선행스킬이 해제되지 않았습니다."));
                    }

                    else if(userinfo.userData.userCoin < SPassiveSkill[i].skillInfoList[j].Price)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("코인이 부족합니다."));
                    }
                }
            }
        }
        ShowLevel();
    }

    public PassiveForm SearchSkill(string _searchName)
    {
        for (int i = 0; i < SPassiveSkill.Count; i++)
        {
            for (int j = 0; j < SPassiveSkill[i].skillInfoList.Count; j++)
            {
                if (SPassiveSkill[i].skillInfoList[j].SkillName == _searchName)
                {
                    return SPassiveSkill[i].skillInfoList[j];
                }
            }
        }

        return null;
    }

    #endregion

    public void ShowInfo(string _searchname)
    {
        _upskillname = _searchname;

        skillname.text = _searchname;

        Infotext.text =
            SearchSkill(_searchname).SkillInformation + "\n\n" +
            "현재 수치 : " + SearchSkill(_searchname).Value * 100 + "%\n" +
            "다음 업그레이드 수치 : " + (SearchSkill(_searchname).Value + SearchSkill(_searchname).UpValueRate) * 100 + "%";

        Pricetext.text = "비용 : " + SearchSkill(_searchname).Price.ToString() + "원";
        
        if(SearchSkill(_searchname).UnLock == 0)
        {
            ButtonText.text = "스킬 해제";
        }
        else
        {
            if(SearchSkill(_searchname).CurrentLevel< SearchSkill(_searchname).MaxLevel)
            {
                ButtonText.text = "업그레이드";
            }
            else
            {
                ButtonText.text = "Max 레벨";
            }
        }
    }

    private void ShowLevel()
    {
        for(int i = 0; i < SPassiveSkill.Count; i++)
        {
            for(int j = 0; j < SPassiveSkill[i].skillInfoList.Count; j++)
            {
                MoneyBundle[i+j].level.text = SearchSkill(MoneyBundle[i + j].skillname).CurrentLevel.ToString() + "/" + SearchSkill(MoneyBundle[i + j].skillname).MaxLevel.ToString();
            }
        }
    }


    private void ResetBtn()
    {

        for (int i = 0; i < SPassiveSkill.Count; i++)
        {
            for (int j = 0; j < SPassiveSkill[i].skillInfoList.Count; j++)
            {

                SPassiveSkill[i].skillInfoList[j].CurrentLevel = 0;

                SPassiveSkill[i].skillInfoList[j].UnLock = 0;

            }
        }
    }



}
