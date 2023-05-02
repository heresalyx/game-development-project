public class LogicEndpoint : LogicNode
{
    private LogicGenerator m_logicGenerator;
    private bool m_isActive = false;

    // Store reference to Logic Generator.
    public void SetLogicGenerator(LogicGenerator logic)
    {
        m_logicGenerator = logic;
    }

    public override void Interact() { }

    // If any inputs are off, turn off the endpoint.
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

    // Toggle whether the logic is ready to send back data.
    public void SetActive(bool value)
    {
        m_isActive = value;
    }
}
