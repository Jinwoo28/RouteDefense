using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSetting : MonoBehaviour
{

    //해상도를 고를 dropdown
    [SerializeField] private TMP_Dropdown resolutionDropdown = null;

    //전체화면 창화면 변경
    [SerializeField] private TMP_Dropdown fullscreenDropdown = null;

    //가지고 있는 해상도 배열
    private List<Resolution> resolutions = new List<Resolution>();

    private int resolutionNum = 0;
    private int fullscreenNum = 0;

    FullScreenMode screenMode;

    private void Start()
    {
        ResolutionInit();
    }
    private void ResolutionInit()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutions.Reverse();


        resolutionDropdown.options.Clear();

        fullscreenDropdown.options.Clear();

        
        TMP_Dropdown.OptionData fulloption = new TMP_Dropdown.OptionData();
        fulloption.text = "Full Screen";

        TMP_Dropdown.OptionData windowoption = new TMP_Dropdown.OptionData();
        windowoption.text = "Window Screen";

        fullscreenDropdown.options.Add(fulloption);
        fullscreenDropdown.options.Add(windowoption);

        int optionNum = 0;

        foreach(Resolution res in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = res.width + "X" + res.height + "Y" + res.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if(res.width == Screen.width && res.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
                optionNum++;
            }


        }

        resolutionDropdown.RefreshShownValue();
        
    }

    //해상도 바꾸는 드롭박스
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    //창모드와 전체화면 드롭박스
    public void DropboxOptionFullscreen(int x)
    {
        //전체화면
        if (x == 0) screenMode = FullScreenMode.FullScreenWindow;
        //창화면
        else if (x == 1) screenMode = FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
