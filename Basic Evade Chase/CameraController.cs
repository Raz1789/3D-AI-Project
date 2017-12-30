using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0.0f, 0.355f, -0.5f);
    public Vector3 zoomFactor = new Vector3(0.0f, 0.05f, 0.05f);
    public float xTilt = 26.0f;
    public Vector3 AICameraPos = new Vector3(0.0f, 5.5f, 0.0f);

    bool AICameraSet = false;
    Vector3 destination = Vector3.zero;
    CharController charController;
   // float rotateVel = 0.0f;

    private void Start()
    {
        SetCameraTarget(target);
    }

    public void SetCameraTarget(Transform t)
    {
        target = t;

        if (target != null)
        {
            if (target.GetComponent<CharController>() != null)
            {
                charController = target.GetComponent<CharController>();
            }
            else
            {
                Debug.LogError("Camera's Target needs CharacterController");

            }
        }
        else
        {
            Debug.LogError("Your cam needs a target");
        }
    }

    private void LateUpdate()
    {
        if (!charController.AIModeOn)
        {
            if (AICameraSet)
            {
                AICameraSet = false;
                //transform.rotation = Quaternion.identity;
                transform.rotation = Quaternion.AngleAxis(xTilt, Vector3.right);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (offsetFromTarget.y - zoomFactor.y >= 0.45)
                {
                    offsetFromTarget.y -= zoomFactor.y;
                }
                else
                {
                    offsetFromTarget.y = 0.45f;
                }

                if (offsetFromTarget.z + zoomFactor.z <= -0.45f)
                {
                    offsetFromTarget.z += zoomFactor.z;
                }
                else
                {
                    offsetFromTarget.z = -0.45f;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (offsetFromTarget.y + zoomFactor.y <= 2)
                {
                    offsetFromTarget.y += zoomFactor.y;
                }
                else
                {
                    offsetFromTarget.y = 2.0f;
                }

                if (offsetFromTarget.z - zoomFactor.z >= -2.0f)
                {
                    offsetFromTarget.z -= zoomFactor.z;
                }
                else
                {
                    offsetFromTarget.z = -2.0f;
                }
            }
            //moving
            MoveToTarget();
            //rotating
            LookAtTarget();
        }
        else if (charController.AIModeOn && !AICameraSet)
        {
            AICameraSet = true;
            transform.position = AICameraPos;
            //transform.rotation = Quaternion.identity;
            transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.right);
        }
    }

    void MoveToTarget()
    {
        destination = charController.TargetRotation * offsetFromTarget;
        destination += target.position;
        if (destination.z < -1.62f || destination.z > 1.62f)
        {
            destination.z = transform.position.z;
        }
        if (destination.x < -2.4f || destination.x > 2.4f)
        {
            destination.x = transform.position.x;
        }
            transform.position = destination;
    }

    void LookAtTarget()
    {
        transform.LookAt(target);
        //float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0.0f);
    }

}
