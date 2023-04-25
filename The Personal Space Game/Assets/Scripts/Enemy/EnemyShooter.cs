using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float fireRate;
    public float recoil;
    float nextTimeToFire;

    public Transform firePoint;

    public GameObject projectile;

    public EnemyMovement enemy;

    public void Shoot()
    {
        if (enemy.target.position.y <= transform.position.y + enemy.jumpPos)
        {
            if (Time.time >= nextTimeToFire)
            {
                firePoint.localEulerAngles = new Vector3(0, firePoint.rotation.y, Random.Range(-recoil, recoil));
                Instantiate(projectile, firePoint.position, firePoint.rotation);

                if (enemy.targetPriority)
                    enemy.animator.SetBool("Sneezing", true);
                else
                    enemy.animator.SetBool("Coughing", true);

                nextTimeToFire = Time.time + 1f / fireRate;
            }
            else if (Time.time < nextTimeToFire)
            {
                if (enemy.targetPriority)
                    enemy.animator.SetBool("Sneezing", false);
                else
                    enemy.animator.SetBool("Coughing", false);
            }
        }
    }
}
