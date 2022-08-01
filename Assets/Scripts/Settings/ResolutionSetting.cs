using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSetting : MonoBehaviour
{

    //�ػ󵵸� �� dropdown
    [SerializeField] private TMP_Dropdown resolutionDropdown = null;

    //��üȭ�� / âȭ�� ����
    [SerializeField] private TMP_Dropdown fullscreenDropdown = null;

    //������ �ִ� �ػ� �迭
    private List<Resolution> resolutions = new List<Resolution>();

    //private int resolutionNum = 0;
    //private int fullscreenNum = 0;

   // [SerializeField] private bool is16v9 = true;
    [SerializeField] private bool hasHz = false;

    private bool isSetting = false;

    private void Start()
    {
        //����Ʈ�� ������ �ִ� ������� ��� �ػ� ����
        resolutions.AddRange(Screen.resolutions);

        //if (resolutionDropdown != null && fullscreenDropdown != null)
        //{
            ResolutionInit();
            OkBtnClick();
        //}
        //ChangeRect();
    }
    private void ResolutionInit()
    {
        resolutions.Reverse();

        //16:9 �ػ� ������ ���� int����
        int xRate = 16;
        int yRate = 9;

        //16:9�� �ػ󵵸� ��������
        if (resolutions.Count != 0)
        {
            resolutions = resolutions.FindAll(x => (float)x.width / x.height == (float)xRate / yRate);
        }

        //�� ���� ��Ӵٿ� �ʱ�ȭ
        resolutionDropdown.ClearOptions();
        fullscreenDropdown.ClearOptions();

        // Hz ǥ�ÿ���
        if (!hasHz && resolutions.Count > 0)
        {
            List<Resolution> tempResolutions = new List<Resolution>();

            //�������� ���ĵǾ� �ֱ� ������ �ε����� �������� ���� �츣���� ����
            int curWidth = resolutions[0].width;
            int curHeight = resolutions[0].height;
            tempResolutions.Add(resolutions[0]);

            foreach (var resolution in resolutions)
            {
                //������ �޶��� �� ù ��° �׸��� ���� ���� ������ ������.
                if (curWidth != resolution.width || curHeight != resolution.height)
                {
                    tempResolutions.Add(resolution);
                    curWidth = resolution.width;
                    curHeight = resolution.height;
                }
            }
            resolutions = tempResolutions;
        }


        // ��üȭ�� ��Ӵٿ text �ɼǰ� �߰�
        TMP_Dropdown.OptionData fulloption = new TMP_Dropdown.OptionData();
        fulloption.text = "��üȭ��";

        TMP_Dropdown.OptionData windowoption = new TMP_Dropdown.OptionData();
        windowoption.text = "â ���";

        fullscreenDropdown.options.Add(fulloption);
        fullscreenDropdown.options.Add(windowoption);

        //int optionNum = 0;


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

    //�ػ� ����
    public int ResolutionIndex
    {
        get => PlayerPrefs.GetInt("ResolutionIndex", 0);
        set => PlayerPrefs.SetInt("ResolutionIndex", value);
    }

    //��üȭ��, â��� ����
    public bool IsFullscreen
    {
        // 1: fullscreen 0: â���
        get => PlayerPrefs.GetInt("isFullScreen", 1) == 1;
        set => PlayerPrefs.SetInt("isFullScreen", value ? 1 : 0);
    }


    //�ػ� �ٲٴ� ��ӹڽ�
    public void DropboxOptionChange(int x)
    {
        Debug.Log(x + "�ػ�");

        ResolutionIndex = x;
    }

    //â���� ��üȭ�� ��ӹڽ�
    public void DropboxOptionFullscreen(int x)
    {
        Debug.Log(x);
        //��üȭ��
        if (x == 0)
        {
            IsFullscreen = true;
        }
        //âȭ��
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

        //Debug.Log("Ȯ��");
        //�ػ󵵿� ���� canvas�� Viewport Rect���� �����Ͽ� �����ϰ� ǥ��
        //https://giseung.tistory.com/19
        //https://www.youtube.com/watch?v=wUkwN8Evy8s&t=823s

       

        Screen.SetResolution(resolutions[ResolutionIndex].width, resolutions[ResolutionIndex].height, IsFullscreen);

       

    }

    private void ChangeRect()
    {
        Debug.Log("ssd");
        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����
        //����� �ػ󵵿� ������ �ػ󵵰� ���� ���� ��� ī�޶��� viewport Rect�� �����Ͽ� �ϱ׷��� ���� ���
        if ((float)resolutions[ResolutionIndex].width / resolutions[ResolutionIndex].height < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)resolutions[ResolutionIndex].width / resolutions[ResolutionIndex].height)
                / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight)
                / (float)resolutions[ResolutionIndex].width / resolutions[ResolutionIndex].height; // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }


}
