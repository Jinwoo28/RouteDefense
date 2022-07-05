using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    private int gridX;
    private int gridY;

    //Node를 2차원 배열로 만들어 index값 부여
    private Node[,] grid;

    [SerializeField] private Texture2D cursorimage = null;

    private DetectObject detector = new DetectObject();

    private HashSet<Node> overlapcheck = new HashSet<Node>();

    private bool isgameing = false;

    private bool TileCanChange = false;

    private List<Node> waypointnode = new List<Node>();
    Vector3[] waypoints1;

    private List<Node> waypointnode2 = new List<Node>();
    Vector3[] waypoints2;

    //길찾기에서 시작과 끝 노드
    private Node StartNode;
    private Node Start2Node;
    private Node EndNode;

    [SerializeField] private GameObject NotFound = null;

    public EnemyManager EM = null;

    private void Start()
    {
        EnemyManager.stageclear += StageClear;
        GameManager.buttonOff += MouseChage;
    }

    private void OnDestroy()
    {
        EnemyManager.stageclear -= StageClear;
        GameManager.buttonOff -= MouseChage;
    }

    public void Settings(int _gridX, int _gridY,Node[,] _grid, Node _start,Node _start2, Node _end)
    {
        gridX = _gridX;
        gridY = _gridY;
        grid = _grid;
        StartNode = _start;
        EndNode = _end;
        Start2Node = _start2;
    }

    private void Update()
    {
        if (TileCanChange)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Node node = detector.ReturnNode();

                if (node != null)
                {
                    StartCoroutine(NodeWalkableChange(node));
                }
            }
        }

        if (TileCanChange)
        {
            Cursor.SetCursor(cursorimage, Vector2.zero, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    IEnumerator NodeWalkableChange(Node _changenode)
    {
        bool walkable = !_changenode.Getwalkable;

        while (Input.GetMouseButton(0))
        {
            Node changenode = detector.ReturnNode();
            Debug.Log(changenode);
            if (changenode != null)
            {
                changenode.ChangeWalkableColor(walkable);

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
        bool can = !TileCanChange;

        GameManager.buttonOff();
        if (!isgameing)
        {
           // if (!AddTileActive)
           TileCanChange = can;
        }
    }

    public void MouseChage()
    {
        TileCanChange = false;
    }

    public void RouteReset()
    {
        GameManager.buttonOff();
        if (!isgameing)
        {
            foreach (Node i in overlapcheck)
            {
                i.ChangeWalkableColor(false);
            }
        }

    }

    bool success1 = false;
    bool success2 = false;

    public void GameStartBtn()
    {
        if (GameManager.GetSetStageType == StageType.Nomal)
        {
            if (GameManager.SetGameLevel == 3)
            {
                if (!isgameing)
                {
                    isgameing = true;
                    if (FindPath(Start2Node) && FindPath(StartNode))
                    {

                        FindPath(StartNode);
                        Vector3[] waypoint = WayPoint2(StartNode, EndNode, waypointnode);
                        EM.gameStartCourtain(waypoint, waypoint[0], 1);

                        FindPath(Start2Node);
                        Vector3[] waypoint2 = WayPoint2(Start2Node, EndNode, waypointnode2);
                        EM.gameStartCourtain(waypoint2, waypoint2[0], 2);

                    }
                    else
                    {
                        StopCoroutine("ShowNotFoundRoute");
                        StartCoroutine("ShowNotFoundRoute");
                        NotFound.SetActive(true);
                    }
                }
            }
            else
            {
                if (!isgameing)
                {
                    isgameing = true;
                    if (FindPath(StartNode))
                    {
                        Vector3[] waypoint = WayPoint2(StartNode, EndNode, waypointnode);
                        EM.gameStartCourtain(waypoint, waypoint[0], 1);
                    }
                    else
                    {
                        StopCoroutine("ShowNotFoundRoute");
                        StartCoroutine("ShowNotFoundRoute");
                        NotFound.SetActive(true);
                    }
                }
            }
        }
        else if(GameManager.GetSetStageType == StageType.UnOrderCheckPoint)
        {
            Debug.Log("UnOrder");
            if (!isgameing)
            {
                isgameing = true;
                if (FindPath2(StartNode))
                {
                    Vector3[] waypoint = WayPoint2(StartNode, EndNode, waypointnode);
                    EM.gameStartCourtain(waypoint, waypoint[0], 1);
                }
                else
                {
                    StopCoroutine("ShowNotFoundRoute");
                    StartCoroutine("ShowNotFoundRoute");
                    NotFound.SetActive(true);
                }
            }
        }
        else
        {
            Debug.Log("Order");
            if (!isgameing)
            {
                isgameing = true;
                if (FindPath3(StartNode))
                {
                    Vector3[] waypoint = WayPoint2(StartNode, EndNode, waypointnode);
                    EM.gameStartCourtain(waypoint, waypoint[0], 1);
                }
                else
                {
                    StopCoroutine("ShowNotFoundRoute");
                    StartCoroutine("ShowNotFoundRoute");
                    NotFound.SetActive(true);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //길찾기 함수
    public bool FindPath(Node _Start)
    {
        GameManager.buttonOff();

        bool findpath = false;

            List<Node> OpenList = new List<Node>();

            //closedList는 내용의 순서가 상관이 없기 때문에 HashSet으로 정의
            //HastSet은 내용의 순서와 상관없이 중복여부만 체크, 중복일 경우 false로 들어가지 않는다.
            HashSet<Node> ClosedList = new HashSet<Node>();

            OpenList.Add(_Start);

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
                    break;
                }


                // https://kiyongpro.github.io/algorithm/AStarPathFinding/

            }

            if (findpath)
            {
                TileCanChange = false;
            //Vector3[] waypoint = WayPoint(StartNode, EndNode);
            //EM.gameStartCourtain(waypoint, waypoint[0]);
            //waypoint = WayPoint(_start, EndNode, _waypoint);
            //EM.gameStartCourtain(waypoint, waypoint[0]);


            return true;
            }

            else
            {
                //StopCoroutine("ShowNotFoundRoute");
                //StartCoroutine("ShowNotFoundRoute");
                //NotFound.SetActive(true);
                Debug.Log("길찾기 실패");
                isgameing = false;
            return false;
            }
        
    }

    public bool CheckFindPath(Node _Start,Node _End)
    {
        GameManager.buttonOff();

        bool findpath = false;

        List<Node> OpenList = new List<Node>();

        //closedList는 내용의 순서가 상관이 없기 때문에 HashSet으로 정의
        //HastSet은 내용의 순서와 상관없이 중복여부만 체크, 중복일 경우 false로 들어가지 않는다.
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(_Start);

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
                    neighbournode.GethCost = GetDistanceCost(neighbournode, _End);
                    neighbournode.parent = currentNode;


                    if (!OpenList.Contains(neighbournode))
                    {

                        OpenList.Add(neighbournode);
                    }
                }

            }

            if (currentNode == _End)
            {

                findpath = true;
                break;
            }


            // https://kiyongpro.github.io/algorithm/AStarPathFinding/

        }

        if (findpath)
        {
            TileCanChange = false;
            //Vector3[] waypoint = WayPoint(StartNode, EndNode);
            //EM.gameStartCourtain(waypoint, waypoint[0]);
            //waypoint = WayPoint(_start, EndNode, _waypoint);
            //EM.gameStartCourtain(waypoint, waypoint[0]);


            return true;
        }

        else
        {
            //StopCoroutine("ShowNotFoundRoute");
            //StartCoroutine("ShowNotFoundRoute");
            //NotFound.SetActive(true);
            Debug.Log("길찾기 실패");
            isgameing = false;
            return false;
        }

    }

    private IEnumerator ShowNotFoundRoute()
    {
        yield return new WaitForSeconds(1.0f);
        NotFound.SetActive(false);
    }

    IEnumerator GameStartCheck(List<Node> _waypointnode)
    {
        while (true)
        {
            isgameing = EM.GameOnGoing;
            if (!isgameing)
            {
                break;
            }
            yield return null;
        }

        for (int i = 0; i < _waypointnode.Count - 1; i++)
        {
            _waypointnode[i].GetComponentInChildren<ShowRoute>().ReturnArrow();
        }
        _waypointnode.Clear();
    }

    private void StageClear()
    {
        isgameing = false;

        for (int i = 0; i < waypointnode.Count - 1; i++)
        {
            waypointnode[i].GetComponentInChildren<ShowRoute>().ReturnArrow();
        }
        waypointnode.Clear();

        for (int i = 0; i < waypointnode2.Count - 1; i++)
        {
            waypointnode2[i].GetComponentInChildren<ShowRoute>().ReturnArrow();
        }
        waypointnode2.Clear();

    }

    //길 찾기 완료후 해당 타일의 위치값 반환

    private Vector3[] WayPoint2(Node Startnode, Node Endnode, List<Node> nodelist)
    {
        Node currentNode = Endnode;

        List<Vector3> waypoint = new List<Vector3>();

        Vector3 tilePos = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);

        //끝 노드를 추가
        waypoint.Add(tilePos);

        nodelist.Add(currentNode);

        while (currentNode != Startnode)
        {
            Vector3 tilePos2 = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);
            currentNode = currentNode.parent;

            waypoint.Add(tilePos2);
            nodelist.Add(currentNode);
        }

        Vector3 tilePos3 = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);
        waypoint.Add(tilePos3);

        nodelist.Reverse();

        waypoint.Reverse();
        Vector3[] waypointary = waypoint.ToArray();


        for (int i = 0; i < nodelist.Count - 1; i++)
        {
            nodelist[i].OriginColor();
            if (nodelist[i].gridX < nodelist[i + 1].gridX)
            {//오른쪽
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(1);
            }
            else if (nodelist[i].gridX > nodelist[i + 1].gridX)
            {//왼쪽
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(2);
            }
            else if (nodelist[i].gridY < nodelist[i + 1].gridY)
            {//위쪽
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(3);
            }
            else if (nodelist[i].gridY > nodelist[i + 1].gridY)
            {//아래쪽
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(4);
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

    //Unorder
    public bool FindPath2(Node _Start)
    {
        GameManager.buttonOff();

        int CheckCount = GameManager.SetGameLevel+1;

        bool findpath = false;

        List<Node> OpenList = new List<Node>();

        //closedList는 내용의 순서가 상관이 없기 때문에 HashSet으로 정의
        //HastSet은 내용의 순서와 상관없이 중복여부만 체크, 중복일 경우 false로 들어가지 않는다.
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(_Start);

        //openList의 노드가 없을 때 까지 반복
        //openList가 비었다는 것은 모든 노드를 검색했다는 뜻
        while (OpenList.Count > 0)
        {

            //현재 노드는 OpenList[0] 즉, 시작 노드부터
            Node currentNode = OpenList[0];

            if (currentNode.GetSetPoint == 1)
            {
                CheckCount--;
            }

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
                break;
            }


            // https://kiyongpro.github.io/algorithm/AStarPathFinding/

        }

        if (findpath && CheckCount ==0)
        {
            TileCanChange = false;
            //Vector3[] waypoint = WayPoint(StartNode, EndNode);
            //EM.gameStartCourtain(waypoint, waypoint[0]);
            //waypoint = WayPoint(_start, EndNode, _waypoint);
            //EM.gameStartCourtain(waypoint, waypoint[0]);


            return true;
        }

        else
        {
            //StopCoroutine("ShowNotFoundRoute");
            //StartCoroutine("ShowNotFoundRoute");
            //NotFound.SetActive(true);
            Debug.Log("길찾기 실패");
            isgameing = false;
            return false;
        }

    }

    //Order
    public bool FindPath3(Node _Start)
    {
        GameManager.buttonOff();

        bool[] passed = new bool[GameManager.SetGameLevel+1];
        bool succesed = false;

        bool findpath = false;

        List<Node> OpenList = new List<Node>();

        //closedList는 내용의 순서가 상관이 없기 때문에 HashSet으로 정의
        //HastSet은 내용의 순서와 상관없이 중복여부만 체크, 중복일 경우 false로 들어가지 않는다.
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(_Start);

        //openList의 노드가 없을 때 까지 반복
        //openList가 비었다는 것은 모든 노드를 검색했다는 뜻
        while (OpenList.Count > 0)
        {

            //현재 노드는 OpenList[0] 즉, 시작 노드부터
            Node currentNode = OpenList[0];

            //if(currentNode.GetSetPoint == 1)
            //{
            //    passed[0] = true;
            //}
            //else
            //{
            //    if(currentNode.GetSetPoint != 0&&passed[currentNode.GetSetPoint-1] == true)
            //    {
            //        passed[currentNode.GetSetPoint] = true;
            //    }
            //}

            /// 수정중
            //if(currentNode.GetSetPoint == )

            //if (currentNode.GetSetPoint == 1)
            //{
            //    CheckCount--;
            //}

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
                break;
            }


            // https://kiyongpro.github.io/algorithm/AStarPathFinding/

        }



        Node[] waypoint = Find3(StartNode, EndNode);

        for(int i = 0; i < waypoint.Length; i++)
        {

            if (waypoint[i].GetSetPoint == 1)
            {
                passed[0] = true;
            }
            else
            {
                if (waypoint[i].GetSetPoint != 0 && passed[waypoint[i].GetSetPoint - 2] == true)
                {
                    passed[waypoint[i].GetSetPoint-1] = true;
                }
            }
        }

        for(int i = 0; i < passed.Length; i++)
        {
            if (!passed[i])
            {
                succesed = false;
                break;
            }
            succesed = true;
        }
        


        if (findpath && succesed)
        {
            TileCanChange = false;
            //Vector3[] waypoint = WayPoint(StartNode, EndNode);
            //EM.gameStartCourtain(waypoint, waypoint[0]);
            //waypoint = WayPoint(_start, EndNode, _waypoint);
            //EM.gameStartCourtain(waypoint, waypoint[0]);
            return true;
        }

        else
        {
            //StopCoroutine("ShowNotFoundRoute");
            //StartCoroutine("ShowNotFoundRoute");
            //NotFound.SetActive(true);
            Debug.Log("길찾기 실패");
            isgameing = false;
            return false;
        }

    }

    private Node[] Find3(Node start, Node end)
    {
        Node currentNode = end;

        List<Node> waypoint = new List<Node>();

        //끝 노드를 추가
        waypoint.Add(currentNode);

        while (currentNode != start)
        {
            currentNode = currentNode.parent;
            waypoint.Add(currentNode);
        }

        waypoint.Add(currentNode);

        waypoint.Reverse();

        Node[] waypointary = waypoint.ToArray();

        return waypointary;
    }

}
