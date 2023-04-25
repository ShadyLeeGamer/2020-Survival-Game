using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public AudioClip[] SFX;

    void Start()
    {
        GetComponent<AudioSource>().clip = SFX[Random.Range(0, SFX.Length)];
        GetComponent<AudioSource>().Play();
    }
}
