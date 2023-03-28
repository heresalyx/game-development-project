using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LogicNOTGate : LogicNode
{
    public LogicNOTGate(LogicNode parentNode) : base(parentNode) { }

    public override void Interact() { }

    public override void Check()
    {
        bool isTrue = true;

        foreach (LogicNode toggle in inputs)
        {
            if (toggle.GetToggleActive())
                isTrue = false;
        }

        currentToggle.isOn = isTrue;
        if (currentToggle.isOn)
        {
            circuit.color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1);
        }
        else
        {
            circuit.color = new Color(0.1019608f, 0.1019608f, 0.1019608f, 1);
        }
        //gameObject.GetComponent<Image>().enabled = isTrue;

        parentNode.Check();
    }
}
