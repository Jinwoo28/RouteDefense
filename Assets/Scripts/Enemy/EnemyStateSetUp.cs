using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyStateSetUp : MonoBehaviour
{
    public SetEnemyData Enemydata;

    const string URL = "https://docs.google.com/spreadsheets/d/1TGYXHFpwIhrgfxqPvFgqKRWoIfrCWKggKbzQOVXlHeQ/export?format=tsv&range=A2:J";

    public static EnemyDataFrame[] enemystate;

    private static int AlreadySet = 0;

    [SerializeField] private GameObject DarkImage = null;

    private void Start()
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

        for(int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            for(int j = 0; j < columnsize; j++)
            {
                EnemyDataFrame item = Enemydata.enemyData[i];

                item.name = column[0];
                item.unitcode = int.Parse(column[1]);
                item.hp = float.Parse(column[2]);
                item.speed = float.Parse(column[3]);
                item.damage = float.Parse(column[4]);
                item.coin = int.Parse(column[5]);
                item.amour = int.Parse(column[6]);
                item.avoidance = int.Parse(column[7]);
                item.feature = float.Parse(column[8]);
                item.enemytype = int.Parse(column[9]);
            }
        }

        enemystate = Enemydata.enemyData;
    }

    public static EnemyDataFrame GetData(int unitcode)
    {
        for(int i = 0; i < enemystate.Length; i++)
        {
            if(enemystate[i].unitcode == unitcode)
            {
                return enemystate[i];
            }
        }

        return null;
    }



}
