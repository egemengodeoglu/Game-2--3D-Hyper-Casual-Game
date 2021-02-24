using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AudioSettings : MonoBehaviour
{
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    public AudioSource backgroundSource;
    public AudioSource[] soundEffectsSources;
    private float backgroundFloat, soundEffectsFloat;

    void Awake()
    {
        ContinuousSettings();
    }

    private void ContinuousSettings()
    {
        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
        soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);

        backgroundSource.volume = backgroundFloat;
        
        foreach (AudioSource tmpAudio in soundEffectsSources)
        {
            tmpAudio.volume = soundEffectsFloat;
        }

    }
}
