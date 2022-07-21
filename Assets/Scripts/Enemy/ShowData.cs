using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI informationText;
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
        EnemyDataFrame stat = EnemyDataSetUp.GetData(enemycode);
        string type = stat.enemytype == 0 ? "지상" : "공중";
        informationText.text = $"이름 : {stat.name}\n 종족 : 동물 \n 체력 : {stat.hp}\t스피드 : {stat.speed}\t아머 : {stat.amour}\n" +
            $"코인 : ${stat.coin}\t회피율 : {stat.avoidance}%\n유닛타입 : {type}\n종족특징 : 3초마다 {stat.feature}의 체력을 회복";
    }

    public void ShowTowerData(int towercode)
    {
        TowerDataFrame towerdata = TowerDataSetUp.GetData(towercode);
        string AtkType;
        if(towerdata.atkType == 1)
        {
            AtkType = "지상";
        }
        else if(towerdata.atkType == 2)
        {
            AtkType = "공중";
        }
        else
        {
            AtkType = "지상, 공중";
        }

        informationText.text = $"이름 : {towerdata.name}\t 공격대상 : {AtkType}\n" +
            $"레벨 : {towerdata.towerStep}\t가격 : {towerdata.towerPrice}\t데미지 : {towerdata.damage}\n" +
            $"공격속도 : {towerdata.delay}\t공격범위 : {towerdata.range}\t크리티컬 : {towerdata.critical * 100}%\n" +
            $"타워 특징 : {towerdata.detailInformation}\n\n" +
            $"업그레이드 수치\n" +
            $"공격력 +{towerdata.upgradAtk}\t크리티컬 +{towerdata.upgradCri*100}%\t업그레이드 가격 : {towerdata.upgradPrice}\n";
    }

    public void ShowEnemyPanel()
    {
        informationText.text = "대상을 선택하세요.";
        EnemyPanel.SetActive(true);
        TowerPanel.SetActive(false);
        EnemyPanelUi.color = Color.white;
        TowerPanelUi.color = Color.gray;
    }
    public void ShowTowerPanel()
    {
        informationText.text = "대상을 선택하세요.";
        EnemyPanel.SetActive(false);
        TowerPanel.SetActive(true);
        EnemyPanelUi.color = Color.gray;
        TowerPanelUi.color = Color.white;
    }

}
