using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource _audioExplosionSource;
    [SerializeField] private AudioClip _explosionSoundClip;


    void Start()
    {
        _audioExplosionSource = GetComponent<AudioSource>();

        if (_audioExplosionSource == null)
        {
            Debug.LogError("the audiosource on explosion is null");
        }

        else
        {
            _audioExplosionSource.clip = _explosionSoundClip;
        }

        _audioExplosionSource.Play();

        Destroy(this.gameObject, 3.0f);
    }

}
