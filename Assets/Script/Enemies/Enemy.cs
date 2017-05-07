using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, iEnemy
{
    [SerializeField]
    private List<Vector3> Waypoints;

    public void Walk()
    {

    }


    public void Look()
    {

    }
    void OnDrawGizmos()
    {

        Vector3 pos = transform.position;
        foreach(Vector3 waypoint in Waypoints)
        {
            Gizmos.DrawLine(pos, waypoint);
            pos = waypoint;
        }
    }


}
