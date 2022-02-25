using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject StartText;
    [SerializeField] private GameObject CancelText;



    private static GameManager instance = null;

    public delegate void CancleStage();
    public static CancleStage canslestage;
    
    List<Enemy> EnemyCount;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /*
     게임매니저에서 필요한 것
    1. 플레이어 코인
    2. 게임 시작 _ 성공, 실패
    3. 스테이지 정보
     */

    public void GameReStart() { }

    public void GameFail() { }



}
