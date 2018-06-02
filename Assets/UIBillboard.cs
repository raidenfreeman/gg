using UnityEngine;
using System.Collections;


public class UIBillboard : MonoBehaviour
{
    [SerializeField]
    private bool LookAtCamera;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Vector3 fixedRelativePosition;
    [SerializeField]
    private Transform parent;
    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (parent == null)
        {
            parent = transform.parent;
        }
        // If no relative position has been set, get the local position
        fixedRelativePosition = fixedRelativePosition != Vector3.zero ? fixedRelativePosition : transform.localPosition;
    }


    void Update()
    {
        // Maintain your position relative to your parent, regardless of the parent's orientation
        transform.position = parent.position + fixedRelativePosition;
        if (LookAtCamera)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}