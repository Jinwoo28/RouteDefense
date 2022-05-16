using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private AddTile mapmanager = null;
    [SerializeField] private PlayerState playerstate = null;
    [SerializeField] private EnemyManager enemymanager = null;
    [SerializeField] private Image Gettetris = null;
    [SerializeField] private Sprite[] Settetris = null;
    [SerializeField] private TextMeshProUGUI tileprice = null;
    [SerializeField] private TextMeshProUGUI playercoin = null;
    [SerializeField] private TextMeshProUGUI playerlife = null;

    [SerializeField] private TextMeshProUGUI stageinfo = null;


    void Update()
    {
        SetVariable();
    }

    public void SetVariable()
    {
        int coin = playerstate.GetSetPlayerCoin;
        int life = playerstate.GetPlayerLife;

        int maxstage = enemymanager.Getmaxstage;
        int currentstage = enemymanager.Getcurrentstage;

        int addtilenum = mapmanager.GetAddTileNum;
        int addtileprice = mapmanager.GetAddtilePrice;

        playercoin.text = $"{coin}";
        playerlife.text = $"{life}";

        if (currentstage <= 20)
        {
            stageinfo.text = $"스테이지 {currentstage} / {maxstage}";
        }
        Gettetris.sprite = Settetris[addtilenum];
        tileprice.text = addtileprice.ToString();
    }
}
