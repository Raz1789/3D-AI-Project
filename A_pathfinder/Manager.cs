using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    MapGrid_A mapGrid;
    A_Algorithm algo;
    List<GameObject> nodeCubes;

	// Use this for initialization
	void Awake () {
        mapGrid = MapGrid_A.Instance;
        algo = A_Algorithm.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(MapGrid_A.origin, new Vector3(MapGrid_A.gridWorldSize.x, 0.2f, MapGrid_A.gridWorldSize.y));


        if (MapGrid_A.Grid != null)
        {
            foreach (Node n in MapGrid_A.Grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                if (A_Algorithm.Output.Count > 0)
                {
                    foreach (NodeContainer nC in A_Algorithm.open)
                    {
                        if (n == MapGrid_A.Grid[nC.currNode.x, nC.currNode.y])
                            Gizmos.color = Color.green;
                    }
                    foreach (NodeContainer nC in A_Algorithm.closed)
                    {
                        if (n == MapGrid_A.Grid[nC.currNode.x, nC.currNode.y])
                            Gizmos.color = Color.gray;
                    }
                    foreach (Vector2Int v in A_Algorithm.Output)
                    {
                        if (n == MapGrid_A.Grid[v.x, v.y])
                            Gizmos.color = Color.blue;
                    }

                }
                Gizmos.DrawCube(n.worldPosition, new Vector3(MapGrid_A.NodeRadius * 2 - 0.01f, 0.1f, MapGrid_A.NodeRadius * 2 - 0.01f));
            }
        }
    }
}
