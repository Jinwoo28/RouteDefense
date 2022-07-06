using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{

    [SerializeField] private int playercoin = 1000;
    [SerializeField] private GameObject NotEnoughMoney = null;
    public int SetPlayerCoin { get => playercoin; set => playercoin = value; }

    private int playerlife = 30;

    private void Start()
    {
        playercoin = (int)(playercoin*SkillSettings.PassiveValue("StartMoney"));
    }

    public int GetSetPlayerCoin
    {
        get
        {
            return playercoin;
        }
        set
        {
            playercoin -= value;
        }
    }

    public void PlayerCoinUp(int count)
    {
        playercoin += (int)(count*SkillSettings.PassiveValue("GetCoinUp"));
    }

    public int GetPlayerLife
    {
        get
        {
            return playerlife;
        }

    }

    public void PlayerLifeDown(int num)
    {
        if (playerlife >= 0)
        {
            playerlife-=num;
        }
    }

    public void ShowNotEnoughMoneyCor()
    {
        NotEnoughMoney.SetActive(true);
        StopCoroutine("ShowNotEnoughMoney");
        StartCoroutine("ShowNotEnoughMoney");
    }
    private IEnumerator ShowNotEnoughMoney()
    {
        yield return new WaitForSeconds(1f);
        NotEnoughMoney.SetActive(false);
    }



}
