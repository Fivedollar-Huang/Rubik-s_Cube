using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *             6    7    8
 *           3    4    5
 *         0    1    2
 *         
 *             15   16   17
 *           12   13   14
 *         9    10   11
 *         
 *             24   25   26
 *           21   22   23
 *         18   19   20
 */


public class GroupPieces : MonoBehaviour
{
    public List<Transform> pieces;

    private List<int> up;
    private List<int> down;
    private List<int> right;
    private List<int> left;
    private List<int> front;
    private List<int> back;

    private List<int> middle;
    private List<int> equatorial;
    private List<int> standing;

    private bool processing;

    RotatePieces rotatePieces;

    private void Start()
    {
        rotatePieces = GetComponent<RotatePieces>();

        up = new List<int>()
        {
            4, 0, 3, 6, 7, 8, 5, 2, 1
        };
        down = new List<int>()
        {
            22, 18, 19, 20, 23, 26, 25, 24, 21
        };
        right = new List<int>()
        {
            14, 2, 5, 8, 17, 26, 23, 20, 11
        };
        left = new List<int>()
        {
            12, 0, 9, 18, 21, 24, 15, 6, 3
        };
        front = new List<int>()
        {
            10, 0, 1, 2, 11, 20, 19, 18, 9
        };
        back = new List<int>()
        {
            16, 8, 7, 6, 15, 24, 25, 26, 17
        };
        middle = new List<int>()
        {
            13, 1, 10, 19, 22, 25, 16, 7, 4
        };
        equatorial = new List<int>()
        {
            13, 9, 10, 11, 14, 17, 16, 15, 12
        };
        standing = new List<int>()
        {
            13, 3, 4, 5, 14, 23, 22, 21, 12
        };

        processing = false;
    }

    public void GroupAndRotate(string move, bool reverse)
    {
        if (processing) return;
        float angle = 90;
        if (reverse) angle = -90;
        switch (move)
        {
            case "F":
                front = UpdateGroup(front, reverse);
                GroupRotateSide(front);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(front), angle);
                processing = true;
                break;
            case "B":
                back = UpdateGroup(back, reverse);
                GroupRotateSide(back);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(back), angle);
                processing = true;
                break;
            case "L":
                left = UpdateGroup(left, reverse);
                GroupRotateSide(left);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(left), angle);
                processing = true;
                break;
            case "R":
                right = UpdateGroup(right, reverse);
                GroupRotateSide(right);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(right), angle);
                processing = true;
                break;
            case "U":
                up = UpdateGroup(up, reverse);
                GroupRotateSide(up);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(up), angle);
                processing = true;
                break;
            case "D":
                down = UpdateGroup(down, reverse);
                GroupRotateSide(down);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(down), angle);
                processing = true;
                break;
            case "M":
                middle = UpdateGroup(middle, reverse);
                GroupRotateSide(middle);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(middle), angle);
                processing = true;
                break;
            case "E":
                equatorial = UpdateGroup(equatorial, reverse);
                GroupRotateSide(equatorial);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(equatorial), angle);
                processing = true;
                break;
            case "S":
                standing = UpdateGroup(standing, reverse);
                GroupRotateSide(standing);
                rotatePieces.RotateCenterPiece(ChangeIntToTransform(standing), angle);
                processing = true;
                break;
        }
    }

    private List<Transform> ChangeIntToTransform(List<int> side)
    {
        List<Transform> result = new List<Transform>();
        for (int i = 0; i < 9; i++)
        {
            result.Add(pieces[side[i]]);
        }
        return result;
    }

    public void FinishGAR(List<Transform> side)
    {
        UngroupRotateSide(side);
        processing = false;
    }

    private List<int> UpdateGroup(List<int> side, bool reverse)
    {
        if (reverse)
        {
            Transform saved_piece1 = pieces[side[1]];
            Transform saved_piece2 = pieces[side[2]];
            for (int i = 1; i < 7; i++)
            {
                pieces[side[i]] = pieces[side[i + 2]];
            }
            pieces[side[7]] = saved_piece1;
            pieces[side[8]] = saved_piece2;
        }
        else
        {
            Transform saved_piece1 = pieces[side[7]];
            Transform saved_piece2 = pieces[side[8]];
            for (int i = 8; i > 2; i--)
            {
                pieces[side[i]] = pieces[side[i - 2]];
            }
            pieces[side[1]] = saved_piece1;
            pieces[side[2]] = saved_piece2;
        }
        return side;
    }

    private void GroupRotateSide(List<int> side)
    {
        for (int i = 1; i < 9; i++)
        {
            pieces[side[i]].transform.parent = pieces[side[0]].transform;
        }
    }

    private void UngroupRotateSide(List<Transform> side)
    {
        for (int i = 1; i < 9; i++)
        {
            side[i].transform.parent = side[0].transform.parent.transform;
        }
    }

}
