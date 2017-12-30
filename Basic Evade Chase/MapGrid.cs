using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour {

    Node[,] grid;
    public Node[,] Grid
    {
        get
        {
            return grid;
        }
    }

    [HideInInspector] public Node playerNode;
    [HideInInspector] public Node EnemyNode;

    public GameObject player;
    public GameObject enemy;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    public int GridSizeX
    {
        get
        {
            return gridSizeX;
        }
    }

    public int GridSizeY
    {
        get
        {
            return gridSizeY;
        }
    }

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; 

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint);
                grid[x, y].gridLoc.x = x;
                grid[x, y].gridLoc.y = y;
            }
        }

    }

    public int[,] Grid2Array()
    {
        int[,] array = new int[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y].walkable)
                {
                    array[x, y] = 1;
                }
                else
                {
                    array[x, y] = 0;
                }
                if (grid[x, y] == player.GetComponent<CharController>().currNode)
                {
                    array[x, y] = 2;
                }
                if (grid[x, y] == enemy.GetComponent<ChaseAI>().currNode)
                {
                    array[x, y] = 3;
                }
            }
        }
        return array;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        if (grid != null)
        {
            int gridX = Mathf.RoundToInt(Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x) * (gridSizeX - 1));
            int gridY = Mathf.RoundToInt(Mathf.Clamp01((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y) * (gridSizeY - 1));
            return grid[gridX, gridY];
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0.2f, gridWorldSize.y));
        if(grid != null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(n == player.GetComponent<CharController>().currNode)
                {
                    Gizmos.color = Color.green;
                }
                if(n == enemy.GetComponent<ChaseAI>().currNode)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, new Vector3(nodeDiameter - 0.01f, 0.1f, nodeDiameter - 0.01f));
            }
        }
    }

}
