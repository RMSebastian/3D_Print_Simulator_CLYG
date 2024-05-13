using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCameraControl : MonoBehaviour
{
    [SerializeField]private float rotationSpeed = 1.0f;
    [SerializeField] private float deadZone = 30;
    [SerializeField] private CinemachineFreeLook cm_freelook;
    private Vector2 lastPosition;
    private bool firstTouch;
    private bool canRotate = true;
    void Update()
    {
        if (!canRotate) return;
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
            if(!firstTouch)
            {
                firstTouch = true;
                return;
            }
            else
            {
                RotateCamera(deltaPosition.x);
            }    
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentPosition = Input.mousePosition;
            Vector2 deltaPosition = currentPosition - lastPosition;
            lastPosition = currentPosition;
            if (!firstTouch)
            {
                firstTouch = true;
                return;
            }
            else
            {
                RotateCamera(deltaPosition.x + deltaPosition.y);
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)
        {
            firstTouch = false;
            lastPosition = Vector2.zero;
        }
    }

    private void RotateCamera(float delta)
    {
        if (delta >= deadZone || delta <= -deadZone)
        {
            delta = Mathf.Clamp(delta, -rotationSpeed, rotationSpeed);
            cm_freelook.m_XAxis.Value = delta;
        }

    }
    public void CanRotate(bool rotate)=> canRotate = rotate;
}
