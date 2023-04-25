using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float delay;

    public GameObject EFX;

    void Update()
    {
        delay -= 1 * Time.deltaTime;

        if (delay <= 0)
        {
            Destroy(gameObject);

            if (EFX)
                Instantiate(EFX, transform.position, Quaternion.identity);
        }
    }
}