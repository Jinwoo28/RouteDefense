using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTile : MonoBehaviour
{
    private MapMake mapmake = null;

    //AddTile프리펩
    [SerializeField] private GameObject AddTilePrefab;
    private bool AddTile = false;
    private bool AddTileActive = false;
    private bool canaddtile = false;

    //추가될 테트리스 타일 넘버
    private int AddtileNum = 0;

    //단차에 따른 타일 색
    private Material[] Tilecolor = null;

    private Node[,] grid = null;

    private int gridX = 0;
    private int gridY = 0;


    private void Awake()
    {
        mapmake = this.GetComponent<MapMake>();
    }

    private void Start()
    {
        gridX = mapmake.GetgridX;
        gridY = mapmake.GetgridY;
        Tilecolor = mapmake.GetMaterials;
        grid = mapmake.GetGrid;
    }

    public void StartMapTileAdd()
    {
        StartCoroutine("MapTileAdd");
    }

    private IEnumerator MapTileAdd()
    {
        AddTile = true;
        Vector3 mousepos = Vector3.zero;
        Vector3 tilepos = Vector3.zero;

        GameObject[] AddtileX = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            AddtileX[i] = Instantiate(AddTilePrefab, mousepos, Quaternion.identity);
            AddTileActive = true;
        }

        int tiledir = 0;
        while (AddTile)
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

                for (int i = 0; i < 4; i++)
                {
                    int X = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].x;
                    int Y = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].z;
                    AddtileX[i].transform.position = AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i];
                    Vector3 intmousepos = Vector3.zero;
                    intmousepos = grid[Y, X].GetComponent<Node>().transform.localScale;

                    AddtileX[i].transform.localScale = intmousepos;



                    if (AddtileX[0].GetComponent<Preview>().CanBuildable && AddtileX[1].GetComponent<Preview>().CanBuildable && AddtileX[2].GetComponent<Preview>().CanBuildable && AddtileX[3].GetComponent<Preview>().CanBuildable)
                    {
                        canaddtile = true;
                        switch ((int)intmousepos.y)
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
                        }
                    }
                    else
                    {
                        AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[14].color;
                        canaddtile = false;
                    }


                }
            }

            if (AddTileActive)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    tiledir++;
                    if (tiledir >= 4) tiledir = 0;
                }
            }

            if (canaddtile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    AddTile = false;
                    AddTileActive = false;

                    for (int j = 0; j < 4; j++)
                    {
                        int X = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].x;
                        int Y = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].z;
                        Destroy(AddtileX[j]);
                        grid[Y, X].GetComponent<Node>().SetActiveTile(true);
                    }
                    AddtileNum = Random.Range(0, 7);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AddTile = false;
                AddTileActive = false;
                for (int i = 0; i < 4; i++)
                {
                    Destroy(AddtileX[i]);
                }
            }
            yield return null;
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
}
