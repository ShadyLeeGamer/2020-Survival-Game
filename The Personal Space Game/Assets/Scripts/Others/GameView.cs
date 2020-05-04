﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    public float smoothness = 0.15f;

    public Transform target;

    public Vector3 offset;
    Vector3 velocity;

    Camera gameView;

    void Start()
    {
        velocity = Vector3.zero;
        gameView = GetComponent<Camera>();

    }
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = gameView.WorldToViewportPoint(target.position);
            Vector3 delta = new Vector3(target.position.x, 0, 0) + offset - gameView.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothness);
        }

    }
}
