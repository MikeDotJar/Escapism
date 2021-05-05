using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    // Awake is called before Start
    void Awake()
    {
        // if-else allows AudioManager to persist between scenes
        // (audio doesn't get cut off when scene changes)

        // if no existing AudioManager for scene, use this object
        if (instance == null)
            instance = this;

        // if AudioManager already exists, delete duplicate
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // game soundtrack/music is played from here
    void Start()
    {
        //Play("Test");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: \'" + name + "\' not found");
            return;
        }

        s.source.Play();
    }
}
