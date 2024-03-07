using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioClip releaseClip;

    public void PressSound()
    {
        audioSource.PlayOneShot(pressClip);
    }
    public void ReleaseSound()
    {
        audioSource.PlayOneShot(releaseClip);
    }
}
