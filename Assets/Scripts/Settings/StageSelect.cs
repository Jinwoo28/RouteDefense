using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum MapShape
{
    Diamond=1,
    Plus,
    Well,
    Window,
    Xshape,
    Center,
    Dot
}

public class StageSelect : MonoBehaviour
{
    [SerializeField] private GameObject levelChoice = null;
    [SerializeField] private GameObject GameType = null;

    [SerializeField] private TextMeshProUGUI easyMoney = null;
    [SerializeField] private TextMeshProUGUI namalMoney = null;
    [SerializeField] private TextMeshProUGUI hardMoney = null;

    [SerializeField] private TextMeshProUGUI InfoLevel1 = null;
    [SerializeField] private TextMeshProUGUI InfoLevel2  = null;
    [SerializeField] private TextMeshProUGUI InfoLevel3 = null;

    [SerializeField] private GameObject GOeasyMoney = null;
    [SerializeField] private GameObject GOnamalMoney = null;
    [SerializeField] private GameObject GOhardMoney = null;
    [SerializeField] private SetStageEnemyData SSED;

    MapShape MS;
    StageDataSetUp SDSU = new StageDataSetUp();
    private string stageShape = null;

    private int easymoney = 0;
    private int nomalmoney = 0;
    private int hardmoney = 0;

    private AlertSetting alter = new AlertSetting();

    public void StageShape(int name)
    {
        MS = (MapShape)name;
        stageShape = MS.ToString();
        GameManager.SetStageData = SDSU.GetStageInfo(name, SSED);
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
    private string StageName;

    public void StartGame()
    {
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

        switch (GameManager.GetSetStageType)
        {
            case StageType.Nomal:
                InfoLevel1.text = "난이도 하";
                InfoLevel2.text = "난이도 중";
                InfoLevel3.text = "난이도 상";
                GOeasyMoney.SetActive(true); 
                GOnamalMoney.SetActive(true);
                GOhardMoney.SetActive(true);
                GOeasyMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400,21);
                GOnamalMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 21);
                GOhardMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 21);
                break;
            case StageType.UnOrderCheckPoint:
                InfoLevel1.text = "체크 포인트 2개";
                InfoLevel2.text = "체크 포인트 3개";
                InfoLevel3.text = "체크 포인트 4개";
                GOeasyMoney.SetActive(true);
                GOnamalMoney.SetActive(true);
                GOhardMoney.SetActive(true);
                GOeasyMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400, 21);
                GOnamalMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 21);
                GOhardMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 21);
                break;
            case StageType.OrderCheckPoint:
                InfoLevel1.text = "체크 포인트 2개";
                InfoLevel2.text = "체크 포인트 3개";
                GOeasyMoney.SetActive(true);
                GOnamalMoney.SetActive(true);
                GOhardMoney.SetActive(false);
                GOeasyMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 21);
                GOnamalMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 21);
                GOhardMoney.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 21);
                break;

        }
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

    public void PlayOnBtnSound()
    {
        alter.PlaySound(AlertKind.Click, this.gameObject);
    }

}


