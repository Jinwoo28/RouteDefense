using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InformationKind
{
    public GameObject Parent;
    public GameObject[] Child;
}

public enum infoKind
{
    Progress,
    Charac,
    forest
}
public class ShowInformation : MonoBehaviour
{
    public GameObject InformationPanel;
    public TextMeshProUGUI Page = null;
    int CurrentPage = 1;
    int MaxPage = 0;

    bool BPrograss = false;
    bool BForest = false;

    public Image progress;
    public TextMeshProUGUI progresstext;

    public Image Forest;
    public TextMeshProUGUI foresttext;

    public InformationKind[] informationkind;

    private infoKind Kind = new infoKind();

    public void ClickProgress()
    {
        Kind = infoKind.Progress;
        CurrentPage = 0;
        MaxPage = informationkind[0].Child.Length;
        progress.color = Color.white;
        progresstext.color = Color.white;
        Forest.color = Color.gray;
        foresttext.color = Color.gray;

        informationkind[0].Parent.SetActive(true);
        informationkind[1].Parent.SetActive(false);

        Page.text = $"{CurrentPage+1} / {informationkind[0].Child.Length}";

        for(int i = 0;i< informationkind[1].Child.Length; i++)
        {
            informationkind[1].Child[i].SetActive(false);
        }

        informationkind[0].Child[CurrentPage].SetActive(true);

    }

    public void ClickForest()
    {
        Kind = infoKind.forest;
        CurrentPage = 0;
        MaxPage = informationkind[1].Child.Length;
        progress.color = Color.gray;
        progresstext.color = Color.gray;
        Forest.color = Color.white;
        foresttext.color = Color.white;

        informationkind[0].Parent.SetActive(false);
        informationkind[1].Parent.SetActive(true);

        Page.text = $"{CurrentPage+1} / {informationkind[1].Child.Length}";

        informationkind[1].Child[CurrentPage].SetActive(true);

        for (int i = 0; i < informationkind[0].Child.Length; i++)
        {
            informationkind[0].Child[i].SetActive(false);
        }

    }

    public void NextPage()
    {
        switch (Kind)
        {
            case infoKind.Progress:
                if (CurrentPage < informationkind[0].Child.Length-1) 
                {
                    informationkind[0].Child[CurrentPage].SetActive(false);
                    CurrentPage++;
                    informationkind[0].Child[CurrentPage].SetActive(true);
                    Page.text = $"{CurrentPage + 1} / {informationkind[0].Child.Length}";
                }
                break;
            case infoKind.forest:
                if (CurrentPage < informationkind[1].Child.Length-1)
                {
                    informationkind[1].Child[CurrentPage].SetActive(false);
                    CurrentPage++;
                    informationkind[1].Child[CurrentPage].SetActive(true);
                    Page.text = $"{CurrentPage + 1} / {informationkind[1].Child.Length}";
                }
                break;
        }
    }

    public void BackPage()
    {
        switch (Kind)
        {
            case infoKind.Progress:
                if (CurrentPage > 0)
                {
                    informationkind[0].Child[CurrentPage].SetActive(false);
                    CurrentPage--;
                    informationkind[0].Child[CurrentPage].SetActive(true);
                    Page.text = $"{CurrentPage + 1} / {informationkind[0].Child.Length}";
                }
                break;
            case infoKind.forest:
                if (CurrentPage > 0)
                {
                    informationkind[1].Child[CurrentPage].SetActive(false);
                    CurrentPage--;
                    informationkind[1].Child[CurrentPage].SetActive(true);
                    Page.text = $"{CurrentPage + 1} / {informationkind[1].Child.Length}";
                }
                break;
        }
    }


    private void Start()
    {
        Kind = infoKind.Progress;
    }

    public void ChangeInformation(int _kind)
    {
        Kind = (infoKind)_kind;
    }

    public void OpenPanel()
    {
        InformationPanel.SetActive(true);
        ClickProgress();
    }
    public void OffenPanel()
    {
        InformationPanel.SetActive(false);
        for (int i = 0; i < informationkind[1].Child.Length; i++)
        {
            informationkind[1].Child[i].SetActive(false);
        }
        for (int i = 0; i < informationkind[0].Child.Length; i++)
        {
            informationkind[0].Child[i].SetActive(false);
        }
    }

}
