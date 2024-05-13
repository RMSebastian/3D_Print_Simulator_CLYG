using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundsMaker : WaitableBase
{
    [SerializeField]private AudioClip audioClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
    }
    public override void HandleRequest(UnityAction finalRequest = null)
    {
        audioSource.Play();
        base.HandleRequest(finalRequest);
    }
}
