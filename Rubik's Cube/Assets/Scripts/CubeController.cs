using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float sensitive = 0.002f;
    private bool left_click = false;

    SpinWholeCube SpinCube;


    // Start is called before the first frame update
    void Start()
    {
        SpinCube = GetComponent<SpinWholeCube>();
    }

    // Update is called once per frame
    void Update()
    {
        checkUserInput();
    }

    void checkUserInput()
    {
        if (Input.GetMouseButtonDown(0)) left_click = true;
        else if (Input.GetMouseButtonUp(0)) left_click = false;
        else if (Input.GetMouseButtonDown(1)) SpinCube.rightClick();
        else if (Input.GetMouseButtonUp(1)) SpinCube.rightRelease();
        else if (Input.GetMouseButtonDown(2)) SpinCube.scrollClick();

        if (left_click)
        {
        }

    }
}