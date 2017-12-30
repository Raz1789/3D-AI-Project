using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_A : MonoBehaviour {
    
    public GameObject target;
    Node targetPrevNode;
    bool targetChanged;
    public GameObject gameManager;
    public float enemyVel = 10f;

    public Node currNode;
    List<Vector2Int> path;
    Node setNode;
    int pathPointer;
    Vector3 destination;
    Vector3 nextDestination;

    // CharController targetController;
    Animator anim;


    // Use this for initialization
    void Start () {
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("No Animator attached to this GameObject");
        }
        targetChanged = false;
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(MapGrid_A.Instance != null)
            currNode = MapGrid_A.Instance.NodeFromWorldPoint(transform.position) ?? currNode;
        if (currNode != null && target.GetComponent<CharController_A>().currNode != null)
        {
            if (A_Algorithm.Instance != null && (path == null || targetChanged))
            {
                path = A_Algorithm.Instance.CalcPath(currNode.gridLoc, target.GetComponent<CharController_A>().currNode.gridLoc);
                if (path == null)
                {
                    Debug.LogError("No Path found!!!");
                }
                pathPointer = path.Count - 1;
                setNode = MapGrid_A.Grid[path[pathPointer].x, path[pathPointer].y];
            }
            //Debug.Log(currNode.gridLoc + " " + setNode.gridLoc);
            if (currNode == setNode)
            {
                if (pathPointer >= 0)
                {
                    setNode = MapGrid_A.Grid[path[pathPointer].x, path[pathPointer].y];
                    pathPointer--;
                }
                else
                {
                    anim.SetBool("Walk", false);
                }
            }

            if (pathPointer >= 0)
            {
                MoveToTarget();
                LookAtTarget();
            }

            if (targetPrevNode != null)
            {
                targetChanged = target.GetComponent<CharController_A>().currNode != targetPrevNode;
                targetPrevNode = target.GetComponent<CharController_A>().currNode;
            }
            else
            {
                targetPrevNode = target.GetComponent<CharController_A>().currNode;
            }
        }

    }

    void MoveToTarget()
    {
        if(setNode != null)
        {
            destination = setNode.worldPosition;
            destination.y = transform.position.y;
            Vector3 relPosition = destination - transform.position;
            transform.position += relPosition * (enemyVel * Time.deltaTime);
            anim.SetBool("Walk", true);
        }
    }

    void LookAtTarget()
    {
        transform.LookAt(destination);
    }
}
