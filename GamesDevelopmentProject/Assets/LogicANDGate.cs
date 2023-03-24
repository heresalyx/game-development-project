using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicANDGate : LogicNode
{
    public LogicANDGate(LogicNode parentNode) : base (parentNode){}

    public override void Interact(){}

    public override void Check()
    {
        bool isTrue = true;

        foreach (LogicNode toggle in inputs)
        {
            if (!toggle.GetToggleActive())
                isTrue = false;
        }

        currentToggle.isOn = isTrue;
        if (isTrue)
        {
            Debug.Log("Color is " + circuit.color);
            circuit.color = new Color(231, 231, 231);
        }
        else
        {
            Debug.Log("Color is " + circuit.color);
            circuit.color = new Color(26, 26, 26);
        }
        //gameObject.GetComponent<Image>().enabled = isTrue;

        parentNode.Check();
    }
}
