using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class StageInfo
{
    public float spawnTime = 0;
    public int spawnCount;

    public int[] enemyKind;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private MultipleSpeed speedSet = null;

    [SerializeField] private GameObject ClearPanal = null;
    [SerializeField] private TextMeshProUGUI PlusCoin1 = null;
    [SerializeField] private GameObject FailPanal = null;
    [SerializeField] private TextMeshProUGUI PlusCoin2 = null;
    
    [SerializeField] private Transform water = null;
    [SerializeField] private GameObject unituiParent = null;

    //���� �װų� �������� �������� ��, ���ȹ���̳� ������ ���Ҹ� ���� �÷��̾� ����
    [SerializeField] private PlayerState playerstate = null;

    //enemy�� ü�¹ٿ� ���ݽ� �������� ���� UI������ ���� canvas
    [SerializeField] private Transform canvas = null;

    //��ȯ�Ǵ� ������ �ο��� ������
    [SerializeField] private GameObject hpbar = null;
    [SerializeField] private GameObject damagenum = null;
    
    [SerializeField] private WeatherSetting weather =  null;

    //������ ���� ���� ǥ���� UI
    [SerializeField] private Image imagebar = null;
    [SerializeField] private Image[] imageEnemy = null;
    [SerializeField] private TextMeshProUGUI ShowBoss = null;

    private StageDataFrame stageData = null;

    public delegate void StageClear();
    public static StageClear stageclear;

    private float EnemyCoinRate = 0;
    
    private int StageNum = 0;
    public int GetStageNum => StageNum;

    //���������� ���������� �Ǵ�
    private bool isGameOnGoing = false;
    //�� ������ �������� ����
    private bool isSpawnFinish = false;

    private AlertSetting alert = new AlertSetting();

    private Vector3[] wayPoint;
    private Vector3 spawnPos;

    //��ȯ�Ǵ� ������ ������ ���� List
    //List<Enemy> EnemyCount = null;

    int EnemyCount = 0;
    int EnemyRemainCount = 0;

    private EnemyPooling Pooling = null;
 
    public int Getmaxstage => stageData.stageCount;
    public int Getcurrentstage => StageNum + 1;

    private void Awake()
    {
        Debug.Log(GameManager.GetStageData());
        stageData = GameManager.GetStageData();
    }
    
    private void Start()
    {
        Pooling = this.GetComponent<EnemyPooling>();
        MultipleSpeed.speedup += SpeedUP;
        ClearPanal.SetActive(false);
        FailPanal.SetActive(false);
        ShowEnemyImage(0);
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }


    //���� ���� �� �� enemy�� ��Ʈ�� ���� ��ġ�� �޾Ƽ� ���� ����
    public void gameStartCourtain(Vector3[] _waypoint, Vector3 _SpawnPos,int spawnNum)
    {
        wayPoint = _waypoint;
        spawnPos = _SpawnPos;
        StartCoroutine(GameStart(_waypoint, _SpawnPos,spawnNum));
    }

    IEnumerator GameStart(Vector3[] _wayPoint, Vector3 _spawnPos,int spawnNum)
    {
        GameManager.buttonOff();

        weather.GameStart();

       isGameOnGoing = true;

        //���� ���� ����
        int count = GameManager.SetGameLevel == 3 ? (int)(stageData.roundData[StageNum].spawnCount * 0.5f) : stageData.roundData[StageNum].spawnCount;

        EnemyRemainCount = GameManager.SetGameLevel==3?count * 2:count;

        int stagenum = StageNum;

        //�� ����
        for (int i = 0; i < count; i++)
        {
            int enemynum = 0;

            int num = Random.Range(0, stageData.roundData[StageNum].enemyKind.Length);

            enemynum = stageData.roundData[StageNum].enemyKind[num];

            var enemy = Pooling.GetEnemy(enemynum, _spawnPos);
            enemy.SetUpEnemy(this, _wayPoint, canvas,hpbar,damagenum, water);
            enemy.SetPooling(Pooling, enemynum);
            enemy.gameObject.layer = 6;
            enemy.StartMove();

            yield return new WaitForSeconds(stageData.roundData[StageNum].spawnTime);
        }
        isSpawnFinish = true;

        if (spawnNum == 1)
        {
            while (true)
            {
                if (isSpawnFinish && EnemyRemainCount <= 0)
                {
                    StageNum++;

                    stageclear();

                    if (StageNum >= stageData.roundData.Length)
                    {
                        alert.PlaySound(AlertKind.Victory,this.gameObject);
                        ClearPanal.SetActive(true);

                        speedSet.StopGame();

                        UserInformation.getMoney += (int)(GameManager.SetMoney * SkillSettings.PassiveValue("GetUserCoinUp"));

                        PlusCoin1.text = "ȹ������ : " + (int)(GameManager.SetMoney * SkillSettings.PassiveValue("GetUserCoinUp"));

                        //�� ������ ���� ��� ���
                    }

                    ShowBoss.enabled = false;
                    isGameOnGoing = false;
                    ShowEnemyImageReset();

                    if (StageNum < 20)
                    {
                        ShowEnemyImage(StageNum);
                    }
                    break;
                }
                yield return null;
            }
        }
    }

    public bool GetGameOnGoing => isGameOnGoing;


    //������ ���� ü���� �� �Ǽ� ���� ��
    public void EnemyDie(Enemy enemy,int coin)
    {
        Pooling.GetCoin(0, enemy.transform.position);
        
        enemy.gameObject.layer = 0;
        playerstate.PlayerCoinUp(coin + Mathf.CeilToInt(coin * EnemyCoinRate));
        EnemyRemainCount--;
    }

    //������ ���� �������� �������� ��
    public void EnemyArriveDestination(Enemy enemy)
    {
        Pooling.GetCoin(1, enemy.transform.position);
        alert.PlaySound(AlertKind.bubu,this.gameObject);
        if (!enemy.GetBoss())
        {
            playerstate.PlayerLifeDown(1);
        }
        else
        {
            playerstate.PlayerLifeDown(5);
        }
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
            return isGameOnGoing;
        }
    }

    Vector2 BossTextPos;
    private void ShowEnemyImage(int num)
    {
        int MaxCount = stageData.roundData[num].enemyKind.Length;

        for (int i =0;i< MaxCount; i++)
        {
            imageEnemy[i].gameObject.SetActive(true);


            Sprite enemy = Resources.Load<Sprite>("Image/EnemyImage/" + Pooling.GetName(stageData.roundData[num].enemyKind[i]));
            imageEnemy[i].sprite = enemy;

            imageEnemy[i].rectTransform.anchoredPosition = new Vector2((-70 * (stageData.roundData[num].enemyKind.Length - 1)) + (i * 140), 460);

            if (Pooling.GetBoss(stageData.roundData[num].enemyKind[i]))
            {
                ShowBoss.enabled = true;
            }

            ShowBoss.rectTransform.anchoredPosition = new Vector2((-70 * (stageData.roundData[num].enemyKind.Length - 1)) + (i * 140), 380);

        }

        float size = stageData.roundData[num].enemyKind.Length > 1 ? (Mathf.Abs(imageEnemy[0].rectTransform.rect.x) + Mathf.Abs(imageEnemy[stageData.roundData[num].enemyKind.Length - 1].rectTransform.rect.x)) : 0;

        float size2 = (Mathf.Abs(imageEnemy[0].rectTransform.anchoredPosition.x) + Mathf.Abs(imageEnemy[stageData.roundData[num].enemyKind.Length - 1].rectTransform.anchoredPosition.x));

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
