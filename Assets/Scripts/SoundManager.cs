using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    public float musicVolume;
    [SerializeField]
    public float effectsVolume;
    [SerializeField]
    private AudioSource _musicSource, _effectsSource;
    [SerializeField]
    private AudioClip _clickSound;

    void Awake()
    {
        // Check, if we do not have any instance yet.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // Destroy 'this' object as there exist another instance
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        SoundManager.Instance.ChangeMusicVolume(musicVolume);
        SoundManager.Instance.ChangeEffectsVolume(effectsVolume);
    }

    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    public void Alarm()
    {
        _effectsSource.Play();
    }

    public void StopAlarm()
    {
        _effectsSource?.Stop();
    }

    public void ButtonClick()
    {
        PlaySound(_clickSound);
    }

    public void ChangeMusicVolume(float val)
    {
        _musicSource.volume = val;
        musicVolume = val;
    }

    public void ChangeEffectsVolume(float val)
    {
        _effectsSource.volume = val;
        effectsVolume = val;
    }
}
