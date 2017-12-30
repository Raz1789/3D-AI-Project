using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    public GameObject gameManager;
    public Transform enemy;
    public float inputDelay = 0.1f;
    public float forwardVel = 2.0f;
    public float rotateVel = 100.0f;
    public bool AIModeOn = false;
    public float stopRadius = 0.7f;
    public float startRadius = 0.5f;
    public Node currNode;

    bool AIRun = false;

    public bool AIModeOnProp
    {
        set
        {
            AIModeOn = !AIModeOn;
        }
    }

    ChaseAI enemyAI;

    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput;

    Animator anim;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    private void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>() != null)
        {
            rBody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("No RigidBody Attached to the Character");
        }

        if (GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("No Animator Attached to the Character");
        }

        if (enemy.GetComponent<ChaseAI>() != null)
        {
            enemyAI = enemy.GetComponent<ChaseAI>();
        }
        else
        {
            Debug.LogError("No ChaseAI Attached to the Enemy");
        }

        if (gameManager.GetComponent<MapGrid>() != null)
        {
            if (gameManager.GetComponent<MapGrid>().Grid != null)
                currNode = gameManager.GetComponent<MapGrid>().NodeFromWorldPoint(transform.position);
        }
        else
        {
            Debug.LogError("GameManager has no Grid Component");
        }

        forwardInput = 0;
        turnInput = 0;
    }

    private void GetInput()
    {
        if (!AIModeOn)
        {
            forwardInput = Input.GetAxis("Vertical");
            turnInput = Input.GetAxis("Horizontal");
        }
        else
        {
            Vector3 relPosition = transform.position - enemy.position;

            float relPosMag = relPosition.sqrMagnitude;

            forwardInput = relPosMag / Mathf.Abs(relPosMag);

            float EulerAngleY = Quaternion.LookRotation(new Vector3(relPosition.x, 0.0f, relPosition.z)).eulerAngles.y;
            float relRotation = Mathf.Round( (EulerAngleY - targetRotation.eulerAngles.y) / 10.0f) * 10.0f;
            turnInput = relRotation / Mathf.Abs(relRotation);

        }
    }

    private void Update()
    {
        if (gameManager.GetComponent<MapGrid>() != null)
        {
            //Debug.Log("Player:");
            if (gameManager.GetComponent<MapGrid>().Grid != null)
                currNode = gameManager.GetComponent<MapGrid>().NodeFromWorldPoint(transform.position);
        }
        else
        {
            Debug.LogError("GameManager has no Grid Component");
        }
        if (currNode != null)
        {
            Turn();
            GetInput();
        }
    }

    private void FixedUpdate()
    {
        if (currNode != null)
        {
            Run();
        }
    }

    private void Run()
    {
        Vector3 relPosition = transform.position - enemy.position;
        float relPosMag = relPosition.magnitude;
        
        if (!AIModeOn)
        {
            if (Mathf.Abs(forwardInput) > inputDelay)
            {
                //move
                rBody.velocity = transform.forward * forwardInput * forwardVel;
                anim.SetBool("Walk", true);
            }
            else
            {
                //zero velocity
                rBody.velocity = Vector3.zero;
                anim.SetBool("Walk", false);
            }
        }
        else
        {
            if(!AIRun && relPosMag < startRadius)
            {
                AIRun = true;
            }
            if(AIRun)
            {
                //move
                rBody.velocity = Vector3.Normalize(currNode.worldPosition - enemyAI.currNode.worldPosition) * forwardInput * forwardVel;
                anim.SetBool("Walk", true);
            }
            if (AIRun && relPosMag > stopRadius)
            {
                AIRun = false;
            }
            if (!AIRun)
            {
                //zero velocity
                rBody.velocity = Vector3.zero;
                anim.SetBool("Walk", false);
            }

       
        }
    }
    

    private void Turn()
    {  
        if (Mathf.Abs(turnInput) > inputDelay)
        {
            //turn
            targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    } 
}
