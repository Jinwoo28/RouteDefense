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

    private void Start()
    {
        skill = this.GetComponent<SkillSettings>();

       // PSkillSetUp();

        if (!SetData)
        {
            LoadUserInfo();
            SetData = true;
            skill.SkillSetUp(userData.PassiveSkill, userData.ActiveSkill);
     //       userDataStatic = userData;
        }

        userData.userCoin += getMoney;
        getMoney = 0;



       // userDataStatic.PassiveSkill.Add(P1skillBundle);
     //   userData.userCoin = userDataStatic.userCoin;
    }

    public void SavePSkill(List<PassiveSkillSet> passiveSkillSets)
    {
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
