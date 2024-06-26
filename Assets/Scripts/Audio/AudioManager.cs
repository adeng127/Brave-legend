using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [Header("�㲥")]
    public FloatEventSO syncVolumeEvent;
    [Header("�¼�����")]
    public PlayAudioEventSO BGMAudioEvent;
    public PlayAudioEventSO FXAudioEvent;
    public FloatEventSO VolumeEvent;
    public VoidEventSO PauseEvent;
    [Header("���")]
    public AudioSource BGMSource;
    public AudioSource FXSource;

    public AudioMixer mixer;
    private void OnEnable()
    {
        FXAudioEvent.OnEventRaised += OnFXEvent;
        BGMAudioEvent.OnEventRaised += OnBGMEvent;
        VolumeEvent.OnEventRaise += OnVolumeEvent;
        PauseEvent.OnEventRaise += OnPauseEvent;
        
    }
    private void OnDisable()
    {
        FXAudioEvent.OnEventRaised -= OnFXEvent;
        BGMAudioEvent.OnEventRaised -= OnBGMEvent;
        VolumeEvent.OnEventRaise -= OnVolumeEvent;
        PauseEvent.OnEventRaise -= OnPauseEvent;
    }

    private void OnPauseEvent()
    {
        float amount;
        mixer.GetFloat("MasterVolume",out amount);
         syncVolumeEvent.RaiseEvent(amount);
    }

    private void OnVolumeEvent(float amount)
    {
        mixer.SetFloat("MasterVolume", amount*100-80);
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
}
