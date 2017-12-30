using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : IComparable<NodeContainer>
{
    public Vector2Int currNode;
    public NodeContainer prevNode;
    float gCost;
    float hCost;
    public float FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public NodeContainer(Vector2Int currNode, NodeContainer prevNode, Vector2Int start, Vector2Int target)
    {
        this.currNode = currNode;
        this.prevNode = prevNode;
        CalcCost(start, target);
    }

    public void CalcCost(Vector2Int start, Vector2Int target)
    {
        if (currNode != Vector2Int.zero)
        {
            gCost = Vector2.SqrMagnitude(currNode - start) * 10;
            hCost = Vector2.SqrMagnitude(currNode - target) * 10;
        }
        else
        {
            Debug.LogError("current Node is not set in the Node Container");
        }
    }

    public int CompareTo(NodeContainer y)
    {
        if (this.FCost < y.FCost)
        {
            return -1;
        }
        else if (this.FCost > y.FCost)
        {
            return 1;
        }
        else if (this.FCost == y.FCost && this.hCost < y.hCost)
        {
            return -1;
        }
        else if (this.FCost == y.FCost && this.hCost > y.hCost)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

}
