using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip m_mainMenuMusic;
    public AudioClip m_whiteNoiseEffect;
    public AudioClip m_deathScreenMusic;
    public AudioClip m_jumpscareEffect;
    public AudioSource m_effectSource;

    public void PlayMainMenuScreenMusic()
    {
        m_effectSource.clip = m_mainMenuMusic;
        m_effectSource.Play();
    }

    public void PlayDeathScreenMusic()
    {
        m_effectSource.clip = m_deathScreenMusic;
        m_effectSource.Play();
    }

    public void StopAllMusic()
    {
        m_effectSource.Stop();
    }

    public void PlayJumpscareClip()
    {
        m_effectSource.PlayOneShot(m_jumpscareEffect);
    }
}
