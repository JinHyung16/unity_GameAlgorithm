using System;
using UnityEngine;

[Serializable]
public class Node : IComparable<Node>
{
    public Vector3 Position { get; private set; }
    public int GridX { get;private set; }
    public int GridY { get;private set; }

    public bool IsWalkable { get; private set; }

    public int GCost { get; set; }
    public int HCost { get; set; }

    public Node Parent { get; set; }

    public Node(bool walkable, Vector3 position, int gridX, int gridY)
    {
        IsWalkable = walkable;
        Position = position;
        GridX = gridX;
        GridY = gridY;
    }

    public int CompareTo(Node otherNode)
    {
        if (FCost < otherNode.FCost) return -1;
        else if (FCost > otherNode.FCost) return 1;
        else return 0;
    }

    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }
}
