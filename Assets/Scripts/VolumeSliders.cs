using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField]
    private Slider _musicVolumeSlider;
    [SerializeField]
    private Slider _effectsVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        _musicVolumeSlider.value = SoundManager.Instance.musicVolume;
        _effectsVolumeSlider.value = SoundManager.Instance.effectsVolume;
        _musicVolumeSlider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        _effectsVolumeSlider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectsVolume(val));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
