using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart_Stop : MonoBehaviour
{
    private MapMake mapmake = null;
    private Node[,] grid = null;
    private int gridX=0;
    private int gridY=0;

    //길찾기에서 시작과 끝 노드
    private Node StartNode;
    private Node EndNode;

    private List<Node> waypointnode = null;

    [SerializeField] private EnemyManager EM = null;

    private bool gameisgoing = false;
    private void Start()
    {
        waypointnode = new List<Node>();

        mapmake = this.GetComponent<MapMake>();
        gridX = mapmake.GetgridX;
        gridY = mapmake.GetgridY;
        grid = mapmake.GetGrid;
        StartNode = mapmake.GetStartNode;
        EndNode = mapmake.GetEndNode;
    }

    private void Update()
    {
        //스폰이 끝나고 필드에 적이 없을 때만 게임이 실행
        gameisgoing = EM.GameOnGoing;
    }

    IEnumerator GameStartCheck()
    {
        while (true)
        {
            gameisgoing = EM.GameOnGoing;
            if (!gameisgoing) break;
            yield return null;
        }

        

        for (int i = 1; i < waypointnode.Count-1; i++)
        {
            waypointnode[i].GetComponentInChildren<ShowRoute>().ReturnArrow();
        }
        waypointnode.Clear();
    }

    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //길찾기 함수
    public void FindPath()
    {
        if (!gameisgoing)
        {
            gameisgoing = true;
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

                Vector3[] ddd = WayPoint(StartNode, EndNode);

                EM.gameStartCourtain(ddd, ddd[0]);
                StartCoroutine("GameStartCheck");
            }

            else
            {
                Debug.Log("길찾기 실패");
                gameisgoing = false;
            }

        }
    }

    //길 찾기 완료후 해당 타일의 위치값 반환
    private Vector3[] WayPoint(Node Startnode, Node Endnode)
    {
        Node currentNode = Endnode;
        List<Vector3> waypoint = new List<Vector3>();

        Vector3 tilePos = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth/2, currentNode.ThisPos.z);

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

        for(int i = 1; i < waypointnode.Count-1; i++)
        {
            Debug.Log($"gridX : {waypointnode[i].gridX}, gridY : {waypointnode[i].gridY}, 갯수 : {waypointnode.Count}");

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







}
