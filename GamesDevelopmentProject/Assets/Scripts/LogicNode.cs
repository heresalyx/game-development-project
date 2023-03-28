using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

abstract public class LogicNode : MonoBehaviour
{
    public LogicNode parentNode;
    public Toggle currentToggle;
    public List<LogicNode> inputs;
    public UILineRenderer circuit;

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

    public void SetCircuit(int height, int level)
    {
        if (height == -1)
            circuit.Points = new Vector2[] { new Vector2(37, 0), new Vector2(213, 0) };
        else if (height == 0)
            circuit.Points = new Vector2[] { new Vector2(37, 0), new Vector2(125, 0), new Vector2(125, (-50 * Mathf.Pow(2, level -1)) + 15), new Vector2(213, (-50 * Mathf.Pow(2, level - 1)) + 15) };
        else
            circuit.Points = new Vector2[] { new Vector2(37, 0), new Vector2(125, 0), new Vector2(125, (50 * Mathf.Pow(2, level - 1)) - 15), new Vector2(213, (50 * Mathf.Pow(2, level - 1)) - 15) };
    }
}
