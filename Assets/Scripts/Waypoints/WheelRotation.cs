using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [SerializeField] private Transform[] wheels;
    [SerializeField] private float wheelRadius = 0.35f;

    private Vector3 lastPosition;

    void Start() => lastPosition = transform.position;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        float degrees = (distance / (2f * Mathf.PI * wheelRadius)) * 360f;
        foreach (Transform wheel in wheels)
        {
            Renderer r = wheel.GetComponent<Renderer>();
            Vector3 center = r != null ? r.bounds.center : wheel.position;
            wheel.RotateAround(center, wheel.up, degrees);
        }
    }
}