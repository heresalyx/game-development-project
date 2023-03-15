using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class LogicNode : MonoBehaviour
{
    public LogicNode parentNode;
    public Toggle currentToggle;
    public List<LogicNode> inputs;

    public LogicNode(LogicNode parentNode)
    {
        this.parentNode = parentNode;
        currentToggle = gameObject.GetComponent<Toggle>();
    }

    abstract public void Interact();

    abstract public void Check();

    public virtual bool Shuffle()
    {
        foreach (LogicNode input in inputs)
        {
            input.Shuffle();
        }
        return true;
    }

    public void AddInput(LogicNode input)
    {
        inputs.Add(input);
    }

    public bool GetToggleActive()
    {
        return currentToggle.isOn;
    }
}
