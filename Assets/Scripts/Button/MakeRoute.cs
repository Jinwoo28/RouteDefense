using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRoute : MonoBehaviour
{
    private MapMake mapmake = null;
    private Node[,] grid = null;
    private bool TileChange = false;
    private bool AddTileActive = false;

    private List<Node> savenode = null;
    private HashSet<Node> overlapcheck = null;

    void Start()
    {
        mapmake = this.GetComponent<MapMake>();
        grid = mapmake.GetGrid;
        savenode = new List<Node>();
        overlapcheck = new HashSet<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (TileChange)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Node node = ReturnNode();

                if (node != null)
                {
                    StartCoroutine("NodeWalkableChange", node);
                }
            }
        }
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
            //while문 안에서 이러한 코루틴 없이 실행이 되면
            //무한 루프 때문에 유니티가 멈춰버림
            yield return null;
        }
    }

    //버튼으로 길찾기를 활성화, 비활성화 시킬 버튼함수
    public void WalkableChangeButton()
    {
        if (!AddTileActive)
            TileChange = !TileChange;
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

    public void RouteReset()
    {
        Debug.Log("1111");

         foreach(Node i in overlapcheck)
        {
            i.ChangeWalkableColor(false);
        }
    }

    public void ReturnColor()
    {
        foreach (Node i in overlapcheck)
        {
            i.ChangeWalkableColor(true);
        }
    }
}
