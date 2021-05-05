using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudioObject : MonoBehaviour
{

    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>(); ;
        Destroy(gameObject, source.clip.length);
    }
}
