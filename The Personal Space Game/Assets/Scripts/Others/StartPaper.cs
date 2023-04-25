using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPaper : MonoBehaviour
{
    bool stacked;

    public Rigidbody2D rb;

    PaperCollection paperCollection;

    void Start()
    {
        paperCollection = FindObjectOfType<PaperCollection>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.y > 0 && !stacked)
        {
            paperCollection.currentPaper++;
            stacked = true;
        }
    }
}