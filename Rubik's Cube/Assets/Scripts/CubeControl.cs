using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour
{
    GroupPieces groupPieces;

    private List<Transform> currentPieces;
    private List<Transform> savedPieces;
    private List<Vector3> savedDirection;

    private bool shift;

    // Start is called before the first frame update
    void Start()
    {
        groupPieces = GetComponent<GroupPieces>();
        savedPieces = new List<Transform>();
        savedDirection = new List<Vector3>();
        for (int i = 0; i < 27; i++)
        {
            savedPieces.Add(groupPieces.pieces[i]);
            savedDirection.Add(groupPieces.pieces[i].forward);
        }
        UpdatePieces();

        shift = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) shift = true;
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) shift = false;
        CheckSolved();
        CheckRotation();
    }

    private void UpdatePieces()
    {
        currentPieces = groupPieces.pieces;
    }

    private void CheckSolved()
    {
        UpdatePieces();
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            for (int i = 0; i < 27; i++)
            {
                if (currentPieces[i] != savedPieces[i] || currentPieces[i].forward != savedDirection[i])
                {
                    print("False");
                    return;
                }
            }
            print("True");
        }
    }

    private void CheckRotation()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            groupPieces.GroupAndRotate("F", shift);
            print("F");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            groupPieces.GroupAndRotate("B", shift);
            print("B");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            groupPieces.GroupAndRotate("R", shift);
            print("R");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            groupPieces.GroupAndRotate("L", shift);
            print("L");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            groupPieces.GroupAndRotate("U", shift);
            print("U");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            groupPieces.GroupAndRotate("D", shift);
            print("D");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            groupPieces.GroupAndRotate("M", shift);
            print("M");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            groupPieces.GroupAndRotate("S", shift);
            print("S");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            groupPieces.GroupAndRotate("E", shift);
            print("E");
        }
    }

}
