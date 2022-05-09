using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSetting : MonoBehaviour
{

    //해상도를 고를 dropdown
    [SerializeField] private TMP_Dropdown resolutionDropdown = null;

    //전체화면 / 창화면 변경
    [SerializeField] private TMP_Dropdown fullscreenDropdown = null;

    //가지고 있는 해상도 배열
    private List<Resolution> resolutions = new List<Resolution>();

    //private int resolutionNum = 0;
    //private int fullscreenNum = 0;

   // [SerializeField] private bool is16v9 = true;
    [SerializeField] private bool hasHz = false;

    private bool isSetting = false;

    private void Start()
    {
        //리스트에 가지고 있는 모니터의 출력 해상도 저장
        resolutions.AddRange(Screen.resolutions);

        //if (resolutionDropdown != null && fullscreenDropdown != null)
        //{
        //    ResolutionInit();
        //    OkBtnClick();
        //}
        //ChangeRect();
    }
    private void ResolutionInit()
    {
        resolutions.Reverse();

        //16:9 해상도 고정을 위한 int변수
        int xRate = 16;
        int yRate = 9;

        //16:9인 해상도만 가져오기
        if (resolutions.Count != 0)
        {
            resolutions = resolutions.FindAll(x => (float)x.width / x.height == (float)xRate / yRate);
        }

        //두 개의 드롭다운 초기화
        resolutionDropdown.ClearOptions();
        fullscreenDropdown.ClearOptions();

        // Hz 표시여부
        if (!hasHz && resolutions.Count > 0)
        {
            List<Resolution> tempResolutions = new List<Resolution>();

            //역순으로 정렬되어 있기 때문에 인덱스가 낮을수록 높은 헤르츠를 가짐
            int curWidth = resolutions[0].width;
            int curHeight = resolutions[0].height;
            tempResolutions.Add(resolutions[0]);

            foreach (var resolution in resolutions)
            {
                //비율이 달랐을 때 첫 번째 항목이 가장 높은 비율을 가진다.
                if (curWidth != resolution.width || curHeight != resolution.height)
                {
                    tempResolutions.Add(resolution);
                    curWidth = resolution.width;
                    curHeight = resolution.height;
                }
            }
            resolutions = tempResolutions;
        }


        // 전체화면 드롭다운에 text 옵션값 추가
        TMP_Dropdown.OptionData fulloption = new TMP_Dropdown.OptionData();
        fulloption.text = "전체화면";

        TMP_Dropdown.OptionData windowoption = new TMP_Dropdown.OptionData();
        windowoption.text = "창 모드";

        fullscreenDropdown.options.Add(fulloption);
        fullscreenDropdown.options.Add(windowoption);

        int optionNum = 0;


        foreach (Resolution res in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = res.width + " X " + res.height;       // 1920 X 1080
            resolutionDropdown.options.Add(option);


            //if (res.width == Screen.width && res.height == Screen.height)
            //{
            //    resolutionDropdown.value = optionNum;
            //    optionNum++;
            //}
        }

        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.value = ResolutionIndex;
    }

    //해상도 저장
    public int ResolutionIndex
    {
        get => PlayerPrefs.GetInt("ResolutionIndex", 0);
        set => PlayerPrefs.SetInt("ResolutionIndex", value);
    }

    //전체화면, 창모드 저장
    public bool IsFullscreen
    {
        // 1: fullscreen 0: 창모드
        get => PlayerPrefs.GetInt("isFullScreen", 1) == 1;
        set => PlayerPrefs.SetInt("isFullScreen", value ? 1 : 0);
    }


    //해상도 바꾸는 드롭박스
    public void DropboxOptionChange(int x)
    {
        Debug.Log(x + "해상도");

        ResolutionIndex = x;
    }

    //창모드와 전체화면 드롭박스
    public void DropboxOptionFullscreen(int x)
    {
        Debug.Log(x);
        //전체화면
        if (x == 0)
        {
            IsFullscreen = true;
        }
        //창화면
        else if((x == 1))
        { 
             IsFullscreen = false;
        }

    }

    public void OkBtnClick()
    {
        resolutionDropdown.value = ResolutionIndex;
        if (IsFullscreen)
        {
            fullscreenDropdown.value = 0;
        }
        else
        {
            fullscreenDropdown.value = 1;
        }

        Debug.Log("확인");
        //해상도에 따라서 canvas의 Viewport Rect값을 변경하여 일정하게 표시
        //https://giseung.tistory.com/19
        //https://www.youtube.com/watch?v=wUkwN8Evy8s&t=823s

       

        Screen.SetResolution(resolutions[ResolutionIndex].width, resolutions[ResolutionIndex].height, IsFullscreen);

       

    }

    private void ChangeRect()
    {
        Debug.Log("ssd");
        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장
        //기기의 해상도와 설정한 해상도가 맞지 않을 경우 카메라의 viewport Rect를 변경하여 일그러짐 없이 출력
        if ((float)resolutions[ResolutionIndex].width / resolutions[ResolutionIndex].height < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)resolutions[ResolutionIndex].width / resolutions[ResolutionIndex].height)
                / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight)
                / (float)resolutions[ResolutionIndex].width / resolutions[ResolutionIndex].height; // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }


}
