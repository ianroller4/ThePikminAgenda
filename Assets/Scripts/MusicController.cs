using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip musicMusic;

    private AudioSource audioSource;

    private bool wasPlayingNormal = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!wasPlayingNormal)
            {
                audioSource.clip = musicMusic;
                audioSource.Play();
                wasPlayingNormal = true;
            }
            else
            {
                audioSource.clip = bossMusic;
                audioSource.Play();
                wasPlayingNormal = false;
            }
        }
    }
}
