using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;

    [SerializeField] private float distanceThreshold = 0.1f;
    [SerializeField] private float rotationSpeed = 2f;

    private Transform currentWaypoint;
    private float currentSpeed = 5f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Minimap;

    private Quaternion rotationGoal;
    private Vector3 directionToWaypoint;

    void Start()
    {
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        currentSpeed = waypoints.GetSpeed(currentWaypoint);
        transform.LookAt(currentWaypoint.position);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, currentSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            currentSpeed = waypoints.GetSpeed(currentWaypoint);
        }
        RotateTorwardsWaypoints();
        if (waypoints.IsStopPoint(currentWaypoint)) { 
            player.transform.position = new Vector3(currentWaypoint.position.x+3, currentWaypoint.transform.position.y, currentWaypoint.transform.position.z);
            player.SetActive(true);
            Minimap.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void RotateTorwardsWaypoints()
    {
        directionToWaypoint = (currentWaypoint.position - transform.position).normalized;
        rotationGoal = Quaternion.LookRotation(directionToWaypoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, rotationSpeed * Time.deltaTime);
    }
}