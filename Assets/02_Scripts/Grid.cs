using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour 
{
	public Transform PlayerTrans;

	public LayerMask UnwalkableMask;
	
	public Vector2 NumberOfGrids;

	public float NodeSize;

    public List<Node> Path;

    private Node[,] _grid;

	private float _nodeHalfSize;
	private int _gridSizeX, _gridSizeY;

	private void Awake()
	{
		_nodeHalfSize = NodeSize * 0.5f;
		_gridSizeX = Mathf.RoundToInt (NumberOfGrids.x / NodeSize);
		_gridSizeY = Mathf.RoundToInt (NumberOfGrids.y / NodeSize);

		CreateGrid ();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(NumberOfGrids.x, 1, NumberOfGrids.y));

        if (_grid != null)
        {

            Node playernode = GetNodeFromPosition(PlayerTrans.position);

            foreach (Node n in _grid)
            {
                Gizmos.color = (n.IsWalkable) ? Color.white : Color.red;

                if (playernode == n)
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    if (Path != null && Path.Contains(n))
                        Gizmos.color = Color.black;
                }

                Gizmos.DrawCube(n.Position, Vector3.one * (NodeSize - 0.1f));
            }
        }
    }

    private void CreateGrid()
	{
		_grid = new Node[_gridSizeX, _gridSizeY];
		Vector3 bottomLeft = transform.position - Vector3.right * NumberOfGrids.x / 2 - Vector3.forward * NumberOfGrids.y / 2;

		for (int x = 0; x < _gridSizeX; x++) 
		{
			for (int y = 0; y < _gridSizeY; y++) 
			{
				Vector3 nodePosition = bottomLeft + Vector3.right * (x * NodeSize + _nodeHalfSize) + Vector3.forward * (y * NodeSize + _nodeHalfSize);
				bool walkable = !(Physics.CheckSphere (nodePosition, _nodeHalfSize, UnwalkableMask));
				_grid [x, y] = new Node (walkable, nodePosition, x , y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node> ();
		for (int x = -1; x <= 1; ++x) 
		{
			for (int y = -1; y <= 1; ++y) 
			{
				if (x == 0 && y == 0) 
				{
					continue;
				}

				int checkX = node.GridX + x;
				int checkY = node.GridY + y;

				if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY) 
				{
					neighbours.Add(_grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}
		
	public Node GetNodeFromPosition(Vector3 position) 
	{
		float percentX = (position.x + NumberOfGrids.x / 2) / NumberOfGrids.x;
		float percentY = (position.z + NumberOfGrids.y / 2) / NumberOfGrids.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt ((_gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt ((_gridSizeY - 1) * percentY);
		return _grid [x, y];
	}
}
