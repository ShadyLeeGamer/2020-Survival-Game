using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float delay;
    float timer = 1;

    public Transform[] startPos;
    public Transform[] quitPos;

    public GameObject enemy;

    void Update()
    {
        timer -= delay * Time.deltaTime;
        
        if (timer <= 0)
        {
            int house = Random.Range(0, startPos.Length);

            GameObject enemy_ = Instantiate(enemy, startPos[house].position, Quaternion.identity) as GameObject;

            enemy_.GetComponent<Enemy>().quitPos = quitPos[house];
            timer = 1;
        }
    }
}
