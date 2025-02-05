using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 1f; // Smoothness of the camera's movement
    public Vector2 offset; // Offset between the player and the camera

    [Header("Camera Bounds")] //unity has a little title on it now :)
    public Vector2 minBounds; // Minimum x and y values for the camera
    public Vector2 maxBounds; // Maximum x and y values for the camera

    void LateUpdate()
    {
        if (player == null) return; //if theres no player do nothing

        // calculate the target position for the camera
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // Clamp the target position within the defined boundaries
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

        // Smoothly interpolate the camera's position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed); //the closer smooth is to 1, the faster the camera moves
    }
}