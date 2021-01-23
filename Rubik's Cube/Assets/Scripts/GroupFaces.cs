using System.Collections.Generic;
using UnityEngine;

public class GroupFaces : MonoBehaviour
{
    public Transform tup;
    public Transform tdown;
    public Transform tleft;
    public Transform tright;
    public Transform tfront;
    public Transform tback;

    public List<GameObject> up;
    public List<GameObject> down;
    public List<GameObject> right;
    public List<GameObject> left;
    public List<GameObject> front;
    public List<GameObject> back;

    public Transform center;

    private readonly int Layer_mask = 1 << 8;
    private readonly float Piece_size = 2f;

    private void Start()
    {
        UpdateSides();
    }

    public void UpdateSides()
    {
        up = CastRays(tup);
        down = CastRays(tdown);
        front = CastRays(tfront);
        back = CastRays(tback);
        left = CastRays(tleft);
        right = CastRays(tright);
    }

    private List<GameObject> CastRays(Transform ray_transform)
    {
        //clear the list to fill in new objects later
        List<GameObject> pieces = new List<GameObject>();
        Vector3 initial_position = ray_transform.localPosition;
        Vector3 ray_direction = ray_transform.forward;

        // use ray_transform (empty object, child of center pieces)
        //  to create size 8 (excluding the center piece itself) object list
        //  put all those object into the list passed in.
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i != 0 || j != 0) //Fillter out the center
                {
                    ray_transform.localPosition = new Vector3(initial_position.x + i * Piece_size * 1 / 100,
                                                         initial_position.y + j * Piece_size * 1/100,
                                                         initial_position.z);
                    Vector3 start_position = ray_transform.position;
                    if (Physics.Raycast(start_position, ray_direction, out RaycastHit hit, Layer_mask))
                    {
                        pieces.Add(hit.collider.transform.parent.gameObject);
                    }
                    ray_transform.localPosition = initial_position;
                }
            }
        }
        return pieces;
    }

    private List<GameObject> findList(string tag)
    {
        List<GameObject> pieces = down;
        switch (tag)
        {
            case Tags.FRONT:
                center = tfront.parent;
                pieces = front;
                break;
            case Tags.BACK:
                center = tback.parent;
                pieces = back;
                break;
            case Tags.LEFT:
                center = tleft.parent;
                pieces = left;
                break;
            case Tags.RIGHT:
                center = tright.parent;
                pieces = right;
                break;
            case Tags.UP:
                center = tup.parent;
                pieces = up;
                break;
            case Tags.DOWN:
                center = tdown.parent;
                pieces = down;
                break;
        }

        return pieces;
    }

    private void parentPieces(List<GameObject> pieces)
    {
        foreach (GameObject piece in pieces)
        {
            piece.transform.parent = center.transform;
        }
    }

    public Transform GroupPieces(string tag)
    {
        parentPieces(findList(tag));
        return center;
    }

    public void UnparentPieces(string tag)
    {
        foreach (GameObject piece in findList(tag))
        {
            piece.transform.parent = center.transform.parent;
        }
    }

    public string CheckFaceBelong(GameObject _piece, Vector3 _normal)
    {
        string center_tag = "None";
        foreach (List<GameObject> pieces in GetListListGameObject())
        {
            if (pieces.Contains(_piece))
            {
                if (pieces == up)
                {
                    if (Vector3.Distance( _normal, -tup.forward) <= 0.001)
                    {
                        center_tag = Tags.UP;
                    }
                }
                else if (pieces == down)
                {
                    if (Vector3.Distance(_normal, -tdown.forward) <= 0.001)
                    {
                        center_tag = Tags.DOWN;
                    }
                }
                else if (pieces == left)
                {
                    if (Vector3.Distance(_normal, -tleft.forward) <= 0.001)
                    {
                        center_tag = Tags.LEFT;
                    }
                }
                else if (pieces == right)
                {
                    if (Vector3.Distance(_normal, -tright.forward) <= 0.001)
                    {
                        center_tag = Tags.RIGHT;
                    }
                }
                else if (pieces == front)
                {
                    if (Vector3.Distance(_normal, -tfront.forward) <= 0.001)
                    {
                        center_tag = Tags.FRONT;
                    }
                }
                else if (pieces == back)
                {
                    if (Vector3.Distance(_normal, -tback.forward) <= 0.001)
                    {
                        center_tag = Tags.BACK;
                    }
                }
            }
        }
        return center_tag;
    }
    
    private List<List<GameObject>> GetListListGameObject()
    {
        List<List<GameObject>> combineList = new List<List<GameObject>> { up, down, right, left, front, back };
        return combineList;
    }   
}
