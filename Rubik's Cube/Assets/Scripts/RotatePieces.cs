using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePieces : MonoBehaviour
{
    public Transform targetTransform;

    [SerializeField]
    private float rotateSpeed = 150f;
    private bool rotating = false;

    private Vector3 localAngle;
    private Vector3 targetAngle;
    private Transform pieceToRotate;
    private List<Transform> rotatingPieces;

    private Quaternion eulerLocal;
    private Quaternion eulerTarget;

    GroupPieces groupPieces;

    private void Start()
    {
        groupPieces = GetComponent<GroupPieces>();
    }

    private void Update()
    {
        if (rotating)
        {
            RotateCenterPiece();
        }
    }

    private float FixTarget(float _float)
    {
        _float %= 360;
        _float = Mathf.RoundToInt(_float / 90) * 90;
        return _float;
    }

    public void RotateCenterPiece(List<Transform> centerPiece, float angle, char side = ' ')
    {
        rotatingPieces = centerPiece;
        pieceToRotate = centerPiece[0];
        localAngle = pieceToRotate.localRotation.eulerAngles;
        print(localAngle);
        if (side == ' ')
        {
            targetAngle = new Vector3(localAngle.x, localAngle.y, FixTarget(localAngle.z + angle));
        }
        else
        {
            targetTransform.position = pieceToRotate.position;
            targetTransform.rotation = pieceToRotate.rotation;
            if (side == 'E')
            { 
                targetTransform.RotateAround(targetTransform.position, -targetTransform.transform.parent.up, angle);
                //targetAngle = new Vector3(localAngle.x, FixTarget(localAngle.y + angle), localAngle.z);
            }
            else if (side == 'S')
            {
                targetTransform.RotateAround(targetTransform.position, targetTransform.transform.parent.right, angle);
                //targetAngle = new Vector3(FixTarget(localAngle.x + angle), localAngle.y, localAngle.z);
            }
            else if(side == 'M')
            {
                targetTransform.RotateAround(targetTransform.position, targetTransform.transform.parent.forward, angle);
            }
            targetAngle = targetTransform.rotation.eulerAngles;
        }
        rotating = true;
        eulerTarget = Quaternion.Euler(targetAngle);
    }

    private void RotateCenterPiece()
    {
        localAngle = pieceToRotate.localRotation.eulerAngles;
        eulerLocal = Quaternion.Euler(localAngle);
        pieceToRotate.localRotation = Quaternion.RotateTowards(eulerLocal,
                                                               eulerTarget,
                                                               rotateSpeed * Time.deltaTime);
        if (Mathf.Abs(Mathf.Abs(eulerLocal.x) - Mathf.Abs(eulerTarget.x)) < 0.01 &&
            Mathf.Abs(Mathf.Abs(eulerLocal.y) - Mathf.Abs(eulerTarget.y)) < 0.01 &&
            Mathf.Abs(Mathf.Abs(eulerLocal.z) - Mathf.Abs(eulerTarget.z)) < 0.01 &&
            Mathf.Abs(Mathf.Abs(eulerLocal.w) - Mathf.Abs(eulerTarget.w)) < 0.01)
        {
            rotating = false;
            groupPieces.FinishGAR(rotatingPieces);
        }
    }


}
