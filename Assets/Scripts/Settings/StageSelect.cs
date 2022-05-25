using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StageSelect : MonoBehaviour
{
    [SerializeField] private GameObject levelChoice = null;

    [SerializeField] private TextMeshProUGUI easyMoney = null;
    [SerializeField] private TextMeshProUGUI namalMoney = null;
    [SerializeField] private TextMeshProUGUI hardMoney = null;

    private string stageShape = null;

    int easymoney = 0;
    int nomalmoney = 0;
    int hardmoney = 0;

    public void StageShape(string _shape)
    {
        stageShape = _shape;
    }


    public void SelectStage(int level)
    {
        GameManager.SetStageShape = stageShape;
        GameManager.SetGameLevel = level;

        switch (level)
        {
            case 1:
                GameManager.SetMoney = easymoney;
                break;
            case 2:
                GameManager.SetMoney = nomalmoney;
                break;
            case 3:
                GameManager.SetMoney = hardmoney;
                break;
        }
    }

    public void OnLevelChoice()
    {
        SetMoney();
        levelChoice.SetActive(true);
    }

    public void SetMoney()
    {
        switch (stageShape)
        {
            case "Diamond":
                PrintMoney(100);
                break;

            case "PlusMap":
                PrintMoney(120);
                break;

            case "WellMap":
                PrintMoney(120);
                break;

            case "WindowMap":
                PrintMoney(140);
                break;

            case "Xshape":
                PrintMoney(160);
                break;

            case "Center":
                PrintMoney(180);
                break;

            case "DotMap":
                PrintMoney(180);
                break;
        }
    }

    private void PrintMoney(int money)
    {
        Debug.Log("money");
        easymoney = money;
        nomalmoney = (money + (money / 2) + (money / 4));
        hardmoney = money * 3;

        easyMoney.text = "보상 : "+ money+"원";
        namalMoney.text = "보상 : " + nomalmoney + "원";
        hardMoney.text = "보상 : " + hardmoney + "원";   
    }

    public void OffLevelChoice()
    {
        levelChoice.SetActive(false);
        stageShape = null;
    }

}


