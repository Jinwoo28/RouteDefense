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

    public List<PassiveSkillSet> PassiveSkill;
    
    public List<ActiveSkillSet> ActiveSkill;
    
}

public class UserInformation : MonoBehaviour
{
    //inspector창에서 쉽게 수정할 수 있도록 변수 생성
    public UserData userData;

    //userData를 전역에서 사용하기 위해 static으로 만들고 저장, 불러오기 할 때 userData를 주고 받음
    //씬에서 데이터를 저장할 static변수
    //public static UserData userDataStatic = new UserData();

    public static int getMoney = 0;


    private static bool SetData = false;

    private SkillSettings skill = null;

    [SerializeField] private ActiveBundle ActiveBundle = null;
    [SerializeField] private PassiveBundle passiveBundle = null;

    private void Start()
    {

        skill = this.GetComponent<SkillSettings>();

        // PSkillSetUp();
        //ResetUserSkillData();

            LoadUserInfo();
           // SetData = true;
            //ResetUserSkillData(); // 보유돈 리셋
            skill.SkillSetUp(userData.PassiveSkill, userData.ActiveSkill);


        userData.userCoin += getMoney;
        getMoney = 0;

        //if (Path.Combine(Application.streamingAssetsPath, "PlayerData.json") == null)
        //{
        //    File.Create(Path.Combine(Application.streamingAssetsPath, "PlayerData.json"));
        //}
    }

    private void ResetUserSkillData()
    {
        Debug.Log("스킬 초기화");
 
        //엑셀에 저장된 스킬 개수만큼 반복
        foreach (var item in ActiveBundle.ActiveSkill)
        {
            bool success2 = false;
            //userdata의 번들 개수
            for (int i = 0; i < userData.ActiveSkill.Count; i++)
            {
                if (item.BundleName == userData.ActiveSkill[i].BundleName)
                {
                    bool success = false;
                    for(int j = 0; j < userData.ActiveSkill[i].skillInfoList.Count; j++)
                    {
                        if (item.SkillName == userData.ActiveSkill[i].skillInfoList[j].SkillName)
                        {
                            userData.ActiveSkill[i].skillInfoList[j] = item;
                            success = true;
                            break;
                        }
                    }

                    if (!success)
                    {
                        userData.ActiveSkill[i].skillInfoList.Add(item);
                    }
                    success2 = true;
                }
            }
                if (!success2)
                {
                    ActiveSkillSet newSet = new ActiveSkillSet(item.BundleName);
                    newSet.skillInfoList.Add(item);
                    userData.ActiveSkill.Add(newSet);
                }

        }

        foreach (var item in passiveBundle.PassiveSkill)
        {
            bool success2 = false;
            for (int i = 0; i < userData.PassiveSkill.Count; i++)
            {
                if (item.BundleName == userData.PassiveSkill[i].BundleName)
                {
                    bool success = false;
                    for (int j = 0; j < userData.PassiveSkill[i].skillInfoList.Count; j++)
                    {
                        if (item.SkillName == userData.PassiveSkill[i].skillInfoList[j].SkillName)
                        {
                            userData.PassiveSkill[i].skillInfoList[j] = item;
                            success = true;
                            break;
                        }
                    }

                    if (!success)
                    {
                        userData.PassiveSkill[i].skillInfoList.Add(item);
                    }
                    success2 = true;
                }
            }
            if (!success2)
            {
                PassiveSkillSet newSet = new PassiveSkillSet(item.BundleName);
                newSet.skillInfoList.Add(item);
                userData.PassiveSkill.Add(newSet);
            }

        }
    }

    public void SavePSkill(List<PassiveSkillSet> passiveSkillSets)
    {
        Debug.Log("저장");
        userData.PassiveSkill = passiveSkillSets;
    }

    public void SaveASkill(List<ActiveSkillSet> activeSkillSets)
    {
        userData.ActiveSkill = activeSkillSets;
    }

    private void OnDestroy()
    {
     //   userData = userDataStatic;
        SaveUserInfo();
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



        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");

        //그렇게 생성된 string형식을 저장
        File.WriteAllText(path, jdata);

        //File.WriteAllText(Application.streamingAssetsPath + "/Jinwoo", jdata);


        Debug.Log("저장");
    }


    //유저 데이터 불러오기
    public void LoadUserInfo()
    {

        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");
        string jdata = File.ReadAllText(path);

    //byte[] bytes = System.Convert.FromBase64String(jdata);
    //string reformat = System.Text.Encoding.UTF8.GetString(bytes);

//    https://forum.unity.com/threads/json-file-not-found-after-building.959523/

        userData = JsonConvert.DeserializeObject<UserData>(jdata);
       // Debug.Log("불러오기");
    }

 


}
