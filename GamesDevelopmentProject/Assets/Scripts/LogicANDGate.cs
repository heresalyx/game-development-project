using UnityEngine;

public class LogicANDGate : LogicNode
{
    public override void Interact(){}

    // If any inputs are off, turn off this toggle.
    public override void Check()
    {
        bool isTrue = true;

        foreach (LogicNode toggle in m_inputs)
        {
            if (!toggle.IsOn())
                isTrue = false;
        }

        if (m_toggle.isOn != isTrue)
        {
            m_toggle.isOn = isTrue;
            if (m_toggle.isOn)
            {
                m_circuit.color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1);
            }
            else
            {
                m_circuit.color = new Color(0.1019608f, 0.1019608f, 0.1019608f, 1);
            }

            m_parentNode.Check();
        }
    }
}
