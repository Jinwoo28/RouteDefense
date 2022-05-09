using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbySceneButton : MonoBehaviour
{
    [SerializeField] private GameObject settingPanal = null;

    [SerializeField] private GameObject ShopPanel = null;

    [SerializeField] private GameObject BooksPanel = null;

    [SerializeField] private GameObject Information = null;
    [SerializeField] private Image InfoImage = null;
    [SerializeField] private Sprite[] InfoList = null;
    [SerializeField] private TextMeshProUGUI informationtext = null;
    private int Page = 0;

    public void SettingPanalOn()
    {
        settingPanal.SetActive(true);
    }
    public void SettingPanalOff()
    {
        settingPanal.SetActive(false);
    }

    public void ShopOpen()
    {
        ShopPanel.SetActive(true);
    }

    public void ShopClose()
    {
        ShopPanel.SetActive(false);
    }

    public void ShowBooks()
    {
        BooksPanel.SetActive(true);
    }
    public void CloseBooks()
    {
        BooksPanel.SetActive(false);
    }

    public void ShowInfo()
    {
        Information.SetActive(true);
        ChangeImage();
    }
    public void CloseInfo()
    {
        Information.SetActive(false);
    }

    public void ChangeImage()
    {
        InfoImage.sprite = InfoList[Page];
        informationtext.text = $"{Page+1} / {InfoList.Length}";
    }

    public void PageNumPlus() 
    {
        if (Page < InfoList.Length-1)
        {
            Page++;
        }
        Debug.Log(Page);
        ChangeImage();
    }
    public void PageNumMinus() 
    {
        if (Page >0)
        {
            Page--;
        }
        ChangeImage();
    }


}
