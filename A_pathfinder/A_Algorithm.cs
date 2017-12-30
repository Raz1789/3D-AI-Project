using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Algorithm{

    public static List<NodeContainer> open;
    public static List<NodeContainer> closed;

    static bool[,] gridArr;
    static List<Vector2Int> output;
    public static List<Vector2Int> Output
    {
        get
        {
            return output;
        }
    }

    static int gridSizeX;
    static int gridSizeY;


    /******************** MAKING SINGLETON **************/
    static A_Algorithm instance;

    A_Algorithm()
    {
        open = new List<NodeContainer>();
        closed = new List<NodeContainer>();
        output = new List<Vector2Int>();
    }

    public static A_Algorithm Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new A_Algorithm();
            }

            return instance;
        }
    }

    /*******************************************************************/

    public List<Vector2Int> CalcPath(Vector2Int start, Vector2Int target)
    {
        // Init Grid Variables
        gridArr = MapGrid_A.Grid2Array();
        gridSizeX = MapGrid_A.GridSizeX;
        gridSizeY = MapGrid_A.gridSizeY;

        //Clearing for new check
        if (open != null) open.Clear();
        if (closed != null) closed.Clear();

        //Init Closed List
        NodeContainer temp = new NodeContainer(start,null,start,start);

        open.Add(temp);

        while(open.Count > 0)
        {
            open.Sort();
            closed.Add(open[0]);
            open.RemoveAt(0);
            int x = closed[closed.Count - 1].currNode.x;
            int y = closed[closed.Count - 1].currNode.y;

            for(int i = -1; i <= 1; i++)
            {
                if (x + i >= 0 && x + i < gridSizeX)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        if(y + j >= 0 && y + j < gridSizeY)
                        {
                            Vector2Int checkNode = new Vector2Int(x + i, y + j);
                            temp = new NodeContainer(checkNode,closed[closed.Count - 1], start, target);

                            if ( (x + i != target.x || y + j != target.y) && gridArr[x + i, y + j])
                            {
                                if (!closed.Exists(e => e.currNode == temp.currNode))
                                {
                                    int index = open.FindIndex(e => e.currNode == temp.currNode);
                                    if(index < 0)
                                    {
                                        open.Add(temp);
                                    } else if(open[index].FCost > temp.FCost)
                                    {
                                        open[index] = temp;
                                    }
                                }
                            }
                            else if(x + i == target.x && y + j == target.y)
                            {
                                MakePathList(temp);
                                return output;
                            }
                        }
                    }

                }
            }
        }

        return null;
    }
    
    static void MakePathList(NodeContainer final)
    {
        output.Clear();
        while(final != null)
        {
            output.Add(final.currNode);
            final = final.prevNode;
        }
    }

}
