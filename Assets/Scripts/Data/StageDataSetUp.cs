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

    private void Awake()
    {
        StartCoroutine("CoDataSetFromWeb");
    }

    IEnumerator CoDataSetFromWeb()
    {
        UnityWebRequest wwwR = UnityWebRequest.Get(URL_Round);

        yield return wwwR.SendWebRequest();
        string dataR = wwwR.downloadHandler.text;

        UnityWebRequest wwwI = UnityWebRequest.Get(URL_Info);
        yield return wwwI.SendWebRequest();
        string dataI = wwwI.downloadHandler.text;

        print(dataR);
        SetStageRound(dataR);
        SetStageInformation(dataI);
        print(dataI);
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
                //item은 동적할당이 아닌 참조형식
                StageDataFrame item = stageData.stageDataFrame[i];
                item.stageCode = int.Parse(column[0]);
                item.stageCount = int.Parse(column[1]);
                Debug.Log(item.stageCount);
            }
        }
    }

    private void SetStageInformation(string dataI)
    {
        string[] row = dataI.Split('\n');       //24줄

        //스테이지별 데이터 넣기
        for (int i = 0; i < row.Length; i+=3)   //현재 8번 반복
        {
            //item은 동적할당이 아닌 참조형식
            StageDataFrame item = stageData.stageDataFrame[i/3];
           
            int columnsize = stageData.stageDataFrame[i/3].stageCount;
            
            string[] columnSpawnCount = row[i].Split('\t');   //열에 있는 데이터
            string[] columnSpawnTime = row[i+1].Split('\t');   //열에 있는 데이터
            string[] columnEnemyKind = row[i+2].Split('\t');   //열에 있는 데이터

            //Debug.Log(columnEnemyKind.Length);

            item.roundData = new RoundData[item.stageCount];
            
            //라운드 별 데이터 넣기
            for (int j = 0; j < item.stageCount; j++)
            {
                item.roundData[j] = new RoundData();
                string[] enemyKind = columnEnemyKind[j].Split(',');

                item.roundData[j].spawnTime = float.Parse(columnSpawnTime[j]);
                item.roundData[j].spawnCount = int.Parse(columnSpawnCount[j]);

                item.roundData[j].enemyKind = new int[enemyKind.Length];

                Debug.Log(enemyKind.Length);
                for (int k = 0; k < enemyKind.Length; k++)
                {
                    item.roundData[j].enemyKind[k] = int.Parse(enemyKind[k]);
                }
            }
        }
    }



}
