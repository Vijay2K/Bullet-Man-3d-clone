using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    private const float waypointGizmosRadius = 0.25f;

    private void OnDrawGizmos()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmosRadius);

            Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
        }
    }

    public int GetNextIndex(int i)
    {
        if(i + 1 == transform.childCount)
        {
            return 0;
        }

        return i + 1;
    }

    public Vector3 GetWayPoint(int i)
    {
        return transform.GetChild(i).position;
    }
}