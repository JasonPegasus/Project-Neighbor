using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborSounds : MonoBehaviour
{
    private AudioSource sd;
    public AudioClip surprise;
    public AudioClip hm1;
    public AudioClip hm2;
    void Start()
    {
        sd = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        sd.clip = sound;
        sd.Play();
    }
}
