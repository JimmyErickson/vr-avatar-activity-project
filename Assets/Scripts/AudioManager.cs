using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public GameObject menu;

    private void Awake()
    {
        // Persistent game object allows for seamless music, may want to remove if we want different music for each scene.
        DontDestroyOnLoad(gameObject);
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    private void Start()
    {
        // to play sounds in other scripts use: FindObjectOfType<AudioManager>().Play("");
        string scene = SceneManager.GetActiveScene().name;

        Play("Theme");
        if (scene == "OpeningScene")
        {
            Play("Introduction");
            /*if (pose == menu)
            {
                Play("Menu");
            }*/
            if (menu.activeInHierarchy)
            {
                Play("Menu");
            }
        }
    }
}
