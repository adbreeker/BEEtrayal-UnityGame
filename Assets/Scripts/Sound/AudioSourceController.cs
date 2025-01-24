using System.Collections;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    public void PlayAndDestroy(AudioClip sound)
    {
        DontDestroyOnLoad(gameObject);
        _audioSource.PlayOneShot(sound);
        StartCoroutine(DestroyAfterPlaying());
    }

    public void SetMute(bool mute)
    {
        _audioSource.mute = mute;
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public void SetPitch(float pitch)
    {
        _audioSource.pitch = pitch;
    }

    IEnumerator DestroyAfterPlaying()
    {
        while(_audioSource.isPlaying)
        {
            yield return null;
        }
        SoundManager.soundManager.DestroyAudioSource(this);
    }
}
