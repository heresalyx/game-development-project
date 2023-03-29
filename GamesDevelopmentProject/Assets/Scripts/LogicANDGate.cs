using UnityEngine;

public class LogicANDGate : LogicNode
{
    public override void Interact(){}

    // If any inputs are off, turn off this toggle.
    public override void Check()
    {
        bool isTrue = true;

        foreach (LogicNode toggle in inputs)
        {
            if (!toggle.IsOn())
                isTrue = false;
        }

        if (currentToggle.isOn != isTrue)
        {
            currentToggle.isOn = isTrue;
            if (currentToggle.isOn)
            {
                circuit.color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1);
            }
            else
            {
                circuit.color = new Color(0.1019608f, 0.1019608f, 0.1019608f, 1);
            }

            parentNode.Check();
        }
    }
}
