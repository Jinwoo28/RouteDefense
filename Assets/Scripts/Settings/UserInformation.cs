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
    //inspectorâ���� ���� ������ �� �ֵ��� ���� ����
    public UserData userData;

    //userData�� �������� ����ϱ� ���� static���� ����� ����, �ҷ����� �� �� userData�� �ְ� ����
    //������ �����͸� ������ static����
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
            //ResetUserSkillData(); // ������ ����
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
        Debug.Log("��ų �ʱ�ȭ");
 
        //������ ����� ��ų ������ŭ �ݺ�
        foreach (var item in ActiveBundle.ActiveSkill)
        {
            bool success2 = false;
            //userdata�� ���� ����
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
        Debug.Log("����");
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

    //���� ������ ����
    public void SaveUserInfo()
    {


        //json�� ���ӵ� string, byte�� ����
        //userdata�� ���̽����� �����ϱ� ���� string���� ��ȯ
        string jdata = JsonConvert.SerializeObject(userData,Formatting.Indented);

        ////��ȣȭ �ϱ� ���� byte���·� ���� ��ȯ
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);

        ////��ȯ�� byte�� �ٽ� string���·� ��ȯ
        //string format = System.Convert.ToBase64String(bytes);



        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");

        //�׷��� ������ string������ ����
        File.WriteAllText(path, jdata);

        //File.WriteAllText(Application.streamingAssetsPath + "/Jinwoo", jdata);


        Debug.Log("����");
    }


    //���� ������ �ҷ�����
    public void LoadUserInfo()
    {

        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");
        string jdata = File.ReadAllText(path);

    //byte[] bytes = System.Convert.FromBase64String(jdata);
    //string reformat = System.Text.Encoding.UTF8.GetString(bytes);

//    https://forum.unity.com/threads/json-file-not-found-after-building.959523/

        userData = JsonConvert.DeserializeObject<UserData>(jdata);
       // Debug.Log("�ҷ�����");
    }

 


}
