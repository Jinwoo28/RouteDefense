using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class StageInfo
{
    public float SpawnTime = 0;
    public int EnemyCount;

    public List<int> enemyNum;

}
public class EnemyManager : MonoBehaviour
{

    public delegate void StageClear();
    public static StageClear stageclear;

    [SerializeField] private MultipleSpeed speedSet = null;

    private float EnemyCoinRate = 0;

    [SerializeField] private GameObject unituiParent = null;

    //스테이지 정보
    [SerializeField] private StageInfo[] stageinfo = null;

    //적이 죽거나 도착지에 도착했을 때, 골드획득이나 라이프 감소를 위한 플레이어 정보
    [SerializeField] private PlayerState playerstate = null;

    //enemy의 체력바와 공격시 데미지를 띄우는 UI정보를 위한 canvas
    [SerializeField] private Transform canvas = null;

    [SerializeField] private GameObject hpbar = null;
    [SerializeField] private GameObject damagenum = null;

    [SerializeField] private Image imagebar = null;
    [SerializeField] private Image[] imageEnemy = null;
    [SerializeField] private TextMeshProUGUI ShowBoss = null;

    private int StageNum = 0;
    public int GetStageNum => StageNum;

    //스테이지가 실행중인지 판단
    private bool gameongoing = false;

    //적 스폰이 끝났는지 여부
    private bool SpawnFinish = false;

    private Vector3[] waypoint;
    private Vector3 SpawnPos;

    [SerializeField] private GameObject ClearPanal = null;
    [SerializeField] private TextMeshProUGUI PlusCoin1 = null;
    [SerializeField] private GameObject FailPanal = null;
    [SerializeField] private TextMeshProUGUI PlusCoin2 = null;
    //private bool StageClear = false;

    [SerializeField] private Transform water = null;

    //소환되는 적들의 정보를 담을 List
    List<Enemy> EnemyCount = null;

     int EnemyRemainCount = 0;

    [SerializeField] private WeatherSetting weather =  null;

    private EnemyPooling Pooling = null;

    public int Getmaxstage => stageinfo.Length;
    public int Getcurrentstage => StageNum + 1;



    private void Start()
    {
        Pooling = this.GetComponent<EnemyPooling>();

        EnemyCount = new List<Enemy>();
        MultipleSpeed.speedup += SpeedUP;
        ClearPanal.SetActive(false);
        FailPanal.SetActive(false);
        ShowEnemyImage(0);
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }


    //게임 시작 될 때 enemy의 루트와 스폰 위치를 받아서 게임 시작
    public void gameStartCourtain(Vector3[] _waypoint, Vector3 _SpawnPos,int spawnNum)
    {
        waypoint = _waypoint;
        SpawnPos = _SpawnPos;
        StartCoroutine(GameStart(_waypoint, _SpawnPos,spawnNum));
    }

    int X = 0;

    IEnumerator GameStart(Vector3[] _wayPoint, Vector3 _spawnPos,int spawnNum)
    {
        GameManager.buttonOff();

        weather.GameStart();

       gameongoing = true;

        //적이 나올 개수
        int count = GameManager.SetGameLevel == 3? (int)(stageinfo[StageNum ].EnemyCount*0.7f): stageinfo[StageNum].EnemyCount;
        int stagenum = StageNum;
        
        //적 종류
        for (int i = 0; i < count; i++)
        {
            X++;
            int enemynum = 0;

            int num = Random.Range(0, stageinfo[StageNum].enemyNum.Count);

            enemynum = stageinfo[StageNum].enemyNum[num];

            var enemy = Pooling.GetEnemy(enemynum, _spawnPos);
            enemy.SetUpEnemy(this, _wayPoint, canvas,hpbar,damagenum, water);
            enemy.SetPooling(Pooling, enemynum);
            enemy.gameObject.layer = 6;
            enemy.StartMove();

 

            //소환되는 enemy를 list에 추가
            EnemyCount.Add(enemy.GetComponent<Enemy>());
            EnemyRemainCount++;

            yield return new WaitForSeconds(stageinfo[StageNum].SpawnTime);
        }
        SpawnFinish = true;

        if (spawnNum == 1)
        {
            while (true)
            {
                if (SpawnFinish && EnemyRemainCount == 0)
                {
                    StageNum++;

                    stageclear();

                    if (StageNum >= stageinfo.Length)
                    {
                        ClearPanal.SetActive(true);

                        speedSet.StopGame();

                        UserInformation.getMoney += (int)(GameManager.SetMoney * SkillSettings.PassiveValue("GetUserCoinUp"));

                        PlusCoin1.text = "획득코인 : " + (int)(GameManager.SetMoney * SkillSettings.PassiveValue("GetUserCoinUp"));

                        //별 개수에 따른 상금 얻기
                    }

                    ShowBoss.enabled = false;
                    gameongoing = false;
                    ShowEnemyImageReset();
                    ShowEnemyImage(StageNum);
                    break;
                }
                yield return null;
            }
        }
    }

    public bool GetGameOnGoing => gameongoing;


    //출현한 적이 체력이 다 되서 죽을 때
    public void EnemyDie(Enemy enemy,int coin)
    {
        enemy.gameObject.layer = 0;
        playerstate.PlayerCoinUp(coin + Mathf.CeilToInt(coin * EnemyCoinRate));
        EnemyCount.Remove(enemy);
        EnemyRemainCount--;
    }

    //출현한 적이 도착지에 도착했을 때
    public void EnemyArriveDestination(Enemy enemy)
    {
        if (!enemy.GetBoss())
        {
            playerstate.PlayerLifeDown(1);
        }
        else
        {
            playerstate.PlayerLifeDown(5);
        }
        EnemyCount.Remove(enemy);
        EnemyRemainCount--;

        if (playerstate.GetPlayerLife <= 0)
        {
            speedSet.StopGame();
            FailPanal.SetActive(true);
        }
    }

    public bool GameOnGoing
    {
        get
        {
            return gameongoing;
        }
    }

    Vector2 BossTextPos;
    private void ShowEnemyImage(int num)
    {
        for(int i =0;i< stageinfo[num].enemyNum.Count; i++)
        {
            imageEnemy[i].gameObject.SetActive(true);


            Sprite enemy = Resources.Load<Sprite>("Image/EnemyImage/" + Pooling.GetName(stageinfo[num].enemyNum[i]));
            imageEnemy[i].sprite = enemy;

            imageEnemy[i].rectTransform.anchoredPosition = new Vector2((-70 * (stageinfo[num].enemyNum.Count - 1)) + (i * 140), 460);

            if (Pooling.GetBoss(stageinfo[num].enemyNum[i]))
            {
                ShowBoss.enabled = true;
                Debug.Log("???");
            }

            ShowBoss.rectTransform.anchoredPosition = new Vector2((-70 * (stageinfo[num].enemyNum.Count - 1)) + (i * 140), 380);

        }

        float size = stageinfo[num].enemyNum.Count > 1 ? (Mathf.Abs(imageEnemy[0].rectTransform.rect.x) + Mathf.Abs(imageEnemy[stageinfo[num].enemyNum.Count - 1].rectTransform.rect.x)) : 0;

        float size2 = (Mathf.Abs(imageEnemy[0].rectTransform.anchoredPosition.x) + Mathf.Abs(imageEnemy[stageinfo[num].enemyNum.Count - 1].rectTransform.anchoredPosition.x));

        imagebar.rectTransform.sizeDelta = new Vector2(size2, 20);
    }

    private void ShowEnemyImageReset()
    {
        for(int i = 0; i < imageEnemy.Length; i++)
        {
            imageEnemy[i].gameObject.SetActive(false);
        }
    }




}
