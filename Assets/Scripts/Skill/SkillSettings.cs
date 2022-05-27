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
    private string BundleName;
    public string GetName => BundleName;

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

    [SerializeField] private List<PassiveSkillSet> PassiveSkill;
    [SerializeField] private List<ActiveSkillSet> ActiveSkill;

    public static List<PassiveSkillSet> UseSkillInfo;
    public static List<ActiveSkillSet> UseActiveSkill;

    private PassiveSkillSet Passive_Money = new PassiveSkillSet("Money");
    private ActiveSkillSet Active_Fire = new ActiveSkillSet("Fire");

    private void Awake()
    {
        PSkillSetUp();
        ASkillSetUp();

        //ResetBtn();

        ShowLevel();
        AShowLevel();
    }

    private void OnDestroy()
    {
        UseSkillInfo = PassiveSkill;
        UseActiveSkill = ActiveSkill;
    }

    private IEnumerator ShowNotText(string text)
    {
        NotTextPanal.SetActive(true);
        NotText.text = text;
        yield return new WaitForSeconds(1.5f);
        NotTextPanal.SetActive(false);
    }

    private void ASkillSetUp()
    {
        for (int i = 0; i < ActiveBundle.ActiveSkill.Count; i++)
        {
            if (ActiveBundle.ActiveSkill[i].BundleName == "Fire")
            {
                Active_Fire.skillInfoList.Add(ActiveBundle.ActiveSkill[i]);
            }
        }
        ActiveSkill.Add(Active_Fire);
    }

    public ActiveForm ASearchSkill(string _searchName)
    {
        for (int i = 0; i < ActiveSkill.Count; i++)
        {
            for (int j = 0; j < ActiveSkill[i].skillInfoList.Count; j++)
            {
                if (ActiveSkill[i].skillInfoList[j].SkillName == _searchName)
                {
                    return ActiveSkill[i].skillInfoList[j];
                }
            }
        }

        return null;
    }

    public static ActiveForm ActiveSkillSearch(string _skillname)
    {
        for (int i = 0; i < UseActiveSkill.Count; i++)
        {
            for (int j = 0; j < UseActiveSkill[i].skillInfoList.Count; j++)
            {
                if (UseActiveSkill[i].skillInfoList[j].SkillName == _skillname)
                {
                    return UseActiveSkill[i].skillInfoList[j];
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
        for (int i = 0; i < ActiveSkill.Count; i++)
        {
            for (int j = 0; j < ActiveSkill[i].skillInfoList.Count; j++)
            {

                FireBundle[i + j].level.text = ASearchSkill(FireBundle[i + j].skillname).CurrentLevel.ToString() + "/" + ASearchSkill(FireBundle[i + j].skillname).MaxLevel.ToString();
                
            }
        }
    }










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

        PassiveSkill.Add(Passive_Money);
    }

    public static float PassiveValue(string _skillname)
    {
        for (int i = 0; i < UseSkillInfo.Count; i++)
        {
            for (int j = 0; j < UseSkillInfo[i].skillInfoList.Count; j++)
            {
                if (UseSkillInfo[i].skillInfoList[j].SkillName == _skillname)
                {
                    return UseSkillInfo[i].skillInfoList[j].Value;
                }
            }
        }
        return 0;
    }

    private string _upskillname;
    private string _activeName;

    public void LevelUpBtn()
    {
        PSkillUnLock(_upskillname);
        ShowLevel();
        ShowInfo(_upskillname);
    }

    public void PSkillUnLock(string skillname)
    {
        for(int i = 0; i < PassiveSkill.Count; i++)
        {
            for(int j = 0; j < PassiveSkill[i].skillInfoList.Count; j++)
            {
                if(PassiveSkill[i].skillInfoList[j].SkillName == skillname)
                {
                    if(
                        (PassiveSkill[i].skillInfoList[j].PreSkill == "NULL" 
                        || SearchSkill(PassiveSkill[i].skillInfoList[j].PreSkill).UnLock == 1) && 
                        UserInformation.userDataStatic.userCoin >= PassiveSkill[i].skillInfoList[j].Price)
                    {
                        if (PassiveSkill[i].skillInfoList[j].CurrentLevel < PassiveSkill[i].skillInfoList[j].MaxLevel)
                        {
                            UserInformation.userDataStatic.userCoin -= PassiveSkill[i].skillInfoList[j].Price;

                            if (PassiveSkill[i].skillInfoList[j].GetCheckLock != 1)
                            {
                                PassiveSkill[i].skillInfoList[j].UnLockSkill();
                            }

                            PassiveSkill[i].skillInfoList[j].LevelUp();
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(ShowNotText("레벨이 Max입니다."));
                        }

                    }

                    else if(PassiveSkill[i].skillInfoList[j].PreSkill != "NULL" &&SearchSkill(PassiveSkill[i].skillInfoList[j].PreSkill).UnLock == 0)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("선행스킬이 해제되지 않았습니다."));
                    }

                    else if(UserInformation.userDataStatic.userCoin < PassiveSkill[i].skillInfoList[j].Price)
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
        for (int i = 0; i < PassiveSkill.Count; i++)
        {
            for (int j = 0; j < PassiveSkill[i].skillInfoList.Count; j++)
            {
                if (PassiveSkill[i].skillInfoList[j].SkillName == _searchName)
                {
                    return PassiveSkill[i].skillInfoList[j];
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
        for(int i = 0; i < PassiveSkill.Count; i++)
        {
            for(int j = 0; j < PassiveSkill[i].skillInfoList.Count; j++)
            {
                MoneyBundle[i+j].level.text = SearchSkill(MoneyBundle[i + j].skillname).CurrentLevel.ToString() + "/" + SearchSkill(MoneyBundle[i + j].skillname).MaxLevel.ToString();
            }
        }
    }


    private void ResetBtn()
    {

        for (int i = 0; i < PassiveSkill.Count; i++)
        {
            for (int j = 0; j < PassiveSkill[i].skillInfoList.Count; j++)
            {

                PassiveSkill[i].skillInfoList[j].CurrentLevel = 0;

                PassiveSkill[i].skillInfoList[j].UnLock = 0;

            }
        }
    }



}
