using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorimage = null;
    [SerializeField]private PlayerState playerstate = null;

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
    private Node EndNode;

    /// ////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////

    //타일의 길 만들기 버튼 활성화 bool 값_ true일 때만 길이 만들어짐
    private bool TileCanChange = false;

    private HashSet<Node> overlapcheck = null;

    /// ////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////
   
    //AddTile프리펩
    [SerializeField] private GameObject AddTilePrefab;

    //추가될 테트리스 타일 넘버
    private int AddtileNum = 0;
    private int addtileprice = 0;

    public int GetAddTileNum => AddtileNum;
    public int GetAddtilePrice => addtileprice;

    private bool AddTileActive = false;
    private bool canaddtile = false;


    /// ////////////////////////////////////////////////
    /// ////////////////////////////////////////////////
    private bool isgameing = false;
    [SerializeField] private GameObject NotFound = null;

    private List<Node> waypointnode = null;
    private List<Node> activeNode = new List<Node>();
    public List<Node> GetActiveList => activeNode;

    public EnemyManager EM = null;

    private void Start()
    {
        waypointnode = new List<Node>();
        overlapcheck = new HashSet<Node>();
        TowerPreview.makerouteoff += makerouteoff;
    }

    private void makerouteoff()
    {
        TileCanChange = false;
    }

    private void Awake()
    {
   
        Mapmake();
        MakeHeight();

        AddtileNum = Random.Range(0, 7);
    }

    private void MakeHeight()
    {
        int Count = Random.Range(9, 10);
        int[,] temp = { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };
        for (int i = 0; i < Count; i++)
        {
            int RanNum = Random.Range(2, 5);
            int Near = Random.Range(0, 3);

            int Xnum = Random.Range(0, gridX);
            int Ynum = Random.Range(0, gridY);
            grid[Xnum, Ynum].UpDownTile(grid[Xnum, Ynum].neighbournode, RanNum);
            if (Near >= 1)
            {
                while (Near >= 1)
                {
                    Near = Random.Range(0, 3);
                    int RanNum2 = Random.Range(0, 4);
                    if (Xnum + temp[RanNum2, 0] < gridX && Xnum + temp[RanNum2, 0] >= 0 && Ynum + temp[RanNum2, 1] < gridY && Ynum + temp[RanNum2, 1] >= 0)
                    {
                        grid[Xnum + temp[RanNum2, 0], Ynum + temp[RanNum2, 1]].UpDownTile(grid[Xnum + temp[RanNum2, 0], Ynum + temp[RanNum2, 1]].neighbournode, RanNum);

                    }
                   
                }
            }

        }
    }


    private void Update()
    {
        isgameing = EM.GameOnGoing;

        if (TileCanChange)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Node node = ReturnNode();

                    if (node != null)
                    {
                       StartCoroutine(NodeWalkableChange(node));
                    }
                }
            }

        if (TileCanChange)
            Cursor.SetCursor(cursorimage, Vector2.zero, CursorMode.ForceSoftware);
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
                //grid[i, j] = new Node(tile,false, i, j, 0,false);

                grid[i, j] = tile.GetComponent<Node>();
                   // tile.AddComponent<Node>();
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

        int ycount = 0;
        int xcount = initialGridX - 1;



        //모양대로 타일 활성화
        for (int i = halfgridy - initialGridY; i < halfgridy + initialGridY; i++)
        {
            for (int j = halfgridx - widthcount; j < halfgridx + widthcount; j++)
            {
                grid[i, j].SetActiveTile(true);
                grid[i, j].GetSetActive = true;
                activeNode.Add(grid[i, j]);
            }

            //조건문을 이용해 가로타일의 개수 조절
            //ycount로 Y축 개수의 중간까지는 추가, 이후 다시 감소
            ycount++;

            if (ycount < initialGridY - xcount)
            {
                widthcount++;
            }

            //X축 시작 개수만큼 가운데 Y의 개수 설정(모양을 잡기위해)
            else if (ycount >= initialGridY - xcount && ycount <= initialGridY + xcount) { }
            else
            {
                widthcount--;
            }
        }


        //start와 end Node 랜덤 선정
        //1. 랜덤으로 start노드 선정
        //2. 랜덤으로 endNode의 인덱스값 추출
        //endNode의 Y축 노드값이 start와 같다면
        //endNode의 X축 노드값은 startNode 의 -1,0,1은 될수 없고
        //endNode의 Y축 노드값이 start와 다르다면
        //endNode의 X축 노드값은 startNode의 0이 될 수 없다.

        //특이점 : Y축에 해당되어있는 x축 타일의 개수를 알아야 한다.
        //https://kinanadel.blogspot.com/2018/09/c.html
        //    int RandomNum;

        //startNode 랜덤 인덱스값
        int SYnum = Random.Range(halfgridy - initialGridY, halfgridy + initialGridY);
        int SXnum = Random.Range(0, gridX);

        while (!grid[SYnum, SXnum].CheckActiveTF)
        {
            SXnum = Random.Range(0, gridX);
        }
        grid[SYnum, SXnum].SetStartNode();
        StartNode = grid[SYnum, SXnum];
        StartNode.Getwalkable = true;

        //endNode 랜덤 인덱스 값
        int EYnum = Random.Range(halfgridy - initialGridY, halfgridy + initialGridY);
        int EXnum = Random.Range(0, gridX);

        if (EYnum == SYnum)
        {
           // Debug.Log("11");
            //Y축이 같을 때

            //타일이 있어야 하고,
            //start x인덱스의 -1,0,+1이 아니어야 한다.

            while (!grid[EYnum, EXnum].CheckActiveTF || EXnum == SXnum || EXnum == (SXnum - 1) || EXnum == (SXnum + 1))
            {
                EXnum = Random.Range(0, gridX);
            }
            grid[EYnum, EXnum].SetEndNode();
            EndNode = grid[EYnum, EXnum];
            EndNode.Getwalkable = true;

        }
        else if (EYnum != SYnum)
        {
           // Debug.Log("22");
            //Y축이 다를 때
            while (!grid[EYnum, EXnum].CheckActiveTF || EXnum == SXnum)
            {
                EXnum = Random.Range(0, gridX);
            }
            grid[EYnum, EXnum].SetEndNode();
            EndNode = grid[EYnum, EXnum];
            EndNode.Getwalkable = true;
        }

        grid[SYnum, SXnum].ChangeColor(Color.red);
        grid[EYnum, EXnum].ChangeColor(Color.blue);

    }

    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //타일추가 함수
    public void OnClickMapAdd()
    {
        StartCoroutine("MapTileAdd");
    }

    IEnumerator MapTileAdd()
    {
        if (playerstate.GetSetPlayerCoin >= addtileprice)
        {
            //추가타일 가격만큼 플레이어 코인 감소
            playerstate.GetSetPlayerCoin = addtileprice;

            //타일의 방향 변수
            int tiledir = 0;
            
            //AddTile = true;
            Vector3 mousepos = Vector3.zero;
            Vector3 tilepos = Vector3.zero;

            GameObject[] AddtileX = new GameObject[4];

            //타일 4개 생성
            for (int i = 0; i < 4; i++)
            {
                AddtileX[i] = Instantiate(AddTilePrefab, mousepos, Quaternion.identity);
            }
                AddTileActive = true;

            while (AddTileActive)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //마우스 위치에 타일 생성

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // if(!hit.collider.CompareTag("Tile")&&hit.point.x>=0&& hit.point.x<gridX)
                    mousepos = hit.point;
                }

                if (AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].z < gridY
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].z < gridY
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].z < gridY
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].z < gridY
                    )
                {
                    //4개 타일의 위치
                    for (int i = 0; i < 4; i++)
                    {
                        int X = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].x;
                        int Y = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].z;
                       
                        AddtileX[i].transform.position = AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i];

                        Vector3 intmousepos = Vector3.zero;
                        intmousepos = grid[Y, X].GetComponent<Node>().transform.localScale;

                        AddtileX[i].transform.localScale = intmousepos;


                        //4개 타일의 크기와 색
                        if (AddtileX[0].GetComponent<Preview>().CanBuildable && AddtileX[1].GetComponent<Preview>().CanBuildable && AddtileX[2].GetComponent<Preview>().CanBuildable && AddtileX[3].GetComponent<Preview>().CanBuildable)
                        {
                            canaddtile = true;
                            switch ((int)(intmousepos.y*2)-1)
                            {
                                case 1:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[0].color;
                                    break;
                                case 2:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[1].color;
                                    break;
                                case 3:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[2].color;
                                    break;
                                case 4:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[3].color;
                                    break;
                                case 5:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[4].color;
                                    break;
                                case 6:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[5].color;
                                    break;
                                case 7:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[6].color;
                                    break;
                                case 8:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[7].color;
                                    break;
                            }
                        }
                        else
                        {
                            AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[8].color;
                            canaddtile = false;
                        }
                    }
                }

                if (AddTileActive)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        tiledir++;
                        if (tiledir >= 4) tiledir = 0;
                    }
                }

                if (canaddtile)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            //AddTile = false;
                            AddTileActive = false;

                            for (int j = 0; j < 4; j++)
                            {
                                int X = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].x;
                                int Y = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].z;
                                Destroy(AddtileX[j]);
                                grid[Y, X].GetComponent<Node>().SetActiveTile(true);
                                grid[Y, X].GetSetActive = true;
                                AddTileActive = false;
                                activeNode.Add(grid[Y,X]);
                            }
                            addtileprice += 50;
                            AddtileNum = Random.Range(0, 7);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //AddTile = false;
                    AddTileActive = false;
                    for (int i = 0; i < 4; i++)
                    {
                        Destroy(AddtileX[i]);
                    }
                    playerstate.GetSetPlayerCoin = -addtileprice;
                }
                yield return null;
            }
        }
    }

    //테트리스 모양의 블럭위치 반환
    private Vector3[] AddTilePos(int tileshapenum, int _tileDir, int Xnum, int Ynum)
    {
        Vector3[] tilelist = new Vector3[11];
        //블록의 타입, 블록의 회전횟수, 타일의 위치 저장

        Vector3[] returntilepos = new Vector3[4];

        /*
         
        모든 타일생성에 필요한 index번호
        0이 마우스 기준
        X 1 2  3
        4 5 0  6
        X 7 8  9
        X X 10 X
         */

        //타일 모양 / 변형가능횟수
        // O(0) I(1) S(2) Z(2) L(4) J(4) T(4)

        //타일모양 O     I          S           Z           L           J           T
        //□ □ □ □       □ □ ■ □    □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □
        //□ ■ ■ □       □ □ ■ □    □ □ ■ ■     □ ■ ■ □     □ ■ □ □     □ □ □ ■     □ □ ■ □
        //□ ■ ■ □       □ □ ■ □    □ ■ ■ □     □ □ ■ ■     □ ■ ■ ■     □ ■ ■ ■     □ ■ ■ ■
        //□ □ □ □       □ □ ■ □    □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □

        tilelist[0] = new Vector3(Xnum, 0, Ynum);
        tilelist[1] = new Vector3(Xnum - 1, 0, Ynum + 1);
        tilelist[2] = new Vector3(Xnum, 0, Ynum + 1);
        tilelist[3] = new Vector3(Xnum + 1, 0, Ynum + 1);
        tilelist[4] = new Vector3(Xnum - 2, 0, Ynum);
        tilelist[5] = new Vector3(Xnum - 1, 0, Ynum);
        tilelist[6] = new Vector3(Xnum + 1, 0, Ynum);
        tilelist[7] = new Vector3(Xnum - 1, 0, Ynum - 1);
        tilelist[8] = new Vector3(Xnum, 0, Ynum - 1);
        tilelist[9] = new Vector3(Xnum + 1, 0, Ynum - 1);
        tilelist[10] = new Vector3(Xnum, 0, Ynum - 2);

        switch (tileshapenum)
        {
            case 0:
                returntilepos[0] = tilelist[0];
                returntilepos[1] = tilelist[1];
                returntilepos[2] = tilelist[2];
                returntilepos[3] = tilelist[5];
                break;
            case 1:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[10];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returntilepos[0] = tilelist[4];
                    returntilepos[1] = tilelist[5];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[6];
                }
                break;
            case 2:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returntilepos[0] = tilelist[6];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[7];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[6];
                    returntilepos[3] = tilelist[9];
                }
                break;
            case 3:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returntilepos[0] = tilelist[5];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[9];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returntilepos[0] = tilelist[3];
                    returntilepos[1] = tilelist[6];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                break;
            case 4:
                if (_tileDir == 0)
                {
                    returntilepos[0] = tilelist[6];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[5];
                    returntilepos[3] = tilelist[7];
                }
                else if (_tileDir == 1)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[9];
                }
                else if (_tileDir == 2)
                {
                    returntilepos[0] = tilelist[3];
                    returntilepos[1] = tilelist[6];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[5];
                }
                else if (_tileDir == 3)
                {
                    returntilepos[0] = tilelist[1];
                    returntilepos[1] = tilelist[2];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                break;

            case 5:
                if (_tileDir == 0)
                {
                    returntilepos[0] = tilelist[5];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[6];
                    returntilepos[3] = tilelist[9];
                }
                else if (_tileDir == 1)
                {
                    returntilepos[0] = tilelist[3];
                    returntilepos[1] = tilelist[2];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                else if (_tileDir == 2)
                {
                    returntilepos[0] = tilelist[1];
                    returntilepos[1] = tilelist[5];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[6];
                }
                else if (_tileDir == 3)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[7];
                }
                break;
            /*

모든 타일생성에 필요한 index번호
0이 마우스 기준
X 1 2  3
4 5 0  6
X 7 8  9
X X 10 X
*/
            case 6:
                if (_tileDir == 0)
                {
                    returntilepos[0] = tilelist[5];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[6];
                    returntilepos[3] = tilelist[8];
                }
                else if (_tileDir == 1)
                {
                    returntilepos[0] = tilelist[6];
                    returntilepos[1] = tilelist[2];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                else if (_tileDir == 2)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[5];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[6];
                }
                else if (_tileDir == 3)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[5];
                }
                break;
        }

        /*if (AddTilePos2(returntilepos))*/
        return returntilepos;
        //else return null;


    }


    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //길 만들기 함수
    //타일 클릭시 해당 노드의 색과 walkable 바꾸기
    IEnumerator NodeWalkableChange(Node _changenode)
    {
        bool walkable = !_changenode.Getwalkable;

        while (Input.GetMouseButton(0))
        {
            Node changenode = ReturnNode();

            if (changenode != null)
            {
                ReturnNode().ChangeWalkableColor(walkable);

                if (walkable)
                {
                    overlapcheck.Add(changenode);
                }
            }
            yield return null;
        }
    }
    //버튼으로 길만들기 활성화, 비활성화 시킬 버튼함수
    public void OnClickWalkableChange()
    {
        if (!isgameing)
        {
            if (!AddTileActive)
                TileCanChange = !TileCanChange;
        }
    }




    public void RouteReset()
    {
        if (!isgameing)
        {
            foreach (Node i in overlapcheck)
            {
                i.ChangeWalkableColor(false);
            }
        }
        
    }




    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //길찾기 함수
    public void FindPath()
    {
        if (!isgameing)
        {
            isgameing = true;
            bool findpath = false;

            List<Node> OpenList = new List<Node>();

            //closedList는 내용의 순서가 상관이 없기 때문에 HashSet으로 정의
            //HastSet은 내용의 순서와 상관없이 중복여부만 체크, 중복일 경우 false로 들어가지 않는다.
            HashSet<Node> ClosedList = new HashSet<Node>();

            OpenList.Add(StartNode);

            //openList의 노드가 없을 때 까지 반복
            //openList가 비었다는 것은 모든 노드를 검색했다는 뜻
            while (OpenList.Count > 0)
            {

                //현재 노드는 OpenList[0] 즉, 시작 노드부터
                Node currentNode = OpenList[0];
                for (int i = 1; i < OpenList.Count; i++)
                {
                    //i가 1부터 시작하는 이유는 startNode가 이미 OpenList에 들어가있기 때문.
                    //openList에 있는 노드들의 거리 계산 후 가장 낮은 비용을 가진 node를 currentnode로 변경
                    if (currentNode.GetfCost < OpenList[i].GetfCost || currentNode.GetfCost == OpenList[i].GetfCost && currentNode.GethCost < OpenList[i].GethCost)
                    {

                        currentNode = OpenList[i];
                    }
                }

                //currentNode는 검색을 끝낸 Node이기 때문에 closedList에 추가
                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);

                //if (currentNode != StartNode)
                //    currentNode.OriginColor();

                //현재 노드의 이웃노드를 찾아서 OpenList에 추가
                foreach (Node neighbournode in GetNeighbours(currentNode))
                {

                    //이웃 노드가 closedlist에 있거나(이미 검색한 Node) 이동불가면 제외
                    if (!neighbournode.Getwalkable || ClosedList.Contains(neighbournode))
                    {

                        continue;
                    }


                    //이웃 노드의 cost계산
                    int newMovementCost = currentNode.GetgCost + GetDistanceCost(currentNode, neighbournode);
                    if (newMovementCost < neighbournode.GetgCost || !OpenList.Contains(neighbournode))
                    {

                        neighbournode.GetgCost = newMovementCost;
                        neighbournode.GethCost = GetDistanceCost(neighbournode, EndNode);
                        neighbournode.parent = currentNode;


                        if (!OpenList.Contains(neighbournode))
                        {

                            OpenList.Add(neighbournode);
                        }
                    }

                }

                if (currentNode == EndNode)
                {

                    findpath = true;
                    Debug.Log("길찾기 성공");
                    break;
                }


                // https://kiyongpro.github.io/algorithm/AStarPathFinding/

            }

            if (findpath)
            {
                TileCanChange = false;
                Vector3[] waypoint = WayPoint(StartNode, EndNode);
                EM.gameStartCourtain(waypoint, waypoint[0]);
                StartCoroutine("GameStartCheck");
            }

            else
            {
                StopCoroutine("ShowNotFoundRoute");
                StartCoroutine("ShowNotFoundRoute");
                NotFound.SetActive(true);
                Debug.Log("길찾기 실패");
                isgameing = false;
            }

        }
    }

    private IEnumerator ShowNotFoundRoute()
    {
        yield return new WaitForSeconds(1.5f);
        NotFound.SetActive(false);
    }

    IEnumerator GameStartCheck()
    {
        while (true)
        {
            isgameing = EM.GameOnGoing;
            if (!isgameing)
            {
                addtileprice = 0;
                break;
            }
            yield return null;
        }

        for (int i = 0; i < waypointnode.Count - 1; i++)
        {
            waypointnode[i].GetComponentInChildren<ShowRoute>().ReturnArrow();
        }
        waypointnode.Clear();
    }


    //길 찾기 완료후 해당 타일의 위치값 반환
    private Vector3[] WayPoint(Node Startnode, Node Endnode)
    {
        Node currentNode = Endnode;
        List<Vector3> waypoint = new List<Vector3>();

        Vector3 tilePos = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);

        //끝 노드를 추가
        waypoint.Add(tilePos);

        waypointnode.Add(currentNode);

        while (currentNode != Startnode)
        {
            Vector3 tilePos2 = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);
            currentNode = currentNode.parent;

            waypoint.Add(tilePos2);
            waypointnode.Add(currentNode);
        }

        Vector3 tilePos3 = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);
        waypoint.Add(tilePos3);

        waypointnode.Reverse();

        waypoint.Reverse();
        Vector3[] waypointary = waypoint.ToArray();

        for (int i = 0; i < waypointnode.Count - 1; i++)
        {
          
            waypointnode[i].OriginColor();
            if (waypointnode[i].gridX < waypointnode[i + 1].gridX)
            {//오른쪽
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(1);
            }
            else if (waypointnode[i].gridX > waypointnode[i + 1].gridX)
            {//왼쪽
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(2);
            }
            else if (waypointnode[i].gridY < waypointnode[i + 1].gridY)
            {//위쪽
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(3);
            }
            else if (waypointnode[i].gridY > waypointnode[i + 1].gridY)
            {//아래쪽
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(4);
            }
        }

        return waypointary;
    }

    //노드간의 거리계산
    int GetDistanceCost(Node A, Node B)
    {

        int disX = Mathf.Abs(A.gridX - B.gridX);
        int disY = Mathf.Abs(A.gridY - B.gridY);

        //if (disX > disY)
        //    return disY * 14 + (disX - disY) * 10;
        //return disX * 14 + (disY - disX) * 10;

        return disX + disY;

    }

    //node의 이웃계산
    //이 게임에는 대각선 이동은 없으므로 좌,우,위,아래에 대한 이웃만 사용
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //현재 노드를 기준으로 x,y인덱스값에 위,아래,왼쪽,오른쪽을 계산할 2차원 int배열 
        int[,] temp = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int i = 0; i < 4; i++)
        {
            int checkX = node.gridX + temp[i, 0]; //1,-1,0,0
            int checkY = node.gridY + temp[i, 1]; //0,0,1,-1

            if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY)
            {
                neighbours.Add(grid[checkY, checkX]);
            }

        }

        return neighbours;
    }



    //마우스 위치의 노드 반환
    public Node ReturnNode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.GetComponent<Node>();
        }
        return null;
    }


    //////////////////////////////////////////////////////////
    ///    //////////////////////////////////////////////////////////
    ///    
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

    public bool GetSetTileChange
    {
        set
        {
            TileCanChange = value;
        }
    }

    public bool GetSetAddTile
    {
        get
        {
            return AddTileActive;
        }
    }
}

