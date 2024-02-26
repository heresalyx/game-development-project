using UnityEngine;

public class LogicNOTGate : LogicNode
{
    public override void Interact() { }

    // If any inputs are on, turn off the toggle.
    public override void Check()
    {
        bool isTrue = true;

        foreach (LogicNode toggle in m_inputs)
        {
            if (toggle.IsOn())
                isTrue = false;
        }

        if (m_toggle.isOn != isTrue)
        {
            m_toggle.isOn = isTrue;
            if (m_toggle.isOn)
            {
                m_circuit.color = new Color(0.902f, 0.902f, 0.902f, 1);
            }
            else
            {
                m_circuit.color = new Color(0.384f, 0.384f, 0.384f, 1);
            }

            m_parentNode.Check();
        }
    }
}
