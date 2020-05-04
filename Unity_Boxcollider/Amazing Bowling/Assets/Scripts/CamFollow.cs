using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public enum State
    {
        Idle,
        Ready,
        Tracking
    }

    private State state
    {
        set
        {
            switch (value)
            {
                case State.Idle:
                    targetZoomSize = roundReadyZoomSize;
                    break;
                case State.Ready:
                    targetZoomSize = readyShotZoomSize;
                    break;
                case State.Tracking:
                    targetZoomSize = trackingZoomSize;
                    break;
            }
        }
    }

    private Transform target;

    public float smoothTime = 0.2f; // 줌 변환시 딜레이

    private Vector3 lastMovingVelocity;
    private Vector3 targetPosition;

    private Camera cam;
    private float targetZoomSize = 5f;

    private const float roundReadyZoomSize = 14.5f; // 각 상태에 따른 카메라 줌 사이즈
    private const float readyShotZoomSize = 5f; // 각 상태에 따른 카메라 줌 사이즈
    private const float trackingZoomSize = 10f; // 각 상태에 따른 카메라 줌 사이즈

    private float lastZoomSpeed;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        state = State.Idle;
    }

    private void Move()
    {
        targetPosition = target.transform.position;

        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref lastMovingVelocity, smoothTime);

        transform.position = smoothPosition;
    }

    private void Zoom()
    {
        float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomSize, ref lastZoomSpeed, smoothTime);

        cam.orthographicSize = smoothZoomSize;
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            Move();
            Zoom();
        }
    }

    public void Reset()
    {
        state = State.Idle;
    }

    public void SetTarget(Transform newTarget, State newState)
    {
        target = newTarget;
        state = newState;
    }
}
