using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSettings : MonoBehaviour
{
    private void Start()
    {
        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        //Screen.SetResolution(ResolutionSetting.staticResolutions[ResolutionIndex].width, ResolutionSetting.staticResolutions[ResolutionIndex].height, IsFullscreen());

        //Debug.Log("해상도 변경");

        ////기기의 해상도와 설정한 해상도가 맞지 않을 경우 카메라의 viewport Rect를 변경하여 일그러짐 없이 출력
        //if ((float)ResolutionSetting.staticResolutions[ResolutionIndex].width / ResolutionSetting.staticResolutions[ResolutionIndex].height < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        //{
        //    float newWidth = ((float)ResolutionSetting.staticResolutions[ResolutionIndex].width / ResolutionSetting.staticResolutions[ResolutionIndex].height)
        //        / ((float)deviceWidth / deviceHeight); // 새로운 너비
        //    Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        //}
        //else // 게임의 해상도 비가 더 큰 경우
        //{
        //    float newHeight = ((float)deviceWidth / deviceHeight)
        //        / (float)ResolutionSetting.staticResolutions[ResolutionIndex].width / ResolutionSetting.staticResolutions[ResolutionIndex].height; // 새로운 높이
        //    Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        //}
    }
    public int ResolutionIndex
    {
        get => PlayerPrefs.GetInt("ResolutionIndex");
    }

    //전체화면, 창모드 저장
    public bool IsFullscreen()
    {
        if (PlayerPrefs.GetInt("isFullScreen") == 1)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}
