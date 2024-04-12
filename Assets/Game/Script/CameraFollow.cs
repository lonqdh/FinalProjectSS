using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public LayerMask collisionMask;
    public float obstacleSizeThreshold = 1.0f;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the target position
            Vector3 targetPosition = target.position - transform.forward * distance;

            // Perform raycast to check for obstacles between camera and target
            RaycastHit hit;
            if (Physics.Raycast(target.position, -transform.forward, out hit, maxDistance, collisionMask))
            {
                // Check the size of the collider hit by the raycast
                if (hit.collider.bounds.size.magnitude > obstacleSizeThreshold)
                {
                    // If the collider is larger than the threshold, adjust the target position closer to the player
                    distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
                }
            }
            else
            {
                // If no hit, reset the distance to the maximum
                distance = maxDistance;
            }

            // Smoothly move the camera towards the target position
            Vector3 desiredPosition = target.position - transform.forward * distance;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
    }
}


