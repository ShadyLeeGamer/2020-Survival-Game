using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    static MenuMusic instance;

    AudioSource audio;
    public AudioClip[] music;

    void Awake()
    {
        audio = GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("MENU") && audio.clip != music[0])
        {
            audio.clip = music[0];
            audio.Play();
        }
    }

    public void PlayGameMusic()
    {
        audio.Pause();
        audio.clip = music[1];
        audio.Play();
    }
}