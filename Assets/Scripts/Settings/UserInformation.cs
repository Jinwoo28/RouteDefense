using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;


[System.Serializable]
public class UserData
{
    public int userCoin = 0;

    public List<SkillInfo> skillSet;

    public List<SkillSet> PassiveSkill;

    public List<SkillSet> ActiveSkill;

    public void UnLockSkill(string skillname)
    {
        foreach (var skill in skillSet)
        {
            if (skill.GetName == skillname)
            {
                if (skill.SetUnLock == false)
                {
                    if (skill.GetPrice <= userCoin)
                    {
                        skill.SetUnLock = true;
                        userCoin -= skill.GetPrice;
                    }
                }
            }
        }
    }

    public void SkillLevelUp(string skillname)
    {
        foreach (var skill in skillSet)
        {
            if (skill.GetName == skillname)
            {
                if (skill.SetUnLock == true)
                {
                    if (skill.GetPrice <= userCoin)
                    {
                        userCoin -= skill.GetPrice;

                        skill.SkillLevelUp();

                    }
                }
            }
        }
    }
}

[System.Serializable]
public class SkillSet
{
    public string BundleName;
    public List<SkillInfo> skillInfoList;
}

//Dictionary는 시리얼라이즈화 시킬 수 없기 때문에 따로 class를 만들어서 list로 묶어서 저장
[System.Serializable]
public class SkillInfo
{
    public enum skillType
    {
        Active,
        Passive
    }

    //스킬 타입
    [SerializeField]
    private skillType skilltype;

    public skillType GetType => skilltype;

    //스킬 이름
    [SerializeField]
    private string skillName;

    public string GetName => skillName;

    [SerializeField]
    private int skillprice;
    public int GetPrice => skillprice;

    //스킬의 해제 여부
    [SerializeField]
    private bool skillUnLock;

    public bool SetUnLock { get => skillUnLock; set => skillUnLock = value; }

    [SerializeField]
    private string preskill;

    [SerializeField]
    private int skillcooltime;
    public int GetCoolTime => skillcooltime;

    [SerializeField]
    private int cooltimeDecrease;

    [SerializeField]
    private int damage;
    public int GetDamage => damage;

    [SerializeField]
    private int damageIncrease;

    [SerializeField]
    private int skillLevel;
    public int GetLevel => skillLevel;

    [SerializeField]
    private int MaxSkillLevel;
    public int GetMaxLevel => MaxSkillLevel;

    [SerializeField]
    private int upgradeprice;
    [SerializeField]
    private int OriginUpgradePrice;

    public void SkillLevelUp()
    {
        skillLevel++;
        upgradeprice = skillLevel * OriginUpgradePrice;
        skillcooltime -= cooltimeDecrease;
        damage += damageIncrease;
    }
}

public class UserInformation : MonoBehaviour
{

    //inspector창에서 쉽게 수정할 수 있도록 변수 생성
    public UserData userData;



    //userData를 전역에서 사용하기 위해 static으로 만들고 저장, 불러오기 할 때 userData를 주고 받음
    //씬에서 데이터를 저장할 static변수
    public static UserData userDataStatic = new UserData();

    private static bool SetData = false;

    private SkillSettings skill = null;

    private void Start()
    {
        skill = this.GetComponent<SkillSettings>();

        if (!SetData)
        {
            LoadUserInfo();
            SetData = true;
            userDataStatic = userData;
          //Debug.Log(userDataStatic.skillSet[0].skillUnLock);
        }

        userData.userCoin = userDataStatic.userCoin;

    }

    private void OnDestroy()
    {
        userData.skillSet = userDataStatic.skillSet;
        SaveUserInfo();
    }

    //생성자를 private으로 만들어서 새로운 객체 생성막기
    private UserInformation() { }


    public void UnlockSkill(string _skillname)
    {
        userData.UnLockSkill(_skillname);
        
    }

    public void SwitchInfoForward()
    {
        userData = userDataStatic;
    }

    public void SwitchInfoReverse()
    {
        userDataStatic = userData;
    }
    


    //유저 데이터 저장
    public void SaveUserInfo()
    {
        //json은 연속된 string, byte만 가능
        //userdata를 제이슨으로 저장하기 위해 string으로 변환
        string jdata = JsonConvert.SerializeObject(userData,Formatting.Indented);

        ////암호화 하기 위해 byte형태로 랜덤 변환
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);

        ////변환된 byte를 다시 string형태로 변환
        //string format = System.Convert.ToBase64String(bytes);

        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.txt");

        //그렇게 생성된 string형식을 저장
        File.WriteAllText(path, jdata);

        //File.WriteAllText(Application.streamingAssetsPath + "/Jinwoo", jdata);


        Debug.Log("저장");
    }


    //유저 데이터 불러오기
    public void LoadUserInfo()
    {

        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.txt");
        string jdata = File.ReadAllText(path);

    //byte[] bytes = System.Convert.FromBase64String(jdata);
    //string reformat = System.Text.Encoding.UTF8.GetString(bytes);

//    https://forum.unity.com/threads/json-file-not-found-after-building.959523/

        userData = JsonConvert.DeserializeObject<UserData>(jdata);
        Debug.Log("불러오기");
    }

 


}
