using DG.Tweening;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    ///We need the following hierarchy:
    ///Root with the script, at the target
    ///Child "zoom pivot" that has an offset Y and Z, so that the camera, is at 0,0,0
    ///Target, child of "zoom pivot" that is where the camera points at
    ///"Rotation pivot" child of "zoom pivot" that ensures that the camera, has 0 rotation at X (even if the camera is rotated, this moves it forward/sideways/etc as if it wasn't)
    ///The camera itself, that can be rotated as we want, on the X axis.


    public const float RotationIncrement = 45f;
    public const float RotationDuration = 1f;
    public const float ZoomIncrement = 2f;
    public const float ZoomDuration = 0.5f;
    public const float MovementSpeed = 0.3f;
    public const int MaxCameraZoomIn = 3;
    public const int MaxCameraZoomOut = -3;


    public Transform targetTransform;
    public Transform cameraTransform;

    public bool canMove;
    public bool canRotate;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            this.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * MovementSpeed, 0, Input.GetAxis("Vertical") * MovementSpeed));
        }

        //Zoom(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            Zoom(1);
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            Zoom(-1);
        }
        if (canRotate)
        {
            if (Input.GetKeyUp(KeyCode.Comma))
            {
                RotateCamera(Rotation.Clockwise);
            }
            if (Input.GetKeyUp(KeyCode.Period))
            {
                RotateCamera(Rotation.Anticlockwise);
            }
        }
    }

    private int CameraZoomLevel = 0;
    private bool isZooming = false;

    private void Zoom(int sign)
    {
        if (isZooming)
        {
            return;
        }
        if (CameraZoomLevel + sign < MaxCameraZoomOut || CameraZoomLevel + sign > MaxCameraZoomIn)
        {
            return;
        }
        isZooming = true;
        CameraZoomLevel += sign;
        var targetPosition = targetTransform.localPosition * 0.25f * CameraZoomLevel;
        cameraTransform.DOLocalMove(targetPosition, ZoomDuration).OnComplete(() => isZooming = false);
    }

    private bool isRotating = false;

    private void RotateCamera(Rotation rotation)
    {
        if (isRotating)
        {
            return;
        }
        isRotating = true;
        short sign = 1;
        if (rotation == Rotation.Anticlockwise)
        {
            sign = -1;
        }
        var targetRotation = transform.localEulerAngles;
        targetRotation.y += sign * RotationIncrement;
        transform.DOLocalRotate(targetRotation, RotationDuration).SetEase(Ease.InOutCirc).OnComplete(() => isRotating = false);
    }

    private enum Rotation { Clockwise, Anticlockwise };
}

