using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip musicMusic;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void BossFight()
    {
        audioSource.clip = bossMusic;
        audioSource.Play();
    }

    public void NormalPlay()
    {
        audioSource.clip = musicMusic;
        audioSource.Play();
    }
}
