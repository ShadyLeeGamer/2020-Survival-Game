using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewItem : MonoBehaviour
{
    bool shoot;
    bool shooting;

    public int type;
    public int intensity;

    public float recoil;
    public float fireRate;
    float nextTimeToFire;
    float nextTimeToAnim;

    public GameObject projectile;

    public Transform firePoint;

    public Image soapImg;

    public Database database;

    void Start()
    {
        database = FindObjectOfType<Database>();
    }

    void Update()
    {
        if (shoot)
        {
            if (Time.time >= nextTimeToFire)
            {
                for (int i = 0; i < intensity; i++)
                {
                    firePoint.localEulerAngles = new Vector3(0, firePoint.rotation.y, Random.Range(-recoil / 2, recoil));
                    GameObject proj = Instantiate(projectile, firePoint.position, firePoint.rotation);
                }

                nextTimeToFire = Time.time + 1f / fireRate;

                soapImg.sprite = database.soapSprite2[type];
            }
            else if (Time.time < nextTimeToFire && Time.time > nextTimeToAnim)
            {
                nextTimeToAnim = Time.time + 1f / fireRate;

                soapImg.sprite = database.soapSprite1[type];
            }
        }
        else
            soapImg.sprite = database.soapSprite1[type];
    }

    public void Preview()
    {
        shoot = true;
    }

    public void EndPreview()
    {
        shoot = false;
    }
}
