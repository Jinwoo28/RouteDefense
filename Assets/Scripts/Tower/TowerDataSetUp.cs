using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerDataSetUp : MonoBehaviour
{
    public TowerDataSc TD;

    const string URL = "https://docs.google.com/spreadsheets/d/19oIy0vvxi2Xtfw5X8V8ZsdRhoujJgtXPAy7Qru5Sjv4/export?format=tsv&range=A2:K";

    public static TowerData[] towerDatas;

    private static int AlreadySet = 0;

    private void Start()
    {
        if (AlreadySet == 0)
        {
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
                TowerData item = TD.towerdata[i];

                item.Name = column[0];
                item.TowerStep = int.Parse(column[1]);
                item.TowerCode = int.Parse(column[2]);
                item.Damage = int.Parse(column[3]);
                item.Delay = float.Parse(column[4]);
                item.Range = float.Parse(column[5]);
                item.Critical = int.Parse(column[6]);
                item.UpgradeAtk = float.Parse(column[7]);
                item.UpgradeCri = int.Parse(column[8]);
                item.TowerPrice = int.Parse(column[9]);
                item.UpgradePrice = int.Parse(column[10]);
            }
        }

        towerDatas = TD.towerdata;
    }

    public static TowerData GetData(int unitcode)
    {
        for (int i = 0; i < towerDatas.Length; i++)
        {
            if (towerDatas[i].TowerCode == unitcode)
            {
                return towerDatas[i];
            }
        }

        return null;
    }

}
