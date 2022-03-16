using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    //Node가 가지는 게임오브젝트
    private GameObject tile;

    //길찾기에서 이동 가능한지 여부
    private bool walkable = false;

    //Node의 인덱스 번호
    public int gridX;
    public int gridY;

    private float YDepth;

    //길찾기의 시작과 끝
    public bool start = false;
    public bool end = false;

    //현재 타일위에 타워가 있는지 여부
    [SerializeField] private bool ontower = false;

    //이동비용
    private int gCost;
    //시작지점부터 새로운 노드까지의 이동비용

    private int hCost;
    //새로운 노드부터 끝지점 까지의 이동비용

    public Node parent;
    //찾은 길을 역으로 올라갈 때 사용할 부모 노드

    private bool ActiveCheck = false;

    //타일의 색을 저장할 Material배열
    private Material[] tilecolor;

    //현재 타일이 walkable인지 아닌지에 따라 달리할 색
    private Color walkablecolorT;
    private Color walkablecolorF;



    public void SetColor(Material[] color)
    {
        tilecolor = color;
    }

    /// ////////////////////////////////////
    /// ////////////////////////////////////

    private bool alreadymove = true;
    //private bool DDDD = true;
    public List<Node> neighbournode;

    

    //생성자 대신 사용해서 노드 값을 초기화 시켜줌
    public void Setnode(GameObject Tile, bool _walkable, int _gridX, int _gridY, bool onTower)
    {
        ontower = onTower;
        tile = Tile;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }


    private void Awake()
    {
        //alreadymove = true;
        //DDDD = true;
    }

    private void Start()
    {
        walkablecolorT = Color.red;
        Invoke("ColorChange2", 1.0f);
    }

    //public bool GetAlreadymove
    //{
    //    get
    //    {
    //        return alreadymove;
    //    }
    //}

    public void OriginColor()
    {
        if(!start&&!end)
        this.GetComponentInChildren<MeshRenderer>().material.color = walkablecolorF;
    }


    //타일의 인근 높이를 정하기 위해 타일마다 주위 노드의 정보를 가지는 함수
    public void GetNeighbournode(List<Node> _neighbournode)
    {
        neighbournode = _neighbournode;
    }

    float ydepth = 0;



    public void UpDownTile(List<Node> _neighbournode, float _Ydepth)
    {
       
        //이미 높이가 변한 적이 있는지
        alreadymove = true;

        //y축 크기를 바꾸기 위한 변수선언
        Vector3 thislocalscale = this.transform.localScale;

        //현재 노드의 y값 초기화
        ydepth = _Ydepth;

       

        //최고 높이를 기준으로 타일의 높이 정렬
        if (_Ydepth >= thislocalscale.y)
        {
            thislocalscale.y = ydepth;
        }

        //시작 높이를 기준으로 현재 높이를 변경
        this.transform.localScale = thislocalscale;

        //현재 Node기준 이웃의 높이값
        float neighbourYscale = thislocalscale.y - 0.5f;

        //이웃 노드개수 만큼 for문을 돌려 이웃의 높이를 설정
        for (int i = 0; i < _neighbournode.Count; i++)
        {
            Vector3 neighbourpos = _neighbournode[i].transform.localScale;

            float neiYdepth = neighbourpos.y;

            float yhightgap = thislocalscale.y - neiYdepth;

            if (_neighbournode[i].alreadymove && yhightgap <= 0.5f) continue;

            //타일의 크기가 바뀐적이 없고, 바뀔 높이가 0보다 클 때
            if (!_neighbournode[i].alreadymove && neighbourYscale > 0 /*&& thislocalscale.y > neighbourpos.y*/)
            {
                _neighbournode[i].alreadymove = true;
                neighbourpos.y = neighbourYscale;
                _neighbournode[i].transform.localScale = neighbourpos;
            }

            //바뀐적이 있고, 갭이 0.5이상 차이가 난다면
            else if (_neighbournode[i].alreadymove && yhightgap > 0.5f)
            {
                neighbourpos.y = neighbourYscale;
                _neighbournode[i].transform.localScale = neighbourpos;
            }

            //현재 y값이 0.5f보다 크고(밑에서 2번째 이상), 현재 타일의 높이가 이웃 타일의 높이보다 클 때
            if (ydepth > 0.5f && thislocalscale.y > neighbourpos.y)
            {
                _neighbournode[i].UpDownTile(_neighbournode[i].neighbournode, neighbourYscale);
            }

        }

        //for (int i = 0; i < _neighbournode.Count; i++)
        //{
        //    Vector3 neighboutscale = _neighbournode[i].transform.localScale;

        //    if (ydepth > 0.5f && thislocalscale.y > neighboutscale.y)
        //    {
        //        _neighbournode[i].UpDownTile(_neighbournode[i].neighbournode, neighbourYscale);
        //    }
        //}
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


    public void ChangeWalkableColor(bool _walkable)
    {
        if (!ontower)
        {
            if (!start && !end)
            {
                //이미 바뀐 적이 있는 타일인지 확인
                walkable = _walkable;
                if (walkable)
                {
                    ChangeColor(walkablecolorT);
                    color = walkablecolorT;
                }

                else if (!walkable)
                {
                    ChangeColor(walkablecolorF);
                    color = walkablecolorF;
                }

            }
        }
    }

    private Color color = Color.yellow;
    public void ReturnColor()
    {
        ChangeColor(walkablecolorT);
    }

    public Color GetColor
    {
        get
        {
            return color;
        }
    }

    public void ChangeColor(Color col)
    {
        this.GetComponentInChildren<MeshRenderer>().material.color = col;
    }

    public void ColorChange2()
    {
        Vector3 thisscale = this.transform.localScale;
        int x = (int)thisscale.y;

        if (!start && !end)
        {
            switch (x)
            {
                case 1:
                    ChangeColor(tilecolor[0].color);
                  //  Debug.Log("111");
                    color = tilecolor[0].color;
                    walkablecolorF = tilecolor[0].color;
  //                  walkablecolorT = tilecolor[7].color;
                    break;
                case 2:
                    ChangeColor(tilecolor[1].color);
                    color = tilecolor[1].color;
                    walkablecolorF = tilecolor[1].color;
   //                 walkablecolorT = tilecolor[8].color;
                    break;
                case 3:
                    ChangeColor(tilecolor[2].color);
                    color = tilecolor[2].color;
                    walkablecolorF = tilecolor[2].color;
   //                 walkablecolorT = tilecolor[9].color;
                    break;
                case 4:
                    ChangeColor(tilecolor[3].color);
                    color = tilecolor[3].color;
                    walkablecolorF = tilecolor[3].color;
  //                  walkablecolorT = tilecolor[10].color;
                    break;
                case 5:
                    ChangeColor(tilecolor[4].color);
                    color = tilecolor[4].color;
                    walkablecolorF = tilecolor[4].color;
    //                walkablecolorT = tilecolor[11].color;
                    break;
                case 6:
                    ChangeColor(tilecolor[5].color);
                    color = tilecolor[5].color;
                    walkablecolorF = tilecolor[5].color;
     //               walkablecolorT = tilecolor[12].color;
                    break;
                case 7:
                    ChangeColor(tilecolor[6].color);
                    color = tilecolor[6].color;
                    walkablecolorF = tilecolor[6].color;
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

    public void ChangeTF(bool tf)
    {
        this.gameObject.SetActive(tf);
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

    public bool GetStartEnd
    {
        get
        {
            return (!end && !start);
        }
    }






}
