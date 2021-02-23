using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private int _nextSong;

    public IEnumerator FadeoutRoutine(AudioSource _audioSource, float FadeTime)
    {
        float startVolume = _audioSource.volume;

        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        _audioSource.Stop();
        _audioSource.volume = startVolume;
        PlayNext(_nextSong);
    }

    public void FadeOut()
    {
        StartCoroutine(FadeoutRoutine(_audioSource, 10f));
    }

    public void PlayNext(int nextSong)
    {
        if (_nextSong < 4)
        {
            _nextSong++;
            _audioSource.clip = audioClip[_nextSong];
            _audioSource.Play();
        }
        else if(_nextSong >= 4)
        {
            _nextSong = Random.Range(0, 5);
            _audioSource.clip = audioClip[_nextSong];
            _audioSource.Play();
        }
    }

}
