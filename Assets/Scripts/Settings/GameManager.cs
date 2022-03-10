using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private SceneControl sceneManager = null;
    [SerializeField] private GameObject settingPanal = null;
    private MultipleSpeed multipleSpeed = null;

    private void Start()
    {
        settingPanal.SetActive(false);
        multipleSpeed = this.GetComponent<MultipleSpeed>();
    }

    public void SettingPanalOn()
    {
        settingPanal.SetActive(true);
        multipleSpeed.OnClickStart(0);
    }

    public void SettingPanalOff()
    {
        settingPanal.SetActive(false);
        multipleSpeed.OnClickStart(1);
    }

    public void GotoLobby()
    {

    }

    public void ReStartGame()
    {

    }


}
