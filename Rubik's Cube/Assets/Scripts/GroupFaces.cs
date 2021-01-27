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
                        Debug.DrawRay(start_position, ray_direction * hit.distance, Color.green, 2f);
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

    public string CheckFaceBelong(GameObject _piece, Vector3 _normal, bool _click)
    {
        string center_tag = "None";
        foreach (List<GameObject> pieces in GetListListGameObject())
        {
            if (pieces.Contains(_piece))
            {
                //center didnt save anything. It will be defined when it is needed.
                //  for now just borrow it instead of creating a new one.
                //  might be better just actually create a new one but whatever.
                center = turnListToTransform(pieces);
                if (_click)
                {
                    if (Vector3.Distance(_normal, center.forward) <= 0.01)
                        center_tag = center.tag;
                }
                else
                {
                    if(Vector3.Distance(_normal,center.forward) >= 1)
                    {
                        center_tag = center.tag;
                    }
                }
            }
        }
        return center_tag;
    }
    private Transform turnListToTransform(List<GameObject> GOs)
    {
        Transform resultTransform = tup.parent.transform;
        if (GOs == down)
            resultTransform = tdown.parent.transform;
        else if (GOs == left)
            resultTransform = tleft.parent.transform;
        else if (GOs == right)
            resultTransform = tright.parent.transform;
        else if (GOs == front)
            resultTransform = tfront.parent.transform;
        else if (GOs == back)
            resultTransform = tback.parent.transform;
        return resultTransform;
    }

    private List<List<GameObject>> GetListListGameObject()
    {
        List<List<GameObject>> combineList = new List<List<GameObject>> { up, down, right, left, front, back };
        return combineList;
    }  
    
    public string FindCornerSpinSide(GameObject _piece, Vector3 _normal, Vector2 mouse_movement)
    {
        string center_tag = "None";
        List<Transform> sides = new List<Transform>();
        List<float> distance = new List<float>();
        foreach (List<GameObject> pieces in GetListListGameObject())
        {
            if (pieces.Contains(_piece))
            {
                center = turnListToTransform(pieces);
                if (Vector3.Distance(_normal, center.forward) >= 1)
                {
                    sides.Add(center);
                    Vector2 spin_direction = new Vector2();
                    Vector3 cross_product = Vector3.Cross(center.forward, _normal);
                    spin_direction.x = -cross_product.x;
                    spin_direction.y = cross_product.y;
                    if (Mathf.Abs(cross_product.x) < Mathf.Abs(cross_product.z))
                        spin_direction.x = cross_product.z;
                    distance.Add(Mathf.Min(Vector2.Distance(spin_direction, mouse_movement),
                                           Vector2.Distance(-spin_direction, mouse_movement)));
                }
            }
        }
        if (distance[0] < distance[1])
            center_tag = sides[0].tag;
        else if(distance[1] < distance[0])  // not necessary but idk, I just want to put it here
            center_tag = sides[1].tag;
        return center_tag;
    }

}
