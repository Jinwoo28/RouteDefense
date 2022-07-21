using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StageDataSetUp : MonoBehaviour
{
    public SetStageEnemyData stageData;
    const string URL_Round = "https://docs.google.com/spreadsheets/d/1XoikRd33aGUOpZk_C94GhEb8U9pkaQ_fN8ZasNaCbbo/export?format=tsv&range=A2:9";
    const string URL_Info = "https://docs.google.com/spreadsheets/d/1uDvzQHEDCkAJiwIzR2pTcsPJ20Z1z6u-YPNPSwPI8nE/export?format=tsv&range=A2:N";

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
        SetStage(dataR, dataI);
    }

    private void SetStage(string dataR, string dataI)
    {
        string[] row = dataR.Split('\n');
        int rowSizeR = row.Length;
        Debug.Log(rowSizeR);

        for(int i = 0; i < rowSizeR; i++)
        {
            //item은 동적할당이 아닌 참조형식
            StageDataFrame item = stageData.stageDataFrame[i];
            item.stageCode = 1;
        }

    }



}
