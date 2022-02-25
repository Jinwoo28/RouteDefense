using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private int playercoin = 500;
    private int playerlife = 20;

    public int PlayerCoin
    {
        get
        {
            return playercoin;
        }
    }

    public void PlayerCoinUp(int count)
    {
        playercoin += count;
    }

    public int PlayerLife
    {
        get
        {
            return playerlife;
        }

    }

    public void PlayerLifeDown()
    {
        playerlife --;
        Debug.Log(playerlife);
    }

}
