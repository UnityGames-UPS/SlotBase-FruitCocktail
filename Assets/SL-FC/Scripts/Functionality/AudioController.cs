using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] internal AudioListener m_Player_Listener;
    [SerializeField] internal AudioSource m_BG_Audio;
    [SerializeField] internal AudioSource m_Bonus_BG_Audio;
    [SerializeField] internal AudioSource m_Click_Audio;
    [SerializeField] internal AudioSource m_Win_Audio;
    [SerializeField] internal AudioSource m_LooseAudio;
    [SerializeField] internal AudioSource m_Bonus_Audio;
    [SerializeField] internal AudioSource m_FreeSpin_Audio;
    [SerializeField] internal AudioSource m_Spin_Audio;
    [SerializeField] internal AudioSource m_Bonus_Spin_Audio;
    [SerializeField] internal AudioSource m_Spin_Button_Clicked;

    [SerializeField] internal Slider m_SoundSlider;
    [SerializeField] internal Slider m_MusicSlider;

    private List<AudioSource> allSources;
    private readonly Dictionary<AudioSource, bool> preFocusMuteState = new Dictionary<AudioSource, bool>();
    private bool isForceMuted = false;

    private void Start()
    {
        UpdateSoundVolume(m_SoundSlider.value);
        UpdateMusicVolume(m_MusicSlider.value);

        m_SoundSlider.onValueChanged.AddListener(delegate
        {
            UpdateSoundVolume(m_SoundSlider.value);
        });
        m_MusicSlider.onValueChanged.AddListener(delegate
        {
            UpdateMusicVolume(m_MusicSlider.value);
        });
    }

    private void UpdateSoundVolume(float value)
    {
        m_Click_Audio.volume = value;
        m_Win_Audio.volume = value;
        m_LooseAudio.volume = value;
        m_Bonus_Audio.volume = value;
        m_FreeSpin_Audio.volume = value;
        m_Spin_Audio.volume = value;
        m_Spin_Button_Clicked.volume = value;
    }

    private void UpdateMusicVolume(float value)
    {
        m_BG_Audio.volume = value;
        m_Bonus_BG_Audio.volume = value;
    }

    internal void InitialAudioSetup()
    {
        if (m_BG_Audio) m_BG_Audio.Play();
    }

    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        if (allSources == null)
        {
            allSources = new List<AudioSource> {
                m_BG_Audio, m_Bonus_BG_Audio, m_Click_Audio, m_Win_Audio, m_LooseAudio,
                m_Bonus_Audio, m_FreeSpin_Audio, m_Spin_Audio, m_Bonus_Spin_Audio, m_Spin_Button_Clicked
            };
        }

        foreach (var source in allSources)
        {
            if (source == null) continue;
            if (forceMute)
            {
                preFocusMuteState[source] = source.mute;
                source.mute = true;
            }
            else
            {
                source.mute = preFocusMuteState.TryGetValue(source, out bool prevMuted) ? prevMuted : source.mute;
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        SetMuteAll(!focus);
    }

    internal void ToggleMute(bool toggle, string type = "all")
    {
        switch (type)
        {
            case "bg":
                m_BG_Audio.mute = toggle;
                break;
            case "button":
                m_Click_Audio.mute = toggle;
                m_Spin_Audio.mute = toggle;
                break;
            case "wl":
                m_Win_Audio.mute = toggle;
                m_Bonus_Audio.mute = toggle;
                m_FreeSpin_Audio.mute = toggle;
                break;
            case "all":
                m_BG_Audio.mute = toggle;
                m_Click_Audio.mute = toggle;
                m_Win_Audio.mute = toggle;
                m_Bonus_Audio.mute = toggle;
                m_FreeSpin_Audio.mute = toggle;
                m_Spin_Audio.mute = toggle;
                break;
        }
    }

}