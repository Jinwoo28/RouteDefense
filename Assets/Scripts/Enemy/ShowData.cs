using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowData : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI datatext;

    [SerializeField] private GameObject EnemyPanel;
    [SerializeField] private Image EnemyPanelUi;
    [SerializeField] private GameObject TowerPanel;
    [SerializeField] private Image TowerPanelUi;

    private void Start()
    {
        ShowEnemyPanel();
    }

    public void ShowEnemyData(int enemycode) 
    {
        EnemyState stat = EnemyStateSetUp.GetData(enemycode);
        string type = stat.enemytype == 0 ? "지상" : "공중";
        datatext.text = $"이름 : {stat.Name}\n 종족 : 동물 \n 체력 : {stat.Hp}\t스피드 : {stat.Speed}\t아머 : {stat.Amour}\n" +
            $"코인 : ${stat.coin}\t회피율 : {stat.avoidance}%\n유닛타입 : {type}\n종족특징 : 3초마다 {stat.feature}의 체력을 회복";
    }

    public void ShowTowerData(int towercode)
    {
        TowerData towerdata = TowerDataSetUp.GetData(towercode);
        string AtkType;
        if(towerdata.CanAtk == 1)
        {
            AtkType = "지상";
        }
        else if(towerdata.CanAtk == 2)
        {
            AtkType = "공중";
        }
        else
        {
            AtkType = "지상, 공중";
        }

        datatext.text = $"이름 : {towerdata.Name}\t 공격대상 : {AtkType}\n" +
            $"레벨 : {towerdata.TowerStep}\t가격 : {towerdata.TowerPrice}\t데미지 : {towerdata.Damage}\n" +
            $"공격속도 : {towerdata.Delay}\t공격범위 : {towerdata.Range}\t크리티컬 : {towerdata.Critical * 100}%\n" +
            $"타워 특징 : {towerdata.DetailInformation}\n\n" +
            $"업그레이드 수치\n" +
            $"공격력 +{towerdata.UpgradeAtk}\t크리티컬 +{towerdata.UpgradeCri*100}%\t업그레이드 가격 : {towerdata.UpgradePrice}\n";
    }

    public void ShowEnemyPanel()
    {
        datatext.text = "대상을 선택하세요.";
        EnemyPanel.SetActive(true);
        TowerPanel.SetActive(false);
        EnemyPanelUi.color = Color.white;
        TowerPanelUi.color = Color.gray;
    }
    public void ShowTowerPanel()
    {
        datatext.text = "대상을 선택하세요.";
        EnemyPanel.SetActive(false);
        TowerPanel.SetActive(true);
        EnemyPanelUi.color = Color.gray;
        TowerPanelUi.color = Color.white;
    }

}
