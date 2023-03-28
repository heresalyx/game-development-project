using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicStartpoint : LogicNode
{
    public LogicStartpoint(LogicNode parentNode) : base(parentNode) { }

    private void Start()
    {
        parentNode.Check();
    }

    public override void Interact() 
    {
        parentNode.Check();

        if (currentToggle.isOn)
        {
            circuit.color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1);
        }
        else
        {
            circuit.color = new Color(0.1019608f, 0.1019608f, 0.1019608f, 1);
        }
    }

    public override void Check() {}

    public void SetRandomToggle(Toggle toggle)
    {
        if (currentToggle != toggle)
            currentToggle = toggle;
        if (Random.Range(0, 2) == 0)
        {
            currentToggle.isOn = true;
            circuit.color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1);
        }
        else
        {
            currentToggle.isOn = false;
            circuit.color = new Color(0.1019608f, 0.1019608f, 0.1019608f, 1);
        }
    }

    public override bool Shuffle()
    {
        SetRandomToggle(currentToggle);
        return true;
    }
}
