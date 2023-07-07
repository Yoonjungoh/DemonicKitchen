using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_SoundController : MonoBehaviour
{
    public AudioMixer _audioMixer;

    public Slider bgmSlider;
    public Slider effectSlider;
    float _bgmValue;
    float _effectValue;
    static bool _isInit = false;
    void Start()
    {
        if (!_isInit)
        {
            _isInit = true;
            PlayerPrefs.SetFloat("BgmValue", 1f);
            PlayerPrefs.SetFloat("EffectValue", 1f);
            PlayerPrefs.Save();
        }
    }
    public void SetBgmVolume()
    {
        _audioMixer.SetFloat("BgmParameter", Mathf.Log10(bgmSlider.value) * 20);
        PlayerPrefs.SetFloat("BgmValue", bgmSlider.value);
        PlayerPrefs.Save();
    }
    public void SetEffectVolume()
    {
        _audioMixer.SetFloat("EffectParameter", Mathf.Log10(effectSlider.value) * 20);
        PlayerPrefs.SetFloat("EffectValue", effectSlider.value);
        PlayerPrefs.Save();
    }
    public void SetAll()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BgmValue");
        effectSlider.value = PlayerPrefs.GetFloat("EffectValue");
    }
}
