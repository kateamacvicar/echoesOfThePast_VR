using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showColliders : MonoBehaviour
{
    private Vector3 colliderCenter;
    private float colliderRadius;
    private void OnDrawGizmos()
    {
        colliderCenter = transform.TransformPoint(transform.GetComponent<SphereCollider>().center);
        colliderRadius = transform.GetComponent<SphereCollider>().radius;
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(colliderCenter, colliderRadius);
    }
}
