using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private Texture2D cursorimage = null;
    [SerializeField] private GameObject NotFound = null;
    
    private int gridX;
    private int gridY;

    //Node�� 2���� �迭�� ����� index�� �ο�
    private Node[,] grid;
    
    //��ã�⿡�� ���۰� �� ���
    private Node StartNode;
    private Node Start2Node;
    private Node EndNode;

    private DetectObject detector = new DetectObject();

    private HashSet<Node> overlapcheck = new HashSet<Node>();

    private bool isGameing = false;
    private bool isTileCanChange = false;

    private List<Node> waypointnode = new List<Node>();
    Vector3[] waypoints1;

    private List<Node> waypointnode2 = new List<Node>();
    Vector3[] waypoints2;

    private AlertSetting alter = new AlertSetting();

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

        if (isTileCanChange)
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

        if (isTileCanChange)
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
    //��ư���� �游��� Ȱ��ȭ, ��Ȱ��ȭ ��ų ��ư�Լ�
    public void OnClickWalkableChange()
    {
        bool can = !isTileCanChange;
        GameManager.buttonOff();

        if (!isGameing)
        {
            // if (!AddTileActive)
            isTileCanChange = can;
        }
    }

    public void MouseChage()
    {
        isTileCanChange = false;
    }

    public void RouteReset()
    {
        GameManager.buttonOff();
        if (!isGameing)
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
                if (!isGameing)
                {
                    isGameing = true;
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
                if (!isGameing)
                {
                    isGameing = true;

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
            if (!isGameing)
            {
                isGameing = true;
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
            if (!isGameing)
            {
                isGameing = true;
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
    //��ã�� �Լ�
    public bool FindPath(Node _Start)
    {
        GameManager.buttonOff();

        bool findpath = false;

            List<Node> OpenList = new List<Node>();

            //closedList�� ������ ������ ����� ���� ������ HashSet���� ����
            //HastSet�� ������ ������ ������� �ߺ����θ� üũ, �ߺ��� ��� false�� ���� �ʴ´�.
            HashSet<Node> ClosedList = new HashSet<Node>();

            OpenList.Add(_Start);

            //openList�� ��尡 ���� �� ���� �ݺ�
            //openList�� ����ٴ� ���� ��� ��带 �˻��ߴٴ� ��
            while (OpenList.Count > 0)
            {

                //���� ���� OpenList[0] ��, ���� ������
                Node currentNode = OpenList[0];
                for (int i = 1; i < OpenList.Count; i++)
                {
                    //i�� 1���� �����ϴ� ������ startNode�� �̹� OpenList�� ���ֱ� ����.
                    //openList�� �ִ� ������ �Ÿ� ��� �� ���� ���� ����� ���� node�� currentnode�� ����
                    if (currentNode.GetfCost < OpenList[i].GetfCost || currentNode.GetfCost == OpenList[i].GetfCost && currentNode.GethCost < OpenList[i].GethCost)
                    {
                        currentNode = OpenList[i];
                    }
                }

                //currentNode�� �˻��� ���� Node�̱� ������ closedList�� �߰�
                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);

                //���� ����� �̿���带 ã�Ƽ� OpenList�� �߰�
                foreach (Node neighbournode in GetNeighbours(currentNode))
                {

                    //�̿� ��尡 closedlist�� �ְų�(�̹� �˻��� Node) �̵��Ұ��� ����
                    if (!neighbournode.Getwalkable || ClosedList.Contains(neighbournode))
                    {
                        continue;
                    }

                    //�̿� ����� cost���
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
                isTileCanChange = false;
                alter.PlaySound(AlertKind.Click, this.gameObject);
                return true;
            }
            else
            {
                isGameing = false;
                return false;
            }
    }

    private IEnumerator ShowNotFoundRoute()
    {
        alter.PlaySound(AlertKind.Cant, this.gameObject);
        isGameing = false;
        yield return new WaitForSeconds(1.0f);
        NotFound.SetActive(false);
    }

    IEnumerator GameStartCheck(List<Node> _waypointnode)
    {
        while (true)
        {
            isGameing = EM.GameOnGoing;
            if (!isGameing)
            {
                alter.PlaySound(AlertKind.Cant, this.gameObject);
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
        isGameing = false;

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

    //�� ã�� �Ϸ��� �ش� Ÿ���� ��ġ�� ��ȯ

    private Vector3[] WayPoint2(Node Startnode, Node Endnode, List<Node> nodelist)
    {
        Node currentNode = Endnode;

        List<Vector3> waypoint = new List<Vector3>();

        Vector3 tilePos = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);

        //�� ��带 �߰�
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
            {//������
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(1);
            }
            else if (nodelist[i].gridX > nodelist[i + 1].gridX)
            {//����
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(2);
            }
            else if (nodelist[i].gridY < nodelist[i + 1].gridY)
            {//����
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(3);
            }
            else if (nodelist[i].gridY > nodelist[i + 1].gridY)
            {//�Ʒ���
                nodelist[i].GetComponentInChildren<ShowRoute>().ShowArrow(4);
            }
        }


        return waypointary;
    }

    //��尣�� �Ÿ����
    int GetDistanceCost(Node A, Node B)
    {
        int disX = Mathf.Abs(A.gridX - B.gridX);
        int disY = Mathf.Abs(A.gridY - B.gridY);

        //if (disX > disY)
        //    return disY * 14 + (disX - disY) * 10;
        //return disX * 14 + (disY - disX) * 10;

        return disX + disY;
    }

    //node�� �̿����
    //�� ���ӿ��� �밢�� �̵��� �����Ƿ� ��,��,��,�Ʒ��� ���� �̿��� ���
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //���� ��带 �������� x,y�ε������� ��,�Ʒ�,����,�������� ����� 2���� int�迭 
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

        //closedList�� ������ ������ ����� ���� ������ HashSet���� ����
        //HastSet�� ������ ������ ������� �ߺ����θ� üũ, �ߺ��� ��� false�� ���� �ʴ´�.
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(_Start);

        //openList�� ��尡 ���� �� ���� �ݺ�
        //openList�� ����ٴ� ���� ��� ��带 �˻��ߴٴ� ��
        while (OpenList.Count > 0)
        {

            //���� ���� OpenList[0] ��, ���� ������
            Node currentNode = OpenList[0];

            if (currentNode.GetSetPoint == 1)
            {
                CheckCount--;
            }

            for (int i = 1; i < OpenList.Count; i++)
            {
                //i�� 1���� �����ϴ� ������ startNode�� �̹� OpenList�� ���ֱ� ����.
                //openList�� �ִ� ������ �Ÿ� ��� �� ���� ���� ����� ���� node�� currentnode�� ����
                if (currentNode.GetfCost < OpenList[i].GetfCost || currentNode.GetfCost == OpenList[i].GetfCost && currentNode.GethCost < OpenList[i].GethCost)
                {
                    currentNode = OpenList[i];
                }
            }

            //currentNode�� �˻��� ���� Node�̱� ������ closedList�� �߰�
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            //if (currentNode != StartNode)
            //    currentNode.OriginColor();

            //���� ����� �̿���带 ã�Ƽ� OpenList�� �߰�
            foreach (Node neighbournode in GetNeighbours(currentNode))
            {

                //�̿� ��尡 closedlist�� �ְų�(�̹� �˻��� Node) �̵��Ұ��� ����
                if (!neighbournode.Getwalkable || ClosedList.Contains(neighbournode))
                {
                    continue;
                }


                //�̿� ����� cost���
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
            isTileCanChange = false;
            return true;
        }

        else
        {
            isGameing = false;
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

        //closedList�� ������ ������ ����� ���� ������ HashSet���� ����
        //HastSet�� ������ ������ ������� �ߺ����θ� üũ, �ߺ��� ��� false�� ���� �ʴ´�.
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(_Start);

        //openList�� ��尡 ���� �� ���� �ݺ�
        //openList�� ����ٴ� ���� ��� ��带 �˻��ߴٴ� ��
        while (OpenList.Count > 0)
        {

            //���� ���� OpenList[0] ��, ���� ������
            Node currentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                //i�� 1���� �����ϴ� ������ startNode�� �̹� OpenList�� ���ֱ� ����.
                //openList�� �ִ� ������ �Ÿ� ��� �� ���� ���� ����� ���� node�� currentnode�� ����
                if (currentNode.GetfCost < OpenList[i].GetfCost || currentNode.GetfCost == OpenList[i].GetfCost && currentNode.GethCost < OpenList[i].GethCost)
                {
                    currentNode = OpenList[i];
                }
            }

            //currentNode�� �˻��� ���� Node�̱� ������ closedList�� �߰�
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            //if (currentNode != StartNode)
            //    currentNode.OriginColor();

            //���� ����� �̿���带 ã�Ƽ� OpenList�� �߰�
            foreach (Node neighbournode in GetNeighbours(currentNode))
            {

                //�̿� ��尡 closedlist�� �ְų�(�̹� �˻��� Node) �̵��Ұ��� ����
                if (!neighbournode.Getwalkable || ClosedList.Contains(neighbournode))
                {
                    continue;
                }


                //�̿� ����� cost���
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

        if(waypoint == null)
        {
            return false;
        }

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
            isTileCanChange = false;
            return true;
        }

        else
        {
            isGameing = false;
            return false;
        }

    }

    private Node[] Find3(Node start, Node end)
    {
        Node currentNode = end;

        List<Node> waypoint = new List<Node>();

        Debug.Log(start.gridX + " : " + start.gridY);
        Debug.Log(end.gridX + " : " + end.gridY);

        //�� ��带 �߰�
        waypoint.Add(currentNode);

        while (currentNode != start)
        {
            Debug.Log(currentNode.parent);

            if(currentNode.parent == null)
            {
                return null;
            }

            currentNode = currentNode.parent;
            waypoint.Add(currentNode);
        }

        waypoint.Add(currentNode);

        waypoint.Reverse();

        Node[] waypointary = waypoint.ToArray();

        return waypointary;
    }

    public void PlayOnClickBtnSound()
    {
        alter.PlaySound(AlertKind.Click,this.gameObject);
    }

}
