using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyStateSetUp : MonoBehaviour
{
    public EnemyDataTest Enemydata;

    const string URL = "https://docs.google.com/spreadsheets/d/1TGYXHFpwIhrgfxqPvFgqKRWoIfrCWKggKbzQOVXlHeQ/export?format=tsv&range=A2:J";

    public static EnemyState[] enemystate;

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

        for(int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            for(int j = 0; j < columnsize; j++)
            {
                EnemyState item = Enemydata.enemydata[i];

                item.Name = column[0];
                item.UnitCode = int.Parse(column[1]);
                item.Hp = float.Parse(column[2]);
                item.Speed = float.Parse(column[3]);
                item.Damage = float.Parse(column[4]);
                item.coin = int.Parse(column[5]);
                item.Amour = int.Parse(column[6]);
                item.avoidance = int.Parse(column[7]);
                item.feature = float.Parse(column[8]);
                item.enemytype = int.Parse(column[9]);
            }
        }

        enemystate = Enemydata.enemydata;
    }

    public static EnemyState GetData(int unitcode)
    {
        for(int i = 0; i < enemystate.Length; i++)
        {
            if(enemystate[i].UnitCode == unitcode)
            {
                return enemystate[i];
            }
        }

        return null;
    }



}
