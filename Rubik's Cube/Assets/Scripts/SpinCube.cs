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
    private bool spin = false;
    private bool spinning = false;
    private string TAG;

    private void Start()
    {
        cubeController = GetComponent<CubeController>();
        groupFaces = GetComponent<GroupFaces>();
    }

    private void Update()
    {
        if (left_click && !spin)
        {
            if (cubeController.MouseMovement() != Vector2.zero) spin = true;
        }
        if (spinning)
        {
            spinSide();
        }
    }

    public void leftClick()
    {
        left_click = true;
    }
    public void leftRelease()
    {
        left_click = false;
        if (spin)
        {
            spin = false;
        }
        else
        {
            click();
        }
    }

    private void click()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            TAG = hit.transform.tag;
            if (TAG != Tags.UNTAGGED)
            {
                spinning = true;
                Center = groupFaces.GroupPieces(TAG);
                float target_rotation = Center.transform.localRotation.eulerAngles.z + 90;
                if (target_rotation == 360) target_rotation = 0;

                euler_target = Quaternion.Euler(Center.localRotation.eulerAngles.x,
                                                           Center.localRotation.eulerAngles.y,
                                                           target_rotation);
            }
        }   
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
            spinning = false;
            groupFaces.UnparentPieces(TAG);
            groupFaces.UpdateSides();
        }
        else
        {
            print("something");
            print(Center.localRotation.eulerAngles);
            print(euler_target.eulerAngles);
            print(Center.localRotation == euler_target);
        }
    }
}
