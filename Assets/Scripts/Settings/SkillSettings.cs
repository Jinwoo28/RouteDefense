using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSettings : MonoBehaviour
{
    public enum skillType
    {
        Active,
        Passive
    }

    //스킬 타입
    public skillType skilltype;
    

    //스킬 이름
    public string skillName;

    public int skillprice;

    //스킬의 해제 여부
    public bool skillUnLock;

    public int skillcooltime;
    public int cooltimeDecrease;

    public int damage;
    public int damageIncrease;

    public int skillLevel;
    public int MaxSkillLevel;

    public int upgradeprice;
    public int OriginUpgradePrice;

    public void SkillLevelUp()
    {
        skillLevel++;
        upgradeprice = skillLevel * OriginUpgradePrice;
        skillcooltime -= cooltimeDecrease;
        damage += damageIncrease;
    }

}
