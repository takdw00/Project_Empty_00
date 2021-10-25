using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offsetFromTarget;

    public float smoothTime = 0.3f;

    private Camera cam;
    private Vector3 currentVelocity;


    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        transform.position = targetTransform.position + offsetFromTarget;
    }

    private void Update()
    {
        SmoothFollowTarget();
    }

    private void SmoothFollowTarget() 
    {
        if (targetTransform != null) 
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position + offsetFromTarget, ref currentVelocity, smoothTime);
        }
    }
}
