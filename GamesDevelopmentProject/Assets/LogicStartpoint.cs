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
}
