using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Vector2 mouse_movement;

    private RotateCube rotateCube;
    private SpinCube spinCube;


    // Start is called before the first frame update
    void Start()
    {
        rotateCube = GetComponent<RotateCube>();
        spinCube = GetComponent<SpinCube>();
    }

    // Update is called once per frame
    void Update()
    {
        checkUserInput();
    }

    void checkUserInput()
    {
        if (Input.GetMouseButtonDown(0)) spinCube.leftClick();
        else if (Input.GetMouseButtonUp(0)) spinCube.leftRelease();
        else if (Input.GetMouseButtonDown(1)) rotateCube.rightClick();
        else if (Input.GetMouseButtonUp(1)) rotateCube.rightRelease();
        else if (Input.GetMouseButtonDown(2)) rotateCube.scrollClick();
        mouse_movement = new Vector2(Input.GetAxis(MouseAxis.MOUSE_X), Input.GetAxis(MouseAxis.MOUSE_Y));
    }

    public Vector2 MouseMovement()
    {
        return mouse_movement;
    }
}