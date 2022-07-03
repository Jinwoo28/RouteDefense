using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StageSelect : MonoBehaviour
{
    [SerializeField] private GameObject levelChoice = null;
    [SerializeField] private GameObject GameType = null;

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

        LoadSceneControler.LoadScene("StageTest");
    }

    public void ToLobby()
    {
        LoadSceneControler.LoadScene("Lobby");
    }

    public void OnLevelChoice()
    {
        //SetMoney();
        GameType.SetActive(true);
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
        float multinum = 0;
        switch (GameManager.GetSetStageType)
        {
            case StageType.Nomal:
                multinum = 1;
                break;
            case StageType.UnOrderCheckPoint:
                multinum = 1.5f;
                break;
            case StageType.OrderCheckPoint:
                multinum = 2;
                break;
        }

        easymoney = (int)(money * multinum);
        nomalmoney = (int)((money + (money / 2) + (money / 4))*multinum);
        hardmoney = (int)(money * 3*multinum);

        easyMoney.text = "보상 : "+ easymoney + "원";
        namalMoney.text = "보상 : " + nomalmoney + "원";
        hardMoney.text = "보상 : " + hardmoney + "원";   
    }

    public void OffLevelChoice()
    {
        levelChoice.SetActive(false);
        stageShape = null;
    }

    public void OffGameType()
    {
        GameType.SetActive(false);
        GameManager.GetSetStageType = StageType.Nomal;
    }

    public void SelectGameType(int type)
    {

        GameManager.GetSetStageType = (StageType)type;
        SetMoney();

        GameType.SetActive(false);
        levelChoice.SetActive(true);
    }

}


