using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundSource;
    public AudioSource[] effectsSources;
    public Slider backgroundSlider, effectsSlider;
    public Dropdown qualityDropdown;

    void Start()
    {
        backgroundSource.volume = GameDataReferences.Instance.backgroundVolume;
        foreach(AudioSource auido in effectsSources)
        {
            auido.volume = GameDataReferences.Instance.backgroundVolume;
        }

        backgroundSlider.value = GameDataReferences.Instance.backgroundVolume;
        effectsSlider.value = GameDataReferences.Instance.effectsVolume;
        qualityDropdown.value = GameDataReferences.Instance.qualityIndex;

        SaveBinary.LoadGameData(GameDataReferences.Instance);
        /*Debug.Log("Data: \nName:"+ GameDataReferences.Instance.playerName+ "\nQualityIndex:"+GameDataReferences.Instance.qualityIndex+
            "\nPlayerIndex:"+GameDataReferences.Instance.carIndex+"\n"+GameDataReferences.Instance.backgroundVolume+"---"+GameDataReferences.Instance.effectsVolume);*/
    }

    public void SaveSoundSettings()
    {
        GameDataReferences.Instance.backgroundVolume = backgroundSlider.value;
        GameDataReferences.Instance.effectsVolume = effectsSlider.value;
        GameDataReferences.Instance.qualityIndex = qualityDropdown.value;
        SaveBinary.SaveGameData(GameDataReferences.Instance);
        
    }

    public void UpdateQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value, true);
    }


    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        backgroundSource.volume = backgroundSlider.value;
        
        foreach (AudioSource tmpAudio in effectsSources)
        {
            tmpAudio.volume = effectsSlider.value;
        }
    }


}
