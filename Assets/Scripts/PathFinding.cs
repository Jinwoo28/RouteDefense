using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
   



    //길찾기 시작
    //gCost = 시작 노드부터 해당 노드까지의 비용
    //hcost = 휴리스틱을 이용한 도착지까지의 비용
    //fCost = gCost + hCost;

    //보통 격자무늬에서 길찾기를 할 때 피타고라스 정의에 의해
    //직선의 이동 비용은 10, 대각선의 이동 비용은 14로 계산

    //openList = 탐색을 기다리는 도드들
    //closedList = 이미 탐색을 끝낸 노드들

    //currentNode == endNode이거나
    //OpenList가 빌때까지 반복
    //currentNode는 OpenList에 있는 노드중 fCost가 가장 작은 노드
    //currentNode가 나오면 OpenList에서 currentNode를 제거하고 closeList에 넣기

    //foreach문으로 current 주변 노드(neighbour Node)들을 모두 검사.
    //이 때 이동할 수 없는(walkable ==false)Node나 closedList에 포함되어 있는 Node는 제외

    //neighbour Node의 parentNode는 current Node

    //만약 neighbour Node가 openList에 들어있지 않다면 추가

    //startNode를 openList

}
