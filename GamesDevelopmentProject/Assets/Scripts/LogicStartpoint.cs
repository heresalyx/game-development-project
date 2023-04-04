using UnityEngine;

public class LogicStartpoint : LogicNode
{
    public AudioClip m_logicOnEffect;
    public AudioClip m_logicOffEffect;
    public AudioSource m_effectSource;

    private void Start()
    {
        Shuffle();
        m_parentNode.Check();
    }

    // Called from Unity Events.
    public override void Interact() 
    {
        m_parentNode.Check();

        if (m_toggle.isOn)
        {
            m_circuit.color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1);
            PlayLogicSwitchEffect(true);
        }
        else
        {
            m_circuit.color = new Color(0.1019608f, 0.1019608f, 0.1019608f, 1);
            PlayLogicSwitchEffect(false);
        }
    }

    public override void Check() {}

    // Randomly set whether the toggle is on or off.
    public override bool Shuffle()
    {
        if (Random.Range(0, 2) == 0)
        {
            m_toggle.isOn = true;
            m_circuit.color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1);
        }
        else
        {
            m_toggle.isOn = false;
            m_circuit.color = new Color(0.1019608f, 0.1019608f, 0.1019608f, 1);
        }
        return true;
    }

    public void PlayLogicSwitchEffect(bool isOn)
    {
        if (isOn)
            m_effectSource.PlayOneShot(m_logicOnEffect);
        else
            m_effectSource.PlayOneShot(m_logicOffEffect);
    }
}
