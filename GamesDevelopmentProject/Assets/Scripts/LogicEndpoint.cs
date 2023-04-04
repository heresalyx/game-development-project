using UnityEngine;

public class LogicEndpoint : LogicNode
{
    private LogicGenerator m_logicGenerator;
    private bool m_isActive = false;

    public void SetLogicGenerator(LogicGenerator logic)
    {
        m_logicGenerator = logic;
    }

    public override void Interact() { }

    public override void Check()
    {
        bool isTrue = true;

        foreach (LogicNode input in m_inputs)
        {
            if (!input.IsOn())
                isTrue = false;
        }

        m_toggle.isOn = isTrue;
    }

    // Invoked from Unity Events.
    public void CheckToggle()
    {
        if (m_isActive)
        {
            if (m_toggle.isOn)
            {
                m_logicGenerator.SetLogicComplete();
            }
            if (!m_toggle.isOn)
            {
                StartCoroutine(m_logicGenerator.InteruptLogic(0));
            }
        }
    }

    public void SetActive(bool value)
    {
        m_isActive = value;
    }
}
