using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public float sensitive = 200f;

    private CubeController cubeController;

    private Vector2 mouse_direction;
    private Vector3 target_rotation;

    private bool fix_rotation = false;

    private bool right_drag = false;
    private bool right_click = false;
    private bool scroll_click = false;

    private void Start()
    {
        cubeController = GetComponent<CubeController>();
    }

    void Update()
    {
        if (right_click)
        {
            mouse_direction = cubeController.MouseMovement();
            if ( mouse_direction != Vector2.zero)
            {
                right_drag = true;
            }
        }
        if (scroll_click)
        {
            resetRotation();
        }
        if (right_drag)
        {
            rotateCube();
        }
        if (fix_rotation)
        {
            fixRotation();
        }
        
    }
    
    public void rightClick()
    {
        right_click = true;
    }
    public void rightRelease()
    {
        if (!right_drag) fix_rotation = true;
        right_click = false;
        right_drag = false;
    }
    public void scrollClick()
    {
        scroll_click = true;
    }



    void rotateCube()
    {
        target_rotation = new Vector3();
        target_rotation.x += -mouse_direction.y;
        target_rotation.y += -mouse_direction.x;
        target_rotation.z += mouse_direction.y;

        transform.Rotate(target_rotation * sensitive * Time.deltaTime, Space.World);
    }

    void fixRotation()
    {
        target_rotation = transform.rotation.eulerAngles;
        roundAngle();
        if (transform.rotation.eulerAngles == target_rotation)
        {
            fix_rotation = false;
            return;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                      Quaternion.Euler(target_rotation),
                                                      sensitive * Time.deltaTime);
    }

    void roundAngle()
    {
        target_rotation.x = Mathf.Round(target_rotation.x / 90) * 90;
        target_rotation.y = Mathf.Round(target_rotation.y / 90) * 90;
        target_rotation.z = Mathf.Round(target_rotation.z / 90) * 90;
    }

    void resetRotation()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                      Quaternion.Euler(Vector3.zero),
                                                      2 * sensitive * Time.deltaTime);
        if (transform.rotation.eulerAngles == Vector3.zero)
            scroll_click = false;
    }
}

