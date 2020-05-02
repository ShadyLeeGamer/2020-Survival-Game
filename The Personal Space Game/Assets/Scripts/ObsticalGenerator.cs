using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalGenerator : MonoBehaviour
{
    public float distanceBetween;
    public float spawnPointX;
    public float spawnPointY;

    public GameObject obstical;

    public Transform generationPoint;

    void Update()
    {
        if (generationPoint.position.x > transform.position.x)
        {
            //float spawnPointX_ = Random.Range(-spawnPointX, spawnPointX);
            //float spawnPointY_ = Random.Range(-spawnPointY, spawnPointY);

            transform.position = new Vector3(transform.position.x + distanceBetween, transform.position.y
                                                                                   , 4);

            Instantiate(obstical, transform.position, Quaternion.identity);

        }
    }
}
