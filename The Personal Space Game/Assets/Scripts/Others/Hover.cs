using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    int direction = 1;

    public float speed;
    public float topPoint;
    public float bottomPoint;

    void Update()
    {
        if (transform.position.y > topPoint)
            direction = -1;
        else if (transform.position.y < bottomPoint)
            direction = 1;

        if (transform.position.y < topPoint && transform.position.y > bottomPoint)
            speed = 0.5f;
        else
            speed = 1.5f;

        Vector3 movement = Vector3.up * direction * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
