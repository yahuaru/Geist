using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public float upperBound = 0.4f;
    public float lowerBound = -0.4f;
    public float leftBound = -0.16f;
    public float rightBound = -0.16f;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 point = camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            destination.x = Mathf.Clamp(destination.x, leftBound, rightBound);
            destination.y = Mathf.Clamp(destination.y, lowerBound, upperBound);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

    }
}