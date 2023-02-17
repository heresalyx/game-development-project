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

        foreach (Toggle toggle in inputs)
        {
            if (!toggle.isOn)
                isTrue = false;
        }

        currentToggle.isOn = isTrue;
        //gameObject.GetComponent<Image>().enabled = isTrue;

        parentNode.Check();
    }
}
