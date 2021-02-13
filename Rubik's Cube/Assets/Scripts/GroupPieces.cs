using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *             6    7    8
 *           3    4    5
 *         0    1    2
 *         
 *         
 *             15   16   17
 *           12   13   14
 *         9    10   11
 *         
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

    private List<Transform> groupedSide;
    private int groupedSize;
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

    public void GroupAndRotate(string move, bool reverse, bool lower)
    {
        if (processing) return;
        float angle = 90;
        if (reverse) angle = -90;
        groupedSize = 9;
        if (!lower)
        {
            switch (move)
            {
                case "F":
                    front = UpdateGroup(front, reverse);
                    GroupRotateSide(front);
                    RememberGoupedSide(front);
                    rotatePieces.RotateCenterPiece(pieces[front[0]], angle);
                    processing = true;
                    return;
                case "B":
                    back = UpdateGroup(back, reverse);
                    GroupRotateSide(back);
                    RememberGoupedSide(back);
                    rotatePieces.RotateCenterPiece(pieces[back[0]], angle);
                    processing = true;
                    return;
                case "L":
                    left = UpdateGroup(left, reverse);
                    GroupRotateSide(left);
                    RememberGoupedSide(left);
                    rotatePieces.RotateCenterPiece(pieces[left[0]], angle);
                    processing = true;
                    return;
                case "R":
                    right = UpdateGroup(right, reverse);
                    GroupRotateSide(right);
                    RememberGoupedSide(right);
                    rotatePieces.RotateCenterPiece(pieces[right[0]], angle);
                    processing = true;
                    return;
                case "U":
                    up = UpdateGroup(up, reverse);
                    GroupRotateSide(up);
                    RememberGoupedSide(up);
                    rotatePieces.RotateCenterPiece(pieces[up[0]], angle);
                    processing = true;
                    return;
                case "D":
                    down = UpdateGroup(down, reverse);
                    GroupRotateSide(down);
                    RememberGoupedSide(down);
                    rotatePieces.RotateCenterPiece(pieces[down[0]], angle);
                    processing = true;
                    return;
                case "M":
                    middle = UpdateGroup(middle, reverse);
                    GroupRotateSide(middle);
                    RememberGoupedSide(middle);
                    rotatePieces.RotateCenterPiece(pieces[middle[0]], angle, 'M');
                    processing = true;
                    return;
                case "E":
                    equatorial = UpdateGroup(equatorial, reverse);
                    GroupRotateSide(equatorial);
                    RememberGoupedSide(equatorial);
                    rotatePieces.RotateCenterPiece(pieces[equatorial[0]], angle, 'E');
                    processing = true;
                    return;
                case "S":
                    standing = UpdateGroup(standing, reverse);
                    GroupRotateSide(standing);
                    RememberGoupedSide(standing);
                    rotatePieces.RotateCenterPiece(pieces[standing[0]], angle, 'S');
                    processing = true;
                    return;
            }
            return;
        }

        groupedSize = 18;
        List<int> combinedList;
        switch (move)
        {
            case "F":
                front = UpdateGroup(front, reverse);
                standing = UpdateGroup(standing, reverse);

                combinedList = combineList(standing, front);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[front[0]], angle);
                processing = true;
                return;
            case "B":
                back = UpdateGroup(back, reverse);
                standing = UpdateGroup(standing, !reverse);

                combinedList = combineList(standing, back);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[back[0]], angle);
                processing = true;
                return;
            case "R":
                right = UpdateGroup(right, reverse);
                middle = UpdateGroup(middle, !reverse);

                combinedList = combineList(middle, right);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[right[0]], angle);
                processing = true;
                return;
            case "L":
                left = UpdateGroup(left, reverse);
                middle = UpdateGroup(middle, reverse);

                combinedList = combineList(middle, left);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[left[0]], angle);
                processing = true;
                return;
            case "U":
                up = UpdateGroup(up, reverse);
                equatorial = UpdateGroup(equatorial, !reverse);

                combinedList = combineList(equatorial, up);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[up[0]], angle);
                processing = true;
                return;
            case "D":
                down = UpdateGroup(down, reverse);
                equatorial = UpdateGroup(equatorial, reverse);

                combinedList = combineList(equatorial, down);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[down[0]], angle);
                processing = true;
                return;
        }
        groupedSize = 27;
        switch (move)
        {
            case "X":
                right = UpdateGroup(right, reverse);
                middle = UpdateGroup(middle, !reverse);
                left = UpdateGroup(left, !reverse);

                combinedList = combineList(middle, right);
                combinedList = combineList(left, combinedList);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[right[0]], angle);
                processing = true;
                return;
            case "Y":
                up = UpdateGroup(up, reverse);
                equatorial = UpdateGroup(equatorial, !reverse);
                down = UpdateGroup(down, !reverse);

                combinedList = combineList(equatorial, up);
                combinedList = combineList(down, combinedList);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[up[0]], angle);
                processing = true;
                return;
            case "Z":
                front = UpdateGroup(front, reverse);
                standing = UpdateGroup(standing, reverse);
                back = UpdateGroup(back, !reverse);

                combinedList = combineList(standing, front);
                combinedList = combineList(back, combinedList);
                GroupRotateSide(combinedList);
                RememberGoupedSide(combinedList);
                rotatePieces.RotateCenterPiece(pieces[front[0]], angle);
                processing = true;
                return;
        }
    }

    private List<int> combineList(List<int> from, List<int> to)
    {
        List<int> result = new List<int>();
        foreach(int i in to)
        {
            result.Add(i);
        }
        foreach (int i in from)
        {
            result.Add(i);
        }
        return result;
    }

    private void RememberGoupedSide(List<int> side)
    {
        groupedSide = new List<Transform>();
        for (int i = 0; i < groupedSize; i++)
        {
            groupedSide.Add(pieces[side[i]]);
        }
    }

    public void FinishGAR()
    {
        UngroupRotateSide();
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
        for (int i = 1; i < groupedSize; i++)
        {
            pieces[side[i]].transform.parent = pieces[side[0]].transform;
        }
    }

    private void UngroupRotateSide()
    {
        for (int i = 1; i < groupedSize; i++)
        {
            groupedSide[i].transform.parent = groupedSide[0].transform.parent.transform;
        }
    }

}
