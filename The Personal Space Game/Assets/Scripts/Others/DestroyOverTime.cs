using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float delay;

    void Update()
    {
        delay -= 1 * Time.deltaTime;

        if (delay <= 0)
            Destroy(gameObject);
    }
}
