using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip m_mainMenuMusic;
    public AudioClip m_backgroundGameMusic;
    public AudioClip m_deathScreenMusic;
    public AudioSource m_musicSource;
    public AudioSource m_effectSource;

    // Play the background music for the menus.
    public void PlayMainMenuScreenMusic()
    {
        m_musicSource.clip = m_mainMenuMusic;
        m_musicSource.Play();
    }

    // Play the background music for the game.
    public void PlayBackgroundGameMusic()
    {
        m_musicSource.clip = m_backgroundGameMusic;
        m_musicSource.Play();
    }

    // Play the background music for the death screen.
    public void PlayDeathScreenMusic()
    {
        m_musicSource.Stop();
        m_musicSource.PlayOneShot(m_deathScreenMusic);
    }

    // Stop all background music.
    public void StopAllMusic()
    {
        m_musicSource.Stop();
    }

    // Play jumpscare effect.
    public void PlayJumpscareClip()
    {
        m_effectSource.Play();
    }
}
