using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingPQ : MonoBehaviour 
{
    [SerializeField]
    private Grid GridMap;

    public Transform PlayerPos;
    public Transform TargetPos;

    private Vector3 _cacheStart, _cacheTarget;

    private void Update()
	{
        if (PlayerPos.position != _cacheStart || TargetPos.position != _cacheTarget)
        {
            FindPath(PlayerPos.position, TargetPos.position);

            _cacheStart = PlayerPos.position;
            _cacheTarget = TargetPos.position;
        }
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = GridMap.GetNodeFromPosition(startPos);
		Node targetNode = GridMap.GetNodeFromPosition(targetPos);

        PriorityQueue<Node> openSet = new PriorityQueue<Node> ();
		HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Enqueue (startNode);
        while (openSet.Count > 0) 
        {
            Node currentNode = openSet.Dequeue();

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            closedSet.Add(currentNode);

            foreach (Node n in GridMap.GetNeighbours(currentNode))
            {
                if (!n.IsWalkable || closedSet.Contains(n))
                {
                    continue;
                }

                int g = currentNode.GCost + GetDistance(currentNode, n);
                int h = GetDistance(n, targetNode);
                int f = g + h;

                // 오픈 셋에 이미 중복 노드가 있는 경우 값이 작은 쪽으로 변경한다.
                if (!openSet.Contains(n))
                {
                    n.GCost = g;
                    n.HCost = h;
                    n.Parent = currentNode;
                    openSet.Enqueue(n);
                }
                else
                {
                    if(n.FCost > f)
                    {
                        n.GCost = g;
                        n.Parent = currentNode;
                        openSet.Reposition(n);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.Parent;
		}

		path.Reverse ();
		GridMap.Path = path;
	}

	private int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
		int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

		if(dstX > dstY)
		{
			return 14*dstY + 10*(dstX - dstY);
		}

		return 14*dstX + 10*(dstY - dstX);
	}
}
