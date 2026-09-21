using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public float FollowSpeed = 2f;
    public float OffsetY = 1f;
    public Transform Target;

    void LateUpdate()
    {
        Vector3 newPosition = new(Target.position.x, Target.position.y + OffsetY, -1f);
        transform.position = Vector3.Lerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}
