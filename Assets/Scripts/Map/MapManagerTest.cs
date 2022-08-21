using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapManagerTest : MonoBehaviour
{
    [SerializeField] private GameObject CheckPoint = null;
    [SerializeField] private GameObject[] NumCheckPoint = null;
    int checkPointCount = 0;
    Node[] checknode;


    [SerializeField] private GameObject[] StartEnd = null;
    [SerializeField] private GameObject CamPos = null;
    private int startX = 0;
    private int startY = 0;

    private int start2X = 0;
    private int start2Y = 0;

    private int endX = 0;
    private int endY = 0;

    //tile 프리펩
    [SerializeField] private GameObject Tile;

    //전체 타일 사이즈크기
    //짝수 단위로 입력할 것
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    //초기 타일 모양을 설정할 타일 개수
    [SerializeField] private int initialGridX;
    [SerializeField] private int initialGridY;

    //단차에 따른 타일 색
    [SerializeField] private Material[] Tilecolor = null;

    //타일들의 부모가 될 빈 게임 오브젝트
    private GameObject parentgrid;

    //Node를 2차원 배열로 만들어 index값 부여
    private Node[,] grid;

    //길찾기에서 시작과 끝 노드
    private Node StartNode;
    private Node StartNode2;
    private Node EndNode;

    private List<Node> useableNode = new List<Node>();

    //    public EnemyManager EM = null;

    MapShapeMake mapshape = new MapShapeMake();

    private SelectPoint selectpoint = new SelectPoint();

    private void Awake()
    {
        checkPointCount = GameManager.SetGameLevel+1;
        checknode = new Node[checkPointCount];

        Mapmake();
        MakeHeight();

        this.GetComponent<AddTile>().Settings(gridX, gridY, grid, Tilecolor, useableNode);

        this.GetComponent<Route>().Settings(gridX, gridY, grid, StartNode,StartNode2, EndNode);

        for(int i = 0; i < gridY; i++)
        {
            for(int j = 0; j < gridX; j++)
            {
                grid[i, j].Test();
            }
        }

        Instantiate(StartEnd[0], new Vector3(StartNode.GetX, StartNode.GetYDepth/2, StartNode.GetY), Quaternion.Euler(0, 90, 0));
        Instantiate(StartEnd[1], new Vector3(EndNode.GetX, EndNode.GetYDepth/2, EndNode.GetY), Quaternion.Euler(0, 90, 0));

        if (GameManager.SetGameLevel == 3&&GameManager.GetSetStageType == StageType.Nomal)
        {
            Instantiate(StartEnd[0], new Vector3(StartNode2.GetX, StartNode2.GetYDepth/2, StartNode2.GetY), Quaternion.Euler(0, 90, 0));
        }

        //체크포인트 생성
        if (GameManager.GetSetStageType == StageType.UnOrderCheckPoint)
        {
            for (int i = 0; i < checkPointCount; i++)
            {
                Instantiate(CheckPoint, new Vector3(checknode[i].GetX, checknode[i].GetYDepth / 2, checknode[i].GetY), Quaternion.Euler(0, 90, 0));
            }
        }
        else if(GameManager.GetSetStageType == StageType.OrderCheckPoint)
        {
            for (int i = 0; i < checkPointCount; i++)
            {
                Instantiate(NumCheckPoint[i], new Vector3(checknode[i].GetX, checknode[i].GetYDepth / 2, checknode[i].GetY), Quaternion.Euler(0, 90, 0));
            }
        }


    }




    private void MakeHeight()
    {
        int Count = Random.Range(20, 30);
        int[,] temp = { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };
        for (int i = 0; i < Count; i++)
        {
            //랜덤 높이값
            int RanNum = Random.Range(2, 5);
            int Near = Random.Range(0, 3);

            int Xnum = Random.Range(0, gridX);
            int Ynum = Random.Range(0, gridY);
            grid[Xnum, Ynum].ChangeNodeHeight(grid[Xnum, Ynum].neighbournode, RanNum);
            if (Near >= 1)
            {
                while (Near >= 1)
                {
                    Near = Random.Range(0, 3);
                    int RanNum2 = Random.Range(0, 4);
                    if (Xnum + temp[RanNum2, 0] < gridX && Xnum + temp[RanNum2, 0] >= 0 && Ynum + temp[RanNum2, 1] < gridY && Ynum + temp[RanNum2, 1] >= 0)
                    {
                        grid[Xnum + temp[RanNum2, 0], Ynum + temp[RanNum2, 1]].ChangeNodeHeight(grid[Xnum + temp[RanNum2, 0], Ynum + temp[RanNum2, 1]].neighbournode, RanNum);
                    }
                }
            }

        }
    }



    private void Mapmake()
    {
        //게임 시작시 타일맵을 만드는 함수

        //Node를 담을 이차원배열 grid의 크기 설정 (직사각형)
        grid = new Node[gridX, gridY];

        if (parentgrid != null) Destroy(parentgrid);
        parentgrid = new GameObject("ParentGrid");

        //중심점을 기준으로 양옆,위아래로 타일을 배치하기 위해 전체 길이의 절반 계산
        int halfgridx = gridX / 2;
        int halfgridy = gridY / 2;

        for (int i = 0; i < gridY; i++)
        {
            for (int j = 0; j < gridX; j++)
            {
                GameObject tile = Instantiate(Tile, new Vector3(j, 0, i), Quaternion.identity);
                tile.transform.parent = parentgrid.transform;
                //생성된 타일을 ParentGrid의 자식으로 넣어서 관리

                //생성과 동시에 인덱스 번호 부여

                grid[i, j] = tile.GetComponent<Node>();
                tile.GetComponent<Node>().Setnode(tile, false, j, i, false);
                tile.GetComponent<Node>().SetColor(Tilecolor);

                //Monobehaviour을 상속받은 스크립트는 new키워드를 사용할 수 없음.
                //사용하기 위해서는 어떠한 게임오브젝트에 component로 붙어있어야 함.

                //해결방법 1. 스크립트를 오브젝트에 붙인다.
                //해결방법 2. monobegaviour을 사용하지 않는다면 없앤다.

                //https://etst.tistory.com/32

                tile.gameObject.SetActive(false);
                //생성과 동시에 모두 비활성화
            }
        }

            /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////
            
            //노드의 이웃노드 저장
            for (int Y = 0; Y < gridY; Y++)
            {
                for (int X = 0; X < gridX; X++)
                {
                    List<Node> node2 = new List<Node>();
                    int[,] temp = { { -1, 1 }, { 0, 1 }, { 1, 1 }, { -1, 0 }, { 1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 } };
                    for (int k = 0; k < 8; k++)
                    {
                        int checkX = X + temp[k, 0];
                        int checkY = Y + temp[k, 1];

                        if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY)
                        {
                            node2.Add(grid[checkY, checkX]);
                        }
                    }
                    grid[Y, X].GetNeighbournode(node2);
                }
            }

        /////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////

        int widthcount = initialGridX;

        int xcount = initialGridX - 1;

        #region MapShape
        //모양대로 타일 활성화

        int RanX = Random.Range(10, gridX - 10);
        int RanY = Random.Range(10, gridY - 10);

        bool[,] activeTileList = new bool[10, 10];

        for(int i = 0; i<LoadMap.map.newshapes.Count; i++)
        {
            if(GameManager.SetStageShape == LoadMap.map.newshapes[i].name)
            {
                activeTileList = LoadMap.map.newshapes[i].tilelist;
                break;
            }
        }

        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                grid[i + RanY, j + RanX].gameObject.SetActive(activeTileList[j, i]);

                if (activeTileList[j, i])
                {
                    useableNode.Add(grid[i + RanY, j + RanX]);
                }
            }
        }

        CamPos.transform.position = new Vector3(RanX+5f, CamPos.transform.position.y, RanY+2.5f);

        #endregion


        //https://kinanadel.blogspot.com/2018/09/c.html

        //시작지점, 종료지점 만들기

        StartNode = selectpoint.GetStartNodePoint(useableNode);

        EndNode = selectpoint.GetEndNodePoint(useableNode, StartNode);

        StartNode.ChangeColor(Color.red);
        EndNode.ChangeColor(Color.blue);

        startX = StartNode.GetY;
        startY = StartNode.GetX;
        endX = EndNode.GetY;
        endY = EndNode.GetX;

        //난이도가 상 일때, 시작 지점 추가 생성
        if (GameManager.SetGameLevel == 3 && GameManager.GetSetStageType == StageType.Nomal)
        {
            StartNode2 = selectpoint.GetCheckNodePoint(useableNode);
            StartNode2.SetStartNode();
            StartNode2.ChangeColor(Color.red);
            useableNode.Remove(StartNode2);
            StartNode2.Getwalkable = true;
        }

        //체크 포인트 모드
        if (GameManager.GetSetStageType == StageType.UnOrderCheckPoint)
        {
            Node[] checknode2 = new Node[checkPointCount];

            for (int i = 0; i < checkPointCount; i++)
            {
                Node node = selectpoint.GetCheckNodePoint(useableNode);

                checknode2[i] = node;

                checknode[i] = node;

                useableNode.Remove(node);

                node.GetSetPoint = 1;
                node.GetSetCheckNode = true;
            }

            for (int i = 0; i < checkPointCount; i++)
            {
                useableNode.Add(checknode2[i]);
            }
        }
        else if (GameManager.GetSetStageType == StageType.OrderCheckPoint)
        {
            Node[] checknode2 = new Node[checkPointCount];

            for (int i = 0; i < checkPointCount; i++)
            {
                Node node = selectpoint.GetCheckNodePoint(useableNode);

                checknode2[i] = node;

                checknode[i] = node;

                useableNode.Remove(node);
                node.GetSetCheckNode = true;
                node.GetSetPoint = i + 1;
            }
                for (int i = 0; i < checkPointCount; i++)
                {
                    useableNode.Add(checknode2[i]);
                }
        }

    }

    #region Properties
    //////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////

    public int GetgridX
    {
        get
        {
            return gridX;
        }
    }

    public int GetgridY
    {
        get
        {
            return gridY;
        }
    }

    public Material[] GetMaterials
    {
        get
        {
            return Tilecolor;
        }
    }

    public Node[,] GetGrid
    {
        get
        {
           return grid;
        }
    }

    public Node GetStartNode
    {
        get
        {
            return StartNode;
        }
    }
    public Node GetEndNode
    {
        get
        {
            return EndNode;
        }
    }
    #endregion
}

