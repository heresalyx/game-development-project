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
    }

    public override void Check() {}

    public void SetRandomToggle(Toggle toggle)
    {
        if (currentToggle != toggle)
            currentToggle = toggle;
        if (Random.Range(0, 2) == 0)
        {
            currentToggle.isOn = true;
            circuit.color = new Color(231,231,231);
        }
        else
        {
            currentToggle.isOn = false;
            circuit.color = new Color(26,26,26);
        }
    }

    public override bool Shuffle()
    {
        SetRandomToggle(currentToggle);
        return true;
    }
}
