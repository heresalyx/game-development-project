using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicXNORGate : LogicNode
{
    public LogicXNORGate(LogicNode parentNode) : base(parentNode) { }

    public override void Interact() { }

    public override void Check()
    {
        int noOfTrue = 0;

        foreach (Toggle toggle in inputs)
        {
            if (toggle.isOn)
                noOfTrue++;
        }

        if (noOfTrue == 1)
            currentToggle.isOn = false;
        else
            currentToggle.isOn = true;
        //gameObject.GetComponent<Image>().enabled = isTrue;

        parentNode.Check();
    }
}
