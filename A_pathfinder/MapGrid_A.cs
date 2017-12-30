using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid_A{

    static Node[,] grid;

    public static Node[,] Grid
    {
        get
        {
            return grid;
        }
    }
   
    public static Vector3 origin;
    public static Vector2 gridWorldSize;
    public static float NodeRadius;
    public static LayerMask unwalkableMask;


    static float NodeDiameter;
    static int gridSizeX;
    public static int GridSizeX
    {
        get
        {
            return gridSizeX;
        }
    }

    public static int gridSizeY;
    static int GridSizeY
    {
        get
        {
            return gridSizeY;
        }
    }
    static MapGrid_A instance;

    private MapGrid_A()
    {
        gridWorldSize = new Vector2(6.0f, 4.5f);
        NodeRadius = 0.15f;
        NodeDiameter = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / NodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / NodeDiameter);
        //Debug.Log(gridSizeX + " " + gridSizeY);
        origin = new Vector3(0.0f, 0.261f, 0.0f);
        unwalkableMask = LayerMask.GetMask("Unwalkable");
        CreateGrid();
    }

    public static MapGrid_A Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new MapGrid_A();
            }
            return instance;
        }
    }

    void  CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = origin - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; 

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * NodeDiameter + NodeRadius) + Vector3.forward * (y * NodeDiameter + NodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius,unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint);
                grid[x, y].gridLoc.x = x;
                grid[x, y].gridLoc.y = y;
            }
        }

    }

    public static bool[,] Grid2Array()
    {
        bool[,] isWalkable = new bool[gridSizeX, gridSizeY];

        for(int i = 0; i < gridSizeX; i++)
        {
            for(int j = 0; j < gridSizeY; j++)
            {
                isWalkable[i, j] = grid[i, j].walkable;
            }
        }

        return isWalkable;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int gridX = Mathf.RoundToInt(Mathf.Clamp01((worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x) * (gridSizeX - 1));
        int gridY = Mathf.RoundToInt(Mathf.Clamp01((worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y)  * (gridSizeY - 1));
        return grid[gridX, gridY];
    }

}
