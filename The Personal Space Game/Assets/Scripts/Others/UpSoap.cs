using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpSoap : MonoBehaviour
{
    SpawnManager spawnManager;

    void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        if (!spawnManager.ready)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Shooter shooter = FindObjectOfType<Shooter>();
            shooter.selectedSoap = 2;
            if (shooter.currentAmmo[4] != shooter.maxAmmo[4])
                shooter.currentAmmo[4] += shooter.maxAmmo[4];
            shooter.upMode = true;
            Destroy(gameObject);
        }
    }
}
