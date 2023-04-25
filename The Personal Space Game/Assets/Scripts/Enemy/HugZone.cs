using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
            other.GetComponent<PlayerMovement>().isHugged = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
            other.GetComponent<PlayerMovement>().isHugged = false;
    }
}
