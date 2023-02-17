using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicXORGate : LogicNode
{
    public LogicXORGate(LogicNode parentNode) : base(parentNode) { }

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
            currentToggle.isOn = true;
        else
            currentToggle.isOn = false;
        //gameObject.GetComponent<Image>().enabled = isTrue;

        parentNode.Check();
    }
}
