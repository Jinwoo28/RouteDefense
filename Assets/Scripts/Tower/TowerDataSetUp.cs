using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerDataSetUp : MonoBehaviour
{
    public SetTowerData TD;

    const string URL = "https://docs.google.com/spreadsheets/d/19oIy0vvxi2Xtfw5X8V8ZsdRhoujJgtXPAy7Qru5Sjv4/export?format=tsv&range=A2:N";

    public static TowerDataFrame[] towerDatas;

    private static int AlreadySet = 0;

    [SerializeField] private GameObject DarkImage = null;

    private void Awake()
    {
        DarkImage.SetActive(false);
        if (AlreadySet == 0)
        {
            DarkImage.SetActive(true);
            StartCoroutine(DataSetFromWeb());
            AlreadySet = 1;

        }

        
    }

    IEnumerator DataSetFromWeb()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        print(data);

        SetEnemy(data);
        DarkImage.SetActive(false);

    }

    void SetEnemy(string tsv)
    {


        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnsize = row[0].Split('\t').Length;

    
        // Debug.Log(so.enemydata.Length + "asdf");

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            for (int j = 0; j < columnsize; j++)
            {
                TowerDataFrame item = TD.towerData[i];

                item.name = column[0];
                item.towerStep = int.Parse(column[1]);
                item.towerCode = int.Parse(column[2]);
                item.damage = float.Parse(column[3]);
                item.delay = float.Parse(column[4]);
                item.range = float.Parse(column[5]);
                item.critical = float.Parse(column[6]);
                item.upgradAtk = float.Parse(column[7]);
                item.upgradCri = float.Parse(column[8]);
                item.towerPrice = int.Parse(column[9]);
                item.upgradPrice = int.Parse(column[10]);
                item.atkType = int.Parse(column[11]);
                item.towerInfo = column[12];
                item.detailInformation = column[13];
            }
        }

        towerDatas = TD.towerData;
    }

    public static TowerDataFrame GetData(int unitcode)
    {
        for (int i = 0; i < towerDatas.Length; i++)
        {
            if (towerDatas[i].towerCode == unitcode)
            {
                return towerDatas[i];
            }
        }

        return null;
    }

}
