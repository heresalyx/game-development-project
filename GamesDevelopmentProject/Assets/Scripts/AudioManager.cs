using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip m_mainMenuMusic;
    public AudioClip m_whiteNoiseEffect;
    public AudioClip m_logicOnEffect;
    public AudioClip m_logicOffEffect;
    public AudioClip m_logicCompleteEffect;
    public AudioClip m_AntiVirusPromptEffect;
    public AudioClip m_glitchEffect;
    public AudioClip m_loadingEffect;
    public AudioClip m_deathEffect;
    public AudioClip m_deathScreenEffect;

    public AudioSource m_musicSource;
    public AudioSource m_effectSource;

    public void PlayLogicCompleteEffect()
    {
        m_effectSource.PlayOneShot(m_logicCompleteEffect);
    }
}
