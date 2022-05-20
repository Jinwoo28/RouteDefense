using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class SkillKind
{
    public GameObject Skill = null;
    public string Skillname = null;
    public float skillCoolTime = 0;
    public bool CanUse = true;
}

public class SkillFunc : MonoBehaviour
{

    [SerializeField] private LayerMask layermask;

    [SerializeField] private Button[] skillButton = null;
    [SerializeField] private GameObject[] skillButtonObject = null;

    //스킬 사용시 범위를 나타낼 오브젝트
    [SerializeField] private GameObject SetSkillPos = null;
    private GameObject setskillpos = null;

    private Vector3 targetpos;

    [SerializeField] private SkillKind[] skillKind = null;


    [SerializeField] private TextMeshProUGUI fireCooltime = null;
    [SerializeField] private TextMeshProUGUI meteorCooltime = null;

    public enum skill
    {
        Fire =1,
        Meteor,
    }
    skill skillnum = 0;

    //스킬이 활성화 되어있는지 확인
    private bool skillActive = false;

    DetectObject detectObject = new DetectObject();



    private void Start()
    {
        GameManager.buttonOff += OffSkillSet;


        MultipleSpeed.speedup += SpeedUP;

        fireCooltime.enabled = false;
        meteorCooltime.enabled = false;


            //if (UserInformation.userDataStatic.skillSet[0].skillUnLock)
            //{
            //    skillButtonObject[0].SetActive(true);
            //}
            //if (UserInformation.userDataStatic.skillSet[1].skillUnLock)
            //{
            //    skillButtonObject[1].SetActive(true);
            //}
        


    }

    public void OffSkillSet()
    {
        if (setskillpos != null)
        {
            Destroy(setskillpos);
            skillActive = false;
        }
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

    bool canSetSkill = false;

    private void Update()
    {


        if (skillActive)
        {
            if (detectObject.ReturnTransform(layermask) != null)
            {
                targetpos = new Vector3((int)detectObject.ReturnTransform(layermask).position.x,
               detectObject.ReturnTransform(layermask).GetComponent<Node>().GetYDepth / 2,
               (int)detectObject.ReturnTransform(layermask).position.z);


                if (!detectObject.ReturnTransform(layermask).GetComponent<Node>().GetOnTower)
                {
                    setskillpos.transform.position = targetpos;
                }
               
                

            }



        

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                skillActive = false;
                Destroy(setskillpos);
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButton(0))
                {
                    switch (skillnum)
                    {
                        case skill.Fire:
                            if (skillKind[0].CanUse)
                            {
                                FireSkill();
                            }
                            break;
                        case skill.Meteor:
                            if (skillKind[1].CanUse)
                            {
                                MeteorSkill();
                            }
                            break;
                    }
                }
            }
        }

    }

    public void ClickSkill(int _skilltype)
    {
        if (setskillpos != null)
        {
            Destroy(setskillpos);
        }

        MapManager.OffFunc();
        switch (_skilltype)
        {
            case 1:
                if (skillKind[0].CanUse)
                {
                    skillnum = skill.Fire;
                    setskillpos = Instantiate(SetSkillPos,Vector3.zero,Quaternion.Euler(90,0,0));
                    skillActive = true;
                }
                break;
            case 2:
                if (skillKind[1].CanUse)
                {
                    skillnum = skill.Meteor;
                    setskillpos = Instantiate(SetSkillPos, Vector3.zero, Quaternion.Euler(90, 0, 0));
                    skillActive = true;
                }
                break;
        }


        
    }

    private void FireSkill()
    {
        Instantiate(skillKind[0].Skill, targetpos, Quaternion.identity);

        SkillSetUp(skillKind[0], skillButton[0],fireCooltime);

    }

    private void MeteorSkill()
    {
        Vector3 spawnPos = new Vector3(targetpos.x + 3.0f, targetpos.y + 10.0f, targetpos.z);
        var meteor = Instantiate(skillKind[1].Skill, spawnPos, Quaternion.identity);
        meteor.GetComponent<MeteorSkill>().SetTarget = targetpos;
        SkillSetUp(skillKind[1], skillButton[1],meteorCooltime);
    }

    private void SkillSetUp(SkillKind _skillkind,Button _button,TextMeshProUGUI _showcooltime)
    {
        skillActive = false;
        skillnum = 0;
        _skillkind.CanUse = false;
        _button.image.fillAmount = 0;
        _button.enabled = false;
        StartCoroutine(SkillCoolTime(_button, _skillkind.skillCoolTime, _skillkind, _showcooltime));
        Destroy(setskillpos);
    }



    IEnumerator SkillCoolTime(Button _button,float _cooltime,SkillKind _skill,TextMeshProUGUI _showCooltime)
    {
        float cooltime = 0;
        _showCooltime.enabled = true;
        while (true)
        {
            cooltime += Time.deltaTime;
            _button.image.fillAmount += Time.deltaTime / _cooltime;
            _showCooltime.text = (_cooltime - (int)cooltime).ToString();

            if (cooltime >= _skill.skillCoolTime)
            {
                _button.enabled = true;
                _skill.CanUse = true;
                _showCooltime.enabled = false;
                break;
            }
            yield return null;
        }
    }


   

}
