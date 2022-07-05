using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSetting : MonoBehaviour
{
    [SerializeField] private AddTile mapmanager = null;

    [SerializeField] private Renderer SkyBox = null;
    [SerializeField] private Renderer Sea = null;

    //받아온 활성화 리스트
    private List<Node> tilelist = new List<Node>();

    //사용가능한 node리스트
    private List<Node> checkednodelist = new List<Node>();

    //생성된 나무 리스트
    [SerializeField] private GameObject[] MakeTreeObj = null;
    private List<TreeSc> treelist = new List<TreeSc>();

    private List<Tree> treelistN = new List<Tree>();

    [SerializeField] private GameObject water = null;
    [SerializeField] private GameObject waterTrigger = null;

    [SerializeField] private ParticleSystem Rain = null;
    private bool rained = false;

    private int rainRate = 10;

    //private bool treechanged = false;

    // 스테이지가 넘어가면 랜덤으로 나무 생성 0~3개
    // 나무가 생성되면 리스트에 저장
    // 저장된 나무들의 나이 ++ => 나이가 짝수이면 다음 나무로 진화
    
    // 비가 오면 나무들의 성장이 2배, 해수면 상승

    // 사용 가능한 node리스트 필요

    // 스테이지가 변화하는지 알 수 있어야 함

    private void Start()
    {
        EnemyManager.stageclear += StageClear;
        
    }

    private void OnDestroy()
    {
        EnemyManager.stageclear -= StageClear;
    }

    //게임준비 화면에 작동할 메서드
    public void StageClear()
    {
        tilelist = mapmanager.GetActiveNode;

        //나무들의 나이 변경
        //나무 생성
        for (int i = 0; i < treelistN.Count; i++)
        {
            if (!rained)
            {
                treelistN[i].EvolveTree(1);
            }
            else
            {
                //비가오면 나무들이 더 빨리 자람
                treelistN[i].EvolveTree(2);
            }
        }

        InsTree();

        //강수 확률 증가
        rainRate += 2;
        if (rainRate > 100)
        {
            rainRate = 100;
        }

        RainTF();
    }

    //게임시작 화면에 작동할 메서드

    //나무 생성
    private void InsTree()
    {
        int Count = Random.Range(0, 3);

        for(int i = 0; i < Count; i++)
        {
            CheckEmptyNode();

            if(checkednodelist.Count == 0)
            {
                return;
            }

            int num = Random.Range(0, checkednodelist.Count);
            int treeNum = Random.Range(0, MakeTreeObj.Length);

            var newTree = Instantiate(MakeTreeObj[treeNum], new Vector3(checkednodelist[num].gridX, checkednodelist[num].GetYDepth / 2, checkednodelist[num].gridY), Quaternion.identity);
            newTree.GetComponent<Tree>().SetNode = checkednodelist[num];
            

            Debug.Log(checkednodelist[num].gridX + " : "  + checkednodelist[num].gridY);

            checkednodelist[num].OnBranch();

            treelistN.Add(newTree.GetComponent<Tree>());
            newTree.GetComponent<Tree>().SetWs = this;
        }

    }

    public void RemoveTree(Tree tree)
    {
        treelistN.Remove(tree);
    }

    public void RainTF()
    {
        int rain = Random.Range(1, 101);

        if (rain <= rainRate)
        {
            rained = true;
            Rain.Play();
            SkyBox.sharedMaterial.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
            Sea.sharedMaterial.SetFloat("_RimPower", 10);
            BuildManager.Rained(true);
        }
        else
        {
            rained = false;
            Rain.Stop();
            SkyBox.sharedMaterial.SetColor("_Tint", new Color(0.65f, 0.65f, 0.65f));
            Sea.sharedMaterial.SetFloat("_RimPower", 5);
            BuildManager.Rained(false);
        }
    }

    //사용가능한 node 확인
    private void CheckEmptyNode()
    {
        checkednodelist.Clear();
        for(int i = 0; i < tilelist.Count; i++)
        {
            if (GameManager.SetGameLevel == 1)
            {
                if (!tilelist[i].GetOnTower && !tilelist[i].Getwalkable&&!tilelist[i].SetOnObstacle)
                {
                    checkednodelist.Add(tilelist[i]);
                }
            }
            else
            {
                if (!tilelist[i].GetOnTower&& !tilelist[i].SetOnObstacle)
                {
                    checkednodelist.Add(tilelist[i]);
                }
            }
        }
        Debug.Log(checkednodelist.Count);
    }

    public void GameStart()
    {
        if (rained) UpSeaLevel();
        else if (!rained) DryWater();
    }

    public void UpSeaLevel()
    {
        if (rained)
        {
            water.transform.position += new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale += new Vector3(0, 0.5f, 0);
        }
    }

    private void DryWater()
    {
        if(water.transform.position.y > 0.625f)
        {
            water.transform.position -= new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale -= new Vector3(0, 0.5f, 0);
        }
    }

}
