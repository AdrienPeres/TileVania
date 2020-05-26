using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public void SetVolume(float volume)
    {
        if (!GetComponent<AudioSource>().mute)
        {
            GetComponent<AudioSource>().volume = volume;
        }
    }
}
