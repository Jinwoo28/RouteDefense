using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StageDataSetUp : MonoBehaviour
{
    public SetStageEnemyData stageData;
    const string URL_Round = "https://docs.google.com/spreadsheets/d/1XoikRd33aGUOpZk_C94GhEb8U9pkaQ_fN8ZasNaCbbo/export?format=tsv&range=A2:B9";
    const string URL_Info = "https://docs.google.com/spreadsheets/d/1uDvzQHEDCkAJiwIzR2pTcsPJ20Z1z6u-YPNPSwPI8nE/export?format=tsv&range=B2:Z";

    //https://www.youtube.com/watch?v=Xo7EEegTUfE&t=670s

    private static int alreadyLoad = 0;

    private void Awake()
    {
        if (alreadyLoad == 0)
        {
            StartCoroutine("CoDataSetFromWeb");
            alreadyLoad = 1;
        }
    }

    IEnumerator CoDataSetFromWeb()
    {
        UnityWebRequest wwwR = UnityWebRequest.Get(URL_Round);

        yield return wwwR.SendWebRequest();
        string dataR = wwwR.downloadHandler.text;

        UnityWebRequest wwwI = UnityWebRequest.Get(URL_Info);
        yield return wwwI.SendWebRequest();
        string dataI = wwwI.downloadHandler.text;

        //print(dataR);
        SetStageRound(dataR);
        SetStageInformation(dataI);
        //print(dataI);
    }

    private void SetStageRound(string dataR)
    {
        string[] row = dataR.Split('\n');
        int rowSizeR = row.Length;

        int columnsize = row[0].Split('\t').Length;

        for(int i = 0; i < rowSizeR; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnsize; j++) 
            {
                //item�� �����Ҵ��� �ƴ� ��������
                StageDataFrame item = stageData.stageDataFrame[i];
                item.stageCode = int.Parse(column[0]);
                item.stageCount = int.Parse(column[1]);
            }
        }
    }

    private void SetStageInformation(string dataI)
    {
        string[] row = dataI.Split('\n');       //24��

        //���������� ������ �ֱ�
        for (int i = 0; i < row.Length; i+=3)   //���� 8�� �ݺ�
        {
            //item�� �����Ҵ��� �ƴ� ��������
            StageDataFrame item = stageData.stageDataFrame[i/3];
           
            int columnsize = stageData.stageDataFrame[i/3].stageCount;
            
            string[] columnSpawnCount = row[i].Split('\t');   //���� �ִ� ������
            string[] columnSpawnTime = row[i+1].Split('\t');   //���� �ִ� ������
            string[] columnEnemyKind = row[i+2].Split('\t');   //���� �ִ� ������

            //Debug.Log(columnEnemyKind.Length);

            item.roundData = new RoundData[item.stageCount];
            
            //���� �� ������ �ֱ�
            for (int j = 0; j < item.stageCount; j++)
            {
                item.roundData[j] = new RoundData();
                string[] enemyKind = columnEnemyKind[j].Split(',');

                item.roundData[j].spawnCount = int.Parse(columnSpawnTime[j]);
                item.roundData[j].spawnTime = float.Parse(columnSpawnCount[j]);

                item.roundData[j].enemyKind = new int[enemyKind.Length];

                for (int k = 0; k < enemyKind.Length; k++)
                {
                    item.roundData[j].enemyKind[k] = int.Parse(enemyKind[k]);
                }
            }
        }
    }

    public StageDataFrame GetStageInfo(int code,SetStageEnemyData SSED)
    {
        for(int i = 0; i < SSED.stageDataFrame.Length;i++)
        {
            if(SSED.stageDataFrame[i].stageCode == code)
            {
                return SSED.stageDataFrame[i];
            }
        }
        return null;
    }
    





}
