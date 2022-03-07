using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private Text life = null;
    [SerializeField] private Text Coin = null;

    [SerializeField] private int playercoin = 500;
    private int playerlife = 20;




    private void Update()
    {
        life.text = "Life : " + playerlife.ToString();
        Coin.text = "Coin : " + playercoin.ToString();
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
    }

}
