using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSetting : MonoBehaviour
{
    private enum weather
    {
        spring,
        summer,
        fall,
        winter
    }

    [SerializeField] private EnemyManager enemyManager = null;
    private int stageNum = 1;

    [SerializeField] private MapManager mapmanager = null;

    //받아온 활성화 리스트
    private List<Node> tilelist = new List<Node>();

    //사용가능한 node리스트
    private List<Node> checkednodelist = new List<Node>();



    private weather weaTher = weather.spring;

    //생성된 나무 리스트
    [SerializeField] private GameObject MakeTreeObj = null;
    private List<TreeSc> treelist = new List<TreeSc>();

    [SerializeField] private GameObject water = null;
    [SerializeField] private GameObject waterTrigger = null;

    [SerializeField] private ParticleSystem Rain = null;
    private bool rained = false;
    [SerializeField] private ParticleSystem Snow = null;

    private bool treechanged = false;

    private void Update()
    {
        tilelist = mapmanager.GetActiveList;
    }


    private void WeatherChange()
    {
        
            weaTher = weather.spring;
        
        
        if (stageNum>=10)
        {
            weaTher = weather.summer;
        }
        //else if (stageNum <= 15)
        //{
        //    weaTher = weather.fall;
        //}
        //else if (stageNum <= 20)
        //{
        //    weaTher = weather.winter;
        //}
    }

    public void WeatherSettings()
    {
        stageNum++;
        WeatherChange();

        if (!rained)
        {
            DryWater();
        }

        TreeChange((int)weaTher);

        switch ((int)weaTher)
        {
            case 0:
                SpringAct();
                break;
            case 1:
                SummerAct();
                break;
            case 2:
                Rain.Stop();
                FallAct();
                break;
            case 3:
                WinterAct();
                break;
        }
    }

    private void SpringAct() 
    {
        //랜덤으로 몇 개가 생길지
        int i = Random.Range(1, 3);
        for(int j = 0; j < i; j++)
        {
            CheckEmptyNode();
            //생성될 노드의 index
            int num = Random.Range(0, checkednodelist.Count);

            GameObject Tree = Instantiate(MakeTreeObj, new Vector3(checkednodelist[num].gridX, checkednodelist[num].GetYDepth/2, checkednodelist[num].gridY), Quaternion.identity);
            treelist.Add(Tree.GetComponent<TreeSc>());
            Tree.GetComponent<TreeSc>().TreeChangeToSpring();
            Tree.GetComponent<TreeSc>().SetNode = checkednodelist[num];
            Tree.GetComponent<Obstacle>().SetNode = checkednodelist[num];
            Tree.GetComponent<TreeSc>().SetWeather = this;
            checkednodelist[num].SetOnObstacle = true;
            checkednodelist[num].GetOnTower = true;
        }
    }
    private void SummerAct() 
    {
        Debug.Log("여름");
        int rainprobabilty = Random.Range(0, 2);
        if(rainprobabilty < 1)
        {
            Rain.Play();
            rained = true;
        }
        else
        {
            Rain.Stop();
            rained = false;
        }
    }
    private void FallAct() 
    {

        for (int i = 0; i < treelist.Count; i++)
        {
            treelist[i].FallAct();
        }
    }

    int XXXX = 0;
    bool iced = false;
    private void WinterAct() 
    {
        XXXX++;
        Snow.Play();
        if (XXXX > 3)
        {
            waterTrigger.GetComponent<WaterTrigger_>().watericed();

            if (!iced)
            {
                water.GetComponent<icedwater>().iced();
                water.GetComponent<SpriteRenderer>().enabled = false;
                iced = true;
            }
        }
            for(int i = 0;i< treelist.Count; i++)
        { 
            treelist[i].DisappearFruit();
        }


        for (int i = 0; i < treelist.Count; i++)
        {
            treelist[i].WinterAct();
        }
    }

    private void CheckEmptyNode()
    {
        checkednodelist = new List<Node>();
        for(int i = 0; i < tilelist.Count; i++)
        {
            if (!tilelist[i].GetOnTower && !tilelist[i].Getwalkable)
            {
                checkednodelist.Add(tilelist[i]);
            }
        }
    }

    private void RainDown()
    {
        Rain.Play();
    }

    public void UpSeaLevel()
    {
        if (rained)
        {
            water.transform.position += new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale += new Vector3(0, 0.5f, 0);
        }

        if(weaTher != weather.summer) rained = false;
    }

    private void RainStop()
    {
        Rain.Stop();
    }

    private void DryWater()
    {
        if(water.transform.position.y > 0.625f)
        {
            water.transform.position -= new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale -= new Vector3(0, 0.5f, 0);
        }
    }

    private void SnowDown()
    {
        Snow.Play();
    }

    private void StopSnow()
    {
        Snow.Stop();
    }

    public void treelistRemove(TreeSc tree,Node node)
    {
        treelist.Remove(tree);
        node.GetOnTower = false;
        node.SetOnObstacle = false;
    }

    private void TreeChange(int weather)
    {
        for(int i = 0; i < treelist.Count; i++)
        {
            switch (weather)
            {
                case 0:
                    treelist[i].TreeChangeToSpring();
                    break;
                case 2:
                    treelist[i].TreeChangeToFall();
                    break;
                case 3:
                    treelist[i].TreeChangeToWinter();
                    break;

            }
        }
    }


}
