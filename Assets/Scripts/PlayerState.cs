using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{

    [SerializeField] private int playercoin = 500;
    private int playerlife = 30;

    private void Start()
    {

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
        playercoin += count;
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
