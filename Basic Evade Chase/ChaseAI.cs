using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAI : MonoBehaviour {

    //Variables for fine tuning
    public GameObject gameManager;
    public Vector3 destination;
    public Transform target;
    public float enemyVel = 0.5f;
    public float stopRadius = 0.1f;
    public Node currNode;

   // CharController targetController;
    Animator anim;

	// Use this for initialization
	void Start () {
        SetTarget(target);
       
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("No Animator attached to this GameObject");
        }

        if (gameManager.GetComponent<MapGrid>() != null)
        {
            currNode = gameManager.GetComponent<MapGrid>().NodeFromWorldPoint(transform.position);
        }
        else
        {
            Debug.LogError("GameManager has no Grid Component");
        }

    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (gameManager.GetComponent<MapGrid>() != null)
        {
            //Debug.Log("Enemy:");
            if(gameManager.GetComponent<MapGrid>().Grid != null)
                currNode = gameManager.GetComponent<MapGrid>().NodeFromWorldPoint(transform.position);
        }
        else
        {
            Debug.LogError("GameManager has no Grid Component");
        }
        if (currNode != null)
        {
            //Move
            MoveToTarget();
            //Turn
            LookAtTarget();
        }
	}

    public void SetTarget(Transform t)
    {
        target = t;

        if(target == null)
        {
            Debug.LogError("Enemies do not have a Target");
        }
    }

    void LookAtTarget()
    {
        Vector3 relPosition = target.position - transform.position;       
        transform.rotation = Quaternion.LookRotation(new Vector3(relPosition.x,0.0f,relPosition.z));
    }

    void MoveToTarget()
    {
        Vector3 relPosition = target.position - transform.position;
        if (relPosition.sqrMagnitude > stopRadius)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, enemyVel * Time.deltaTime);
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
}
