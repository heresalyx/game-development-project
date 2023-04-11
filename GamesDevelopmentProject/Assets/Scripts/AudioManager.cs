using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip m_mainMenuMusic;
    public AudioClip m_backgroundGameMusic;
    public AudioClip m_deathScreenMusic;
    public AudioSource m_musicSource;
    public AudioSource m_effectSource;

    public void PlayMainMenuScreenMusic()
    {
        m_musicSource.clip = m_mainMenuMusic;
        m_musicSource.Play();
    }

    public void PlayBackgroundGameMusic()
    {
        m_musicSource.clip = m_backgroundGameMusic;
        m_musicSource.Play();
    }

    public void PlayDeathScreenMusic()
    {
        m_musicSource.Stop();
        m_musicSource.PlayOneShot(m_deathScreenMusic);
    }

    public void StopAllMusic()
    {
        m_musicSource.Stop();
    }

    public void PlayJumpscareClip()
    {
        m_effectSource.Play();
    }
}
