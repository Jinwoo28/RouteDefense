using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private Text life = null;
    [SerializeField] private Text Coin = null;

    private int playercoin = 500;
    private int playerlife = 20;

    private void Start()
    {
        StartCoroutine("ShowPlayerInfo");
    }



    IEnumerator ShowPlayerInfo()
    {
        while (true)
        {

            life.text = "Life : " + playerlife.ToString();
            Coin.text = "Coin : " + playercoin.ToString();
            yield return null;
        }
    }


    public int PlayerCoin
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
