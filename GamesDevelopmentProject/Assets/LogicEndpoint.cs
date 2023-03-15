using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicEndpoint : LogicNode
{
    public LogicGenerator logicGenerator;
    public bool isActive = false;

    public LogicEndpoint(LogicNode parentNode) : base(parentNode) { }

    public void SetLogicGenerator(LogicGenerator logic)
    {
        logicGenerator = logic;
    }

    public override void Interact() { }

    public override void Check()
    {
        bool isTrue = true;

        foreach (LogicNode input in inputs)
        {
            if (!input.GetToggleActive())
                isTrue = false;
        }

        currentToggle.isOn = isTrue;
    }

    public void CheckToggle()
    {
        if (currentToggle.isOn && isActive)
        {
            logicGenerator.LogicComplete();
        }
        if (!currentToggle.isOn && isActive)
        {
            StartCoroutine(logicGenerator.LogicInterupted(0));
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }
}
