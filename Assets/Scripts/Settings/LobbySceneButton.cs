using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbySceneButton : MonoBehaviour
{
    [SerializeField] private GameObject settingPanal = null;

    [SerializeField] private GameObject ShopPanel = null;

    [SerializeField] private GameObject[] ShopContents = null;
    [SerializeField] private Image[] image;

    [SerializeField] private GameObject BooksPanel = null;

    [SerializeField] private GameObject Information = null;
    [SerializeField] private Image InfoImage = null;
    [SerializeField] private Sprite[] InfoList = null;
    [SerializeField] private TextMeshProUGUI informationtext = null;
    private int Page = 0;

    public void SettingPanalOn()
    {
        Debug.Log("??");
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

    public void ShopContentsChange(int i)
    {
        switch (i)
        {
            case 0:
                ShopContents[0].SetActive(true);
                ShopContents[1].SetActive(false);
                ShopContents[2].SetActive(false);
                image[0].color = Color.black;
                image[1].color = new Color(0.6117647f, 0.6117647f, 0.6117647f);
                image[2].color = new Color(0.6117647f, 0.6117647f, 0.6117647f);
                break;
            case 1:
                ShopContents[0].SetActive(false);
                ShopContents[1].SetActive(true);
                ShopContents[2].SetActive(false);
                image[0].color = new Color(0.6117647f, 0.6117647f, 0.6117647f);
                image[1].color = Color.black;
                image[2].color = new Color(0.6117647f, 0.6117647f, 0.6117647f);
                break;
            case 2:
                ShopContents[0].SetActive(false);
                ShopContents[1].SetActive(false);
                ShopContents[2].SetActive(true);
                image[0].color = new Color(0.6117647f, 0.6117647f, 0.6117647f);
                image[1].color = new Color(0.6117647f, 0.6117647f, 0.6117647f);
                image[2].color = Color.black;
                break;
        }
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
