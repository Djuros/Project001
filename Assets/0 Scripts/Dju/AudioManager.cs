using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager ins;
    public AudioSource _as;
    public AudioClip ac;
    private void Awake()
    {
        ins = this;
    }
    public void PlaySound()
    {
        _as.PlayOneShot(ac);
    }
}
