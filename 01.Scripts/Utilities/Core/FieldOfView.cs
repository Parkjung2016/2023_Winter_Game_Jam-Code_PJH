using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;


    [Range(0, 360)] public float viewAngle;

    public LayerMask targetMask, obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();



    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.1f));
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return null;
            FindVisibleTargets();
        }
    }


    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target.GetComponentInParent<Transform>());
                }
            }
        }

        for (int i = 0; i < visibleTargets.Count; i++)
        {
            if (visibleTargets[i] == null  ) visibleTargets.RemoveAt(i);
        }

        SortVisibleTargets();
    }

    void SortVisibleTargets()
    {
        visibleTargets= visibleTargets.OrderBy(x => Vector3.Distance(x.position, transform.position)).ToList();
    }

    private Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0,
            Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        Gizmos.color = Color.red;
        foreach (Transform visible in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, visible.transform.position);
        }
    }
}