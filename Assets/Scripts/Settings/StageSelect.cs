using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetStageStar
{
    public int stageNum;
    public GameObject[] Star;
}

public class StageSelect : MonoBehaviour
{
    [SerializeField] private SetStageStar[] setStageStar = null;

    private void Start()
    {
        SetResolution();
        ShowStar();
    }

    private void ShowStar()
    {
        int index = 0;
        foreach(var star in UserInformation.userDataStatic.stageClear)
        {
            if (star.Star1)
            {
                setStageStar[index].Star[0].SetActive(true);
            }
            if (star.Star2)
            {
                setStageStar[index].Star[1].SetActive(true);
            }
            if (star.Star3)
            {
                setStageStar[index].Star[2].SetActive(true);
            }
            index++;
        }
    }

    public void SelectStage(string _stagename)
    {
        LoadSceneControler.LoadScene(_stagename);
    }

    public void SetResolution()
    {
        int Index = PlayerPrefs.GetInt("ResolutionIndex");
        int full = PlayerPrefs.GetInt("isFullScreen");

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(ResolutionSetting.staticResolutions[Index].width, ResolutionSetting.staticResolutions[Index].height, PlayerPrefs.GetInt("isFullScreen")==1?true:false);

        //기기의 해상도와 설정한 해상도가 맞지 않을 경우 카메라의 viewport Rect를 변경하여 일그러짐 없이 출력
        if ((float)ResolutionSetting.staticResolutions[Index].width / ResolutionSetting.staticResolutions[Index].height < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)ResolutionSetting.staticResolutions[Index].width / ResolutionSetting.staticResolutions[Index].height)
                / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight)
                / (float)ResolutionSetting.staticResolutions[Index].width / ResolutionSetting.staticResolutions[Index].height; // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

  
}


