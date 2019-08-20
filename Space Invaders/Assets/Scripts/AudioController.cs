using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip _playerShot;
    public AudioClip _enemyShot;
    public AudioClip _playerDeath;
    public AudioClip _enemyDeath;
    public AudioClip _baseDestroyed;
    public AudioClip _playerVictory;
    public AudioClip _gameOverSound;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
}
