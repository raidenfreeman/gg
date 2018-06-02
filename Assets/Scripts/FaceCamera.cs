//This script is used to make UI elements face the player's camera

using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Transform mainCamera;   //The camera's transform


    void Start()
    {
        //Set the Main Camera as the target
        mainCamera = Camera.main.transform;
    }

    //Update after all other updates have run
    void LateUpdate()
    {
        if (mainCamera == null)
            return;

        //Apply the rotation needed to look at the camera. Note, since pointing a UI text element
        //at the camera makes it appear backwards, we are actually pointing this object
        //directly *away* from the camera.
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
    }
}