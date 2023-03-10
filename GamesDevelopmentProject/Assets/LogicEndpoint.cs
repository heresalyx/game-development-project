using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicEndpoint : LogicNode
{
    public LogicGenerator logicGenerator;

    public LogicEndpoint(LogicNode parentNode) : base(parentNode) { }

    public void SetLogicGenerator(LogicGenerator logic)
    {
        logicGenerator = logic;
    }

    public override void Interact() { }

    public override void Check()
    {
        bool isTrue = true;

        foreach (Toggle toggle in inputs)
        {
            if (!toggle.isOn)
                isTrue = false;
        }

        currentToggle.isOn = isTrue;
    }

    public void CheckToggle()
    {
        if (currentToggle.isOn)
        {
            logicGenerator.LogicComplete();
        }
    }
}
