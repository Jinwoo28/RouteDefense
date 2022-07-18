using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private AddTile addTile = null;
    [SerializeField] private PlayerState playerState = null;
    [SerializeField] private EnemyManager enemyManager = null;
    [SerializeField] private Image tetrisImage = null;
    [SerializeField] private Sprite[] tetrisSprites = null;

    [SerializeField] private TextMeshProUGUI textTilePrice = null;
    [SerializeField] private TextMeshProUGUI textPlayerCoin = null;
    [SerializeField] private TextMeshProUGUI textPlayerLife = null;
    [SerializeField] private TextMeshProUGUI textStageCount = null;



    void Update()
    {
        SetVariable();
    }

    public void SetVariable()
    {
        int coin = playerState.GetSetPlayerCoin;
        int life = playerState.GetPlayerLife;

        int maxStage = enemyManager.Getmaxstage;
        int currentStage = enemyManager.Getcurrentstage;

        int addTileNum = addTile.GetAddTileNum;
        int addTilePrice = addTile.GetAddtilePrice;

        textPlayerCoin.text = "$" + coin;
        textPlayerLife.text = $"{life}";

        if (currentStage <= 20)
        {
            textStageCount.text = $"스테이지 {currentStage} / {maxStage}";
        }
        tetrisImage.sprite = tetrisSprites[addTileNum];
        textTilePrice.text = "$" + addTilePrice.ToString();
    }
}
