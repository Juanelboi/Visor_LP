using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform playerTransform;

    public void SetTarget(Transform target)
    {
        playerTransform = target;
    }

    private void LateUpdate()
    {
           Vector3 newPosition = playerTransform.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(0f, playerTransform.eulerAngles.y, 0f);
    }
}
