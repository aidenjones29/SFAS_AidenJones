using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static void playSoundEffect(AudioClip sound)
    {
        GameObject player = GameObject.Find("PlayerBody");
        AudioSource audioSource = player.GetComponent<AudioSource>();
        audioSource.PlayOneShot(sound);
    }
}
