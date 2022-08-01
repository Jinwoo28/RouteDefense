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
    public string BundleName;
    //public string GetName => BundleName;

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
    [SerializeField] private List<SKillText> ShowActiveSkillText;

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

    private AlertSetting alter = new AlertSetting();

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
        Debug.Log("���� ��");
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
                            alter.PlaySound(AlertKind.OK,this.gameObject);
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(ShowNotText("������ Max�Դϴ�."));
                            alter.PlaySound(AlertKind.Cant, this.gameObject);
                        }

                    }

                    else if (SActiveSkill[i].skillInfoList[j].PreSkill != "NULL" && ActiveSkillSearch(SActiveSkill[i].skillInfoList[j].PreSkill).UnLock == 0)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("���ེų�� �������� �ʾҽ��ϴ�."));
                        alter.PlaySound(AlertKind.Cant, this.gameObject);
                    }

                    else if (userinfo.userData.userCoin < SActiveSkill[i].skillInfoList[j].Price)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("������ �����մϴ�."));
                        alter.PlaySound(AlertKind.Cant, this.gameObject);
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
            "���� ������ : " + ASearchSkill(_searchname).Value + "\n" +
            "���� ���׷��̵� ������ : " + (ASearchSkill(_searchname).Value + ASearchSkill(_searchname).UpValueRate) + "\n" +
            "���� ��Ÿ�� : " + (ASearchSkill(_searchname).CoolTime) + "��\n" +
            "���� ��Ÿ�� ��ġ : " + ((ASearchSkill(_searchname).CoolTime) - (ASearchSkill(_searchname).CoolTimeDown)) + "��";

        APricetext.text = "��� : " + ASearchSkill(_searchname).Price.ToString() + "��";

        if (ASearchSkill(_searchname).UnLock == 0)
        {
            AButtonText.text = "��ų ����";
        }
        else
        {
            if (ASearchSkill(_searchname).CurrentLevel < ASearchSkill(_searchname).MaxLevel)
            {
                AButtonText.text = "���׷��̵�";
            }
            else
            {
               AButtonText.text = "Max ����";
            }
        }
    }

    private void AShowLevel()
    {
        for(int j = 0; j< ShowActiveSkillText.Count; j++)
        {
            ShowActiveSkillText[j].level.text = ASearchSkill(ShowActiveSkillText[j].skillname).CurrentLevel.ToString() + "/" + ASearchSkill(ShowActiveSkillText[j].skillname).MaxLevel.ToString();
            //for (int j = 0; j < SActiveSkill.Count; j++)
            //{
            //    for(int k = 0; k < SActiveSkill[j].skillInfoList.Count)
            //    {
            //        if(ShowActiveSkillText[i].skillname == ASearchSkill())
            //    }
            //}
        }




        // for (int i = 0; i < SActiveSkill.Count; i++)
        // {
        //    for (int j = 0; j < SActiveSkill[i].skillInfoList.Count; j++)
        //    {
        //        ShowActiveSkillText[j].level.text = ASearchSkill(ShowActiveSkillText[ j].skillname).CurrentLevel.ToString() + "/" + ASearchSkill(ShowActiveSkillText[ j].skillname).MaxLevel.ToString();
        //        Debug.Log(ShowActiveSkillText[j].skillname);
        //    }
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
                            alter.PlaySound(AlertKind.OK, this.gameObject);
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(ShowNotText("������ Max�Դϴ�."));
                            alter.PlaySound(AlertKind.Cant, this.gameObject);
                        }

                    }

                    else if(SPassiveSkill[i].skillInfoList[j].PreSkill != "NULL" &&SearchSkill(SPassiveSkill[i].skillInfoList[j].PreSkill).UnLock == 0)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("���ེų�� �������� �ʾҽ��ϴ�."));
                        alter.PlaySound(AlertKind.Cant, this.gameObject);
                    }

                    else if(userinfo.userData.userCoin < SPassiveSkill[i].skillInfoList[j].Price)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ShowNotText("������ �����մϴ�."));
                        alter.PlaySound(AlertKind.Cant, this.gameObject);
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
            "���� ��ġ : " + SearchSkill(_searchname).Value * 100 + "%\n" +
            "���� ���׷��̵� ��ġ : " + (SearchSkill(_searchname).Value + SearchSkill(_searchname).UpValueRate) * 100 + "%";

        Pricetext.text = "��� : " + SearchSkill(_searchname).Price.ToString() + "��";
        
        if(SearchSkill(_searchname).UnLock == 0)
        {
            ButtonText.text = "��ų ����";
        }
        else
        {
            if(SearchSkill(_searchname).CurrentLevel< SearchSkill(_searchname).MaxLevel)
            {
                ButtonText.text = "���׷��̵�";
            }
            else
            {
                ButtonText.text = "Max ����";
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
