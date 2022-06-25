using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{

    private int playercoin = 10000;
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

    public void PlayerLifeDown()
    {
        if (playerlife >= 0)
        {
            playerlife--;
        }
    }

   

}
