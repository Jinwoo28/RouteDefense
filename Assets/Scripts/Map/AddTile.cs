using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddTile : MonoBehaviour
{
    [SerializeField] private PlayerState playerState = null;
    [SerializeField] private GameObject AddTilePrefab;

    private GameObject[] tiles = null;

    //전체 타일 사이즈크기
    //짝수 단위로 입력할 것
    private int gridX;
    private int gridY;
    private int addPrice = 10;
    //추가될 테트리스 타일 넘버
    private int tetrisNum = 0;

    private bool isTileActive = false;
    private bool isCanBuildTile = false;

    private AlertSetting alter = new AlertSetting();

    //Node를 2차원 배열로 만들어 index값 부여
    private Node[,] grid;

    //단차에 따른 타일 색
    private Material[] tilecolor = null;
    private List<Node> activeNode = null;
    public List<Node> GetActiveNode => activeNode;
    public int GetAddTileNum => tetrisNum;
    public int GetAddtilePrice => addPrice;

    public void Settings(int _gridX, int _gridY, Node[,] _grid, Material[] _tilecolor, List<Node> _activeNode)
    {
        gridX = _gridX;
        gridY = _gridY;
        grid = _grid;
        tilecolor = _tilecolor;
        activeNode = _activeNode;
    }

    private void Start()
    {
        tetrisNum = Random.Range(0, 7);
        GameManager.buttonOff+= CancelAddTile;
        EnemyManager.stageclear+= TileChange;
    }

    //테트리스 모양의 블럭위치 반환
    private Vector3[] SetTilePos(int tileshapenum, int _tileDir, int Xnum, int Ynum)
    {
        Vector3[] tileList = new Vector3[11];
        //블록의 타입, 블록의 회전횟수, 타일의 위치 저장

        Vector3[] returnTilePos = new Vector3[4];

        /*
        모든 타일생성에 필요한 index번호
        0이 마우스 기준
        X 1 2  3
        4 5 0  6
        X 7 8  9
        X X 10 X
        
        타일 모양 / 변형가능횟수
         O(0) I(1) S(2) Z(2) L(4) J(4) T(4)

        타일모양 O     I          S           Z           L           J           T
        □ □ □ □       □ □ ■ □    □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □
        □ ■ ■ □       □ □ ■ □    □ □ ■ ■     □ ■ ■ □     □ ■ □ □     □ □ □ ■     □ □ ■ □
        □ ■ ■ □       □ □ ■ □    □ ■ ■ □     □ □ ■ ■     □ ■ ■ ■     □ ■ ■ ■     □ ■ ■ ■
        □ □ □ □       □ □ ■ □    □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □     □ □ □ □
         */

        tileList[0] = new Vector3(Xnum, 0, Ynum);
        tileList[1] = new Vector3(Xnum - 1, 0, Ynum + 1);
        tileList[2] = new Vector3(Xnum, 0, Ynum + 1);
        tileList[3] = new Vector3(Xnum + 1, 0, Ynum + 1);
        tileList[4] = new Vector3(Xnum - 2, 0, Ynum);
        tileList[5] = new Vector3(Xnum - 1, 0, Ynum);
        tileList[6] = new Vector3(Xnum + 1, 0, Ynum);
        tileList[7] = new Vector3(Xnum - 1, 0, Ynum - 1);
        tileList[8] = new Vector3(Xnum, 0, Ynum - 1);
        tileList[9] = new Vector3(Xnum + 1, 0, Ynum - 1);
        tileList[10] = new Vector3(Xnum, 0, Ynum - 2);

        switch (tileshapenum)
        {
            case 0:
                returnTilePos[0] = tileList[0];
                returnTilePos[1] = tileList[1];
                returnTilePos[2] = tileList[2];
                returnTilePos[3] = tileList[5];
                break;
            case 1:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returnTilePos[0] = tileList[2];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[8];
                    returnTilePos[3] = tileList[10];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returnTilePos[0] = tileList[4];
                    returnTilePos[1] = tileList[5];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[6];
                }
                break;
            case 2:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returnTilePos[0] = tileList[6];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[8];
                    returnTilePos[3] = tileList[7];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returnTilePos[0] = tileList[2];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[6];
                    returnTilePos[3] = tileList[9];
                }
                break;
            case 3:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returnTilePos[0] = tileList[5];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[8];
                    returnTilePos[3] = tileList[9];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returnTilePos[0] = tileList[3];
                    returnTilePos[1] = tileList[6];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[8];
                }
                break;
            case 4:
                if (_tileDir == 0)
                {
                    returnTilePos[0] = tileList[6];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[5];
                    returnTilePos[3] = tileList[7];
                }
                else if (_tileDir == 1)
                {
                    returnTilePos[0] = tileList[2];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[8];
                    returnTilePos[3] = tileList[9];
                }
                else if (_tileDir == 2)
                {
                    returnTilePos[0] = tileList[3];
                    returnTilePos[1] = tileList[6];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[5];
                }
                else if (_tileDir == 3)
                {
                    returnTilePos[0] = tileList[1];
                    returnTilePos[1] = tileList[2];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[8];
                }
                break;

            case 5:
                if (_tileDir == 0)
                {
                    returnTilePos[0] = tileList[5];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[6];
                    returnTilePos[3] = tileList[9];
                }
                else if (_tileDir == 1)
                {
                    returnTilePos[0] = tileList[3];
                    returnTilePos[1] = tileList[2];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[8];
                }
                else if (_tileDir == 2)
                {
                    returnTilePos[0] = tileList[1];
                    returnTilePos[1] = tileList[5];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[6];
                }
                else if (_tileDir == 3)
                {
                    returnTilePos[0] = tileList[2];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[8];
                    returnTilePos[3] = tileList[7];
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
                    returnTilePos[0] = tileList[5];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[6];
                    returnTilePos[3] = tileList[8];
                }
                else if (_tileDir == 1)
                {
                    returnTilePos[0] = tileList[6];
                    returnTilePos[1] = tileList[2];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[8];
                }
                else if (_tileDir == 2)
                {
                    returnTilePos[0] = tileList[2];
                    returnTilePos[1] = tileList[5];
                    returnTilePos[2] = tileList[0];
                    returnTilePos[3] = tileList[6];
                }
                else if (_tileDir == 3)
                {
                    returnTilePos[0] = tileList[2];
                    returnTilePos[1] = tileList[0];
                    returnTilePos[2] = tileList[8];
                    returnTilePos[3] = tileList[5];
                }
                break;
        }
        return returnTilePos;
    }

    public void OnClickAddBtn()
    {
        GameManager.buttonOff();
        StartCoroutine("CoTileAdd");
    }

    IEnumerator CoTileAdd()
    {
        if (playerState.GetSetPlayerCoin >= addPrice)
        {
            alter.PlaySound(AlertKind.Click, this.gameObject);
            //추가타일 가격만큼 플레이어 코인 감소
            playerState.GetSetPlayerCoin = addPrice;

            //타일의 방향 변수
            int tiledir = 0;

            //AddTile = true;
            Vector3 mousepos = Vector3.zero;
            Vector3 tilepos = Vector3.zero;

            /*GameObject[]*/
            tiles = new GameObject[4];

            //타일 4개 생성
            for (int i = 0; i < 4; i++)
            {
                tiles[i] = Instantiate(AddTilePrefab, mousepos, Quaternion.identity);
            }
            isTileActive = true;

            while (isTileActive)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //마우스 위치에 타일 생성

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    mousepos = hit.point;
                }

                if (IsInBoundaryTetris(tiledir, mousepos))
                {
                    //4개 타일의 위치
                    for (int i = 0; i < 4; i++)
                    {
                        int X = (int)SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].x;
                        int Y = (int)SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].z;

                        tiles[i].transform.position = SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i];

                        Vector3 intmousepos = Vector3.zero;
                        intmousepos = grid[Y, X].GetComponent<Node>().transform.localScale;

                        tiles[i].transform.localScale = intmousepos;


                        //4개 타일의 크기와 색
                        if (tiles[0].GetComponent<Preview>().CanBuildable && tiles[1].GetComponent<Preview>().CanBuildable &&
                            tiles[2].GetComponent<Preview>().CanBuildable && tiles[3].GetComponent<Preview>().CanBuildable)
                        {
                            isCanBuildTile = true;
                            tiles[i].GetComponentInChildren<MeshRenderer>().material.color = tilecolor[((int)(intmousepos.y * 2))-2].color;
                        }
                        else
                        {
                            tiles[i].GetComponentInChildren<MeshRenderer>().material.color = tilecolor[8].color;
                            isCanBuildTile = false;
                        }
                    }
                }

                if (isTileActive)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        tiledir++;
                        if (tiledir >= 4) tiledir = 0;
                    }
                }

                if (isCanBuildTile)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            //AddTile = false;
                            isTileActive = false;

                            for (int j = 0; j < 4; j++)
                            {
                                int X = (int)SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].x;
                                int Y = (int)SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].z;
                                Destroy(tiles[j]);
                                grid[Y, X].GetComponent<Node>().SetActiveTile(true);
                                grid[Y, X].GetSetActive = true;
                                isTileActive = false;
                                activeNode.Add(grid[Y, X]);
                            }
                            addPrice += 10;
                            tetrisNum = Random.Range(0, 7);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelAddTile();
                }
                yield return null;
            }
        }
        else
        {
            alter.PlaySound(AlertKind.bubu, this.gameObject);
            playerState.ShowNotEnoughMoneyCor();
        }
    }

    public void CancelAddTile()
    {
        if (isTileActive)
        {
            isTileActive = false;
            for (int i = 0; i < 4; i++)
            {
                Destroy(tiles[i]);
            }
            playerState.GetSetPlayerCoin = -addPrice;
        }
    }

    //테트리스 범위 제한
    private bool IsInBoundaryTetris(int tiledir, Vector3 mousepos)
    {
        for(int i = 0; i < 4; i++)
        {
            if(!(SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].x >= 0 &&
                SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].x < gridX &&
                SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].z >= 0 &&
                SetTilePos(tetrisNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].z < gridY))
            {
                return false;
            }
        }

        return true;

    }

    public void TileChange()
    {
        tetrisNum = Random.Range(0, 7);
    }

}
