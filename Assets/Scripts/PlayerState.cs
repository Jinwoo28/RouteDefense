using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    [SerializeField] private GameObject notEnoughMoney = null;
    
    private int playerCoin = 500;
    private int playerLife = 30;

    private void Start()
    {
        playerCoin = (int)(playerCoin*SkillSettings.PassiveValue("StartMoney"));
    }

    public int GetSetPlayerCoin
    {
        get
        {
            return playerCoin;
        }
        set
        {
            playerCoin -= value;
        }
    }

    public void PlayerCoinUp(int count)
    {
        playerCoin += (int)(count*SkillSettings.PassiveValue("GetCoinUp"));
    }

    public int GetPlayerLife
    {
        get
        {
            return playerLife;
        }

    }

    public void PlayerLifeDown(int num)
    {
        if (playerLife >= 0)
        {
            playerLife-=num;
        }
    }

    public void ShowNotEnoughMoneyCor()
    {
        notEnoughMoney.SetActive(true);
        StopCoroutine("ShowNotEnoughMoney");
        StartCoroutine("ShowNotEnoughMoney");
    }
    private IEnumerator ShowNotEnoughMoney()
    {
        yield return new WaitForSeconds(1f);
        notEnoughMoney.SetActive(false);
    }



}
