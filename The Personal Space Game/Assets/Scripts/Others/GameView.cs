using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    /*public float smoothness;

    public Transform target;

    public Vector3 offset;

    void Update()
    {
        if (target != null)
        {
            Vector3 targetedPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position,
                                                      targetedPosition, smoothness);

            transform.position = new Vector3(smoothedPosition.x,
                                             smoothedPosition.y, targetedPosition.z);
        }
    }*/
    public float smoothness = 0.15f;

    public Transform target;

    public Vector3 offset;
    Vector3 velocity;

    Camera camera;

    // Update is called once per frame

    void Start()
    {
        velocity = Vector3.zero;
        camera = GetComponent<Camera>();

    }
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = camera.WorldToViewportPoint(target.position);
            Vector3 delta = new Vector3(target.position.x, 0, 0) + offset - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothness);
        }

    }
}
