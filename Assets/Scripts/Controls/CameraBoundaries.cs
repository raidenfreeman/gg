using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{

    public Vector2 topRightBoundary, bottomLeftBoundary;

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        position.x = Mathf.Clamp(position.x, bottomLeftBoundary.x, topRightBoundary.x);
        position.z = Mathf.Clamp(position.z, bottomLeftBoundary.y, topRightBoundary.y);
        transform.position = position;
    }
}
