using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        foreach (Transform waypoint in transform)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(waypoint.position, 0.5f);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null) return transform.GetChild(0);

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            return currentWaypoint;
        }
    }

    public float GetSpeed(Transform waypoint)
    {
        WaypointData data = waypoint.GetComponent<WaypointData>();
        return data != null ? data.speed : 5f; // valor por defecto si no tiene componente
    }

    public bool IsStopPoint(Transform waypoint)
    {
        WaypointData data = waypoint.GetComponent<WaypointData>();
        return data != null && data.isStopPoint;
    }
}