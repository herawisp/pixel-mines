using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public float FollowSpeed = 2f;
    public float OffsetY = 1f;
    public Transform Target;

    void Update()
    {
        Vector3 newPosition = new(Target.position.x, Target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}
