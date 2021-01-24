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
            print("free_spin");
            mouse_movement = cubeController.MouseMovement();
        }
        if (spinning)
        {
            spinSide();
        }
    }

    public void leftClick()
    {
        if (!spinning)
        {
            left_click = true;
            click();
        }
        else
        {
            pending_spin++;
            if (pending_spin == 4) pending_spin = 0;
        }
    }
    public void leftRelease()
    {
        if (left_click)
        {
            left_click = false;
            if (!free_spin)
            {
                click_spin();
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
            if (TAG != Tags.UNTAGGED)
            {
                if (TAG == Tags.CORNER || TAG == Tags.EDGE)
                {
                    TAG = groupFaces.CheckFaceBelong(hit.transform.parent.transform.gameObject, hit.normal, true, true);
                }
            }
        }   
    }
    private void click_spin()
    {
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
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            // face needed to spin cross with hit normal 
            //  if clicked at the front, moving right is + mouse x, with cross product for x
            //  multiply to get negative rotating angle (counter clock wise)
        }
    }

}
