using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePieces : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 150f;
    private bool rotating = false;

    private Vector3 localAngle;
    private Vector3 targetAngle;
    private Transform pieceToRotate;
    private List<Transform> rotatingPieces;

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

    public void RotateCenterPiece(List<Transform> centerPiece, float angle)
    {
        rotatingPieces = centerPiece;
        pieceToRotate = centerPiece[0];
        localAngle = pieceToRotate.localRotation.eulerAngles;
        targetAngle = new Vector3(localAngle.x, localAngle.y, (localAngle.z + angle)%360);
        rotating = true;
    }

    private void RotateCenterPiece()
    {
        localAngle = pieceToRotate.localRotation.eulerAngles;
        Quaternion eulerLocal = Quaternion.Euler(localAngle);
        Quaternion eulerTarget = Quaternion.Euler(targetAngle);
        pieceToRotate.localRotation = Quaternion.RotateTowards(eulerLocal,
                                                               eulerTarget,
                                                               rotateSpeed * Time.deltaTime);
        if (Mathf.Abs(Mathf.Abs(eulerLocal.x) - Mathf.Abs(eulerTarget.x)) < 0.01 &&
            Mathf.Abs(Mathf.Abs(eulerLocal.y) - Mathf.Abs(eulerTarget.y)) < 0.01 &&
            Mathf.Abs(Mathf.Abs(eulerLocal.z) - Mathf.Abs(eulerTarget.z)) < 0.01)
        {
            rotating = false;
            groupPieces.FinishGAR(rotatingPieces);
        }
    }


}
