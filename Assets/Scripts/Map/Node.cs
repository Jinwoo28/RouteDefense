using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    //Node�� ������ ���ӿ�����Ʈ
    private GameObject tile;

    //��ã�⿡�� �̵� �������� ����
    private bool walkable = false;

    //Ÿ���� Ȱ��ȭ �Ǿ��ִ���
    private bool checkActive = false;
    public bool GetSetActive { get => checkActive; set => checkActive = value; }

    //node���� ��ֹ��� �ִ���
    private bool OnObstacle = false;
    public bool SetOnObstacle { get => OnObstacle; set => OnObstacle = value; }

    private bool CheckNode = false;
    public bool GetSetCheckNode { get => CheckNode; set => CheckNode = value; }

    //Node�� �ε��� ��ȣ
    public int gridX;
    public int gridY;

    private float YDepth;

    //��ã���� ���۰� ��
    public bool start = false;
    public bool end = false;

    //���� Ÿ������ Ÿ���� �ִ��� ����
    private bool ontower = false;

    //�̵����
    private int gCost;
    //������������ ���ο� �������� �̵����

    private int hCost;
    //���ο� ������ ������ ������ �̵����

    public Node parent;
    //ã�� ���� ������ �ö� �� ����� �θ� ���

    private bool ActiveCheck = false;

    //Ÿ���� ���� ������ Material�迭
    private Material[] tilecolor;

    //���� Ÿ���� walkable���� �ƴ����� ���� �޸��� ��
    private Color walkablecolorT;
    private Color walkablecolorF;

    //���̿� ���� ���� ���� ����
    private Color color = Color.yellow;

    private int CheckNodePoint = 0;

    public int GetSetPoint { get => CheckNodePoint; set => CheckNodePoint = value; }

    public void SetColor(Material[] color)
    {
        tilecolor = color;
       
    }

    private bool alreadymove = true;

    public List<Node> neighbournode;

    

    //������ ��� ����ؼ� ��� ���� �ʱ�ȭ ������
    public void Setnode(GameObject Tile, bool _walkable, int _gridX, int _gridY, bool onTower)
    {
        ontower = onTower;
        tile = Tile;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }


    private void Start()
    {
        walkablecolorT = Color.red;
        ColorSetting();
    }

    public void SetStartColor()
    {

    }

    public void OriginColor()
    {
        if(!start&&!end)
        this.GetComponentInChildren<MeshRenderer>().material.color = walkablecolorF;
    }


    //Ÿ���� �α� ���̸� ���ϱ� ���� Ÿ�ϸ��� ���� ����� ������ ������ �Լ�
    public void GetNeighbournode(List<Node> _neighbournode)
    {
        neighbournode = _neighbournode;
    }

    [SerializeField] float ydepth = 1.0f;

    public void Test()
    {
        ydepth = this.transform.localScale.y;
    }

    public void ChangeNodeHeight(List<Node> _neighbournode, float _Ydepth)
    {
        //�̹� ���̰� ���� ���� �ִ���
        alreadymove = true;

        //y�� ũ�⸦ �ٲٱ� ���� ��������
        Vector3 thislocalscale = this.transform.localScale;

        //���� ����� y�� �ʱ�ȭ
        ydepth = _Ydepth;

        //�ְ� ���̸� �������� Ÿ���� ���� ����
        if (_Ydepth >= thislocalscale.y)
        {
            thislocalscale.y = ydepth;
        }

        //���� ���̸� �������� ���� ���̸� ����
        this.transform.localScale = thislocalscale;

        //���� Node���� �̿��� ���̰�
        float neighbourYscale = thislocalscale.y - 0.5f;

        //�̿� ��尳�� ��ŭ for���� ���� �̿��� ���̸� ����
        for (int i = 0; i < _neighbournode.Count; i++)
        {
            Vector3 neighbourpos = _neighbournode[i].transform.localScale;

            float neiYdepth = neighbourpos.y;

            float yhightgap = thislocalscale.y - neiYdepth;

            if (_neighbournode[i].alreadymove && yhightgap <= 0.5f) continue;

            //Ÿ���� ũ�Ⱑ �ٲ����� ����, �ٲ� ���̰� 0���� Ŭ ��
            if (!_neighbournode[i].alreadymove && neighbourYscale > 0)
            {
                _neighbournode[i].alreadymove = true;
                neighbourpos.y = neighbourYscale;
                _neighbournode[i].transform.localScale = neighbourpos;
            }

            //�ٲ����� �ְ�, ���� 0.5�̻� ���̰� ���ٸ�
            else if (_neighbournode[i].alreadymove && yhightgap > 0.5f)
            {
                neighbourpos.y = neighbourYscale;
                _neighbournode[i].transform.localScale = neighbourpos;
            }

            //���� y���� 0.5f���� ũ��(�ؿ��� 2��° �̻�), ���� Ÿ���� ���̰� �̿� Ÿ���� ���̺��� Ŭ ��
            if (ydepth > 0.5f && thislocalscale.y > neighbourpos.y)
            {
                _neighbournode[i].ChangeNodeHeight(_neighbournode[i].neighbournode, neighbourYscale);
            }
        }
    }

    


    public Vector3 ThisPos
    {
        get
        {
            Vector3 thispos = tile.transform.position;
            thispos.y += 1.0f;
            return thispos;
        }
    }



    public void SetActiveTile(bool X)
    {
        tile.SetActive(X);
        ActiveCheck = X;
    }

    public float GetYDepth{
        get
        {
            return ydepth;
        }
    }

    public void OnBranch()
    {
        if (!end && !start)
        {
            OnObstacle = true;
            walkable = false;
            ChangeColor(walkablecolorF);
        }
    }

    public void RemoveObs()
    {
        OnObstacle = false;
        walkable = false;
        ChangeColor(walkablecolorF);
    }




    public void ChangeWalkableColor(bool _walkable)
    {
        if (!ontower&&!OnObstacle)
        {
            if (!start && !end)
            {
                //�̹� �ٲ� ���� �ִ� Ÿ������ Ȯ��
                walkable = _walkable;
                if (walkable)
                {
                    ChangeColor(walkablecolorT);
                    //color = walkablecolorT;
                }

                else if (!walkable)
                {
                    ChangeColor(walkablecolorF);
                   // color = walkablecolorF;
                }

            }
        }
    }

    
    public void ReturnColor()
    {
        if (OnObstacle) ChangeColor(walkablecolorF);
        else ChangeColor(walkablecolorT);
    }

    public void ChangeColor(Color col)
    {
        this.GetComponentInChildren<MeshRenderer>().material.color = col;
    }

    public void ColorSetting()
    {
        Vector3 thisscale = this.transform.localScale;
        int x = (int)(thisscale.y*2)-1;


        if (!start && !end)
        {
            switch (x)
            {
                case 1:
                    ChangeColor(tilecolor[0].color);
                  //  Debug.Log("111");
                  //  color = tilecolor[0].color;
                    walkablecolorF = tilecolor[0].color;
  //                  walkablecolorT = tilecolor[7].color;
                    break;
                case 2:
                    ChangeColor(tilecolor[1].color);
                  //  color = tilecolor[1].color;
                    walkablecolorF = tilecolor[1].color;
   //                 walkablecolorT = tilecolor[8].color;
                    break;
                case 3:
                    ChangeColor(tilecolor[2].color);
                 //   color = tilecolor[2].color;
                    walkablecolorF = tilecolor[2].color;
   //                 walkablecolorT = tilecolor[9].color;
                    break;
                case 4:
                    ChangeColor(tilecolor[3].color);
                  //  color = tilecolor[3].color;
                    walkablecolorF = tilecolor[3].color;
  //                  walkablecolorT = tilecolor[10].color;
                    break;
                case 5:
                    ChangeColor(tilecolor[4].color);
                 //   color = tilecolor[4].color;
                    walkablecolorF = tilecolor[4].color;
    //                walkablecolorT = tilecolor[11].color;
                    break;
                case 6:
                    ChangeColor(tilecolor[5].color);
                 //   color = tilecolor[5].color;
                    walkablecolorF = tilecolor[5].color;
     //               walkablecolorT = tilecolor[12].color;
                    break;
                case 7:
                    ChangeColor(tilecolor[6].color);
                 //   color = tilecolor[6].color;
                    walkablecolorF = tilecolor[6].color;
   //                 walkablecolorT = tilecolor[13].color;
                    break;
                case 8:
                    ChangeColor(tilecolor[7].color);
                  //  color = tilecolor[7].color;
                    walkablecolorF = tilecolor[7].color;
                    //                 walkablecolorT = tilecolor[13].color;
                    break;
            }
           
        }
    }

    public void SetStartNode()
    {
        start = true;
    }

    public void SetEndNode()
    {
        end = true;
    }


    public int GetX
    {
        get
        {
            return gridX;
        }
    }

    public int GetY
    {
        get
        {
            return gridY;
        }
    }

    public bool CheckActiveTF
    {
        get
        {
            return ActiveCheck;
        }
    }

    public int GetgCost
    {
        get
        {
            return gCost;
        }
        set
        {
            gCost = value;
        }
    }

    public int GethCost
    {
        get
        {
            return hCost;
        }
        set
        {
            hCost = value;
        }
    }

    public int GetfCost
    {
        get
        {
            return gCost + hCost;
        }
   
    }

    public bool Getwalkable
    {
        get
        {
            return walkable;
        }
        set
        {
            walkable = value;
        }
    }

    public bool GetOnTower
    {
        get
        {
            return ontower;
        }
        set
        {
            ontower = value;
        }
    }







}
