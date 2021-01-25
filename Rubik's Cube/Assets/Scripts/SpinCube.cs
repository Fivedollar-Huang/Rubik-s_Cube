using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCube : MonoBehaviour
{
    public float sensitive = 200;

    private GroupFaces groupFaces;
    private CubeController cubeController;

    public Transform Center;

    private Quaternion euler_target;

    private bool left_click = false;
    private bool free_spin = false;
    private bool spinning = false;
    private string TAG;

    private int pending_spin = 0;
    private Vector2 mouse_movement;
    private bool change_direction = false;
    private float rotate_direction = 0;

    private RaycastHit hit;

    private void Start()
    {
        cubeController = GetComponent<CubeController>();
        groupFaces = GetComponent<GroupFaces>();
    }

    private void Update()
    {
        if (left_click && !free_spin)
        {
            if (cubeController.MouseMovement() != Vector2.zero) free_spin = true; 
        }
        if (free_spin)
        {
            if (left_click)
            {
                mouse_movement = cubeController.MouseMovement();
                mouse_movement *= sensitive * Time.deltaTime;
                FreeSpin();
            }
        }
        if (spinning)
        {
            spinSide();
        }
    }

    //****************************************************************************************
    public void leftClick()
    {
        if (!spinning)
        {
            left_click = true;
            click();
        }
        else
        {
            checkClick();
        }
    }
    public void leftRelease()
    {
        if (left_click)
        {
            left_click = false;
            if (TAG != Tags.NULL && TAG != Tags.UNTAGGED)
                if (!free_spin)
                    click_spin();
                else
                    fixSpin();
        }
    }

    //**************************************************************************************
    private void checkClick()
    {
        RaycastHit save_hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out save_hit))
        {
            if (save_hit.transform.tag == TAG)
            {
                pending_spin++;
                if (pending_spin == 4) pending_spin = 0;
            }
        }
    }
    private void addSpinToEuler_target()
    {
        float target_rotation = Center.transform.localRotation.eulerAngles.z + 90;
        if (target_rotation == 360) target_rotation = 0;

        euler_target = Quaternion.Euler(Center.localRotation.eulerAngles.x,
                                                    Center.localRotation.eulerAngles.y,
                                                    target_rotation);
    }

    private void click()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            TAG = hit.transform.tag;
        }
        else TAG = Tags.NULL;
    }
    private void click_spin()
    {
        if (TAG != Tags.UNTAGGED)
        {
            if (TAG == Tags.CORNER || TAG == Tags.EDGE)
            {
                TAG = groupFaces.CheckFaceBelong(hit.transform.parent.transform.gameObject, hit.normal, true, false);
            }
        }
        if (TAG == "None") return;
        spinning = true;
        Center = groupFaces.GroupPieces(TAG);
        addSpinToEuler_target();
    }

    private void spinSide()
    {
        Center.localRotation = Quaternion.RotateTowards(Center.localRotation, 
                                                        euler_target,
                                                        sensitive * Time.deltaTime);
        if (Center.localRotation.eulerAngles.x % 90 == 0 &&
            Center.localRotation.eulerAngles.y % 90 == 0 &&
            Center.localRotation.eulerAngles.z % 90 == 0 ||
            Center.localRotation == euler_target)
        {
            if (pending_spin == 0)
            {
                spinning = false;
                free_spin = false;
                groupFaces.UnparentPieces(TAG);
                groupFaces.UpdateSides();
            }
            else
            {
                addSpinToEuler_target();
                pending_spin--;
            }
        }
    }

    private void FreeSpin()
    {
        if (TAG != Tags.UNTAGGED)
        {
            if (TAG == Tags.CORNER || TAG == Tags.EDGE)
            {
                TAG = groupFaces.CheckFaceBelong(hit.transform.parent.transform.gameObject, hit.normal, false, false);
            }
        }
        if (TAG == "None") return;
        Center = groupFaces.GroupPieces(TAG);

        Vector3 cross_product = Vector3.Cross(Center.forward, hit.normal);
        float x_factor = -cross_product.x;
        float y_factor = cross_product.y;
        if(Mathf.Abs(cross_product.x) < Mathf.Abs(cross_product.z))
            x_factor = cross_product.z;

        float target_rotation = mouse_movement.x * x_factor + mouse_movement.y * y_factor;
        Center.Rotate(0, 0, target_rotation, Space.Self);
        if (!change_direction)
        {
            if (rotate_direction == 0)
            {
                rotate_direction = target_rotation;
            }
            else if (rotate_direction > 0)
            {
                if (target_rotation < -0.1)
                {
                    change_direction = true;
                    rotate_direction = 0;
                }
            }
            else if (rotate_direction < 0)
            {
                if (target_rotation > 0.1)
                {
                    change_direction = true;
                    rotate_direction = 0;
                }
            }
        }
    }

    private void fixSpin()
    {
        Vector3 target_rotation = Center.localRotation.eulerAngles;
        if (change_direction || rotate_direction == 0)
        {
            target_rotation = fix_angles(target_rotation, 0);
        }
        else if (rotate_direction > 0)
        {
            target_rotation = fix_angles(target_rotation, 1);
        }
        else if(rotate_direction < 0)
        {
            target_rotation = fix_angles(target_rotation, -1);
        }
        change_direction = false;
        rotate_direction = 0;
        euler_target = Quaternion.Euler(target_rotation);
        spinning = true;
    }

    private Vector3 fix_angles(Vector3 _v, int _dir)
    {
        Vector3 new_v;
        new_v.x = Mathf.Round(_v.x/ 90) * 90;
        new_v.y = Mathf.Round(_v.y / 90) * 90;
        new_v.z = Mathf.Round(_v.z / 90) * 90;
        if (_dir > 0)
        {
            if (new_v.x < _v.x) new_v.x += 90;
            if (new_v.y < _v.y) new_v.y += 90;
            if (new_v.z < _v.z) new_v.z += 90;
        }
        if (_dir < 0)
        {
            if (new_v.x > _v.x) new_v.x -= 90;
            if (new_v.y > _v.y) new_v.y -= 90;
            if (new_v.z > _v.z) new_v.z -= 90;
        }
        return new_v;
    }
}
