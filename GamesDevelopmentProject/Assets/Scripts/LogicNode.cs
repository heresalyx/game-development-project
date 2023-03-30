using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

abstract public class LogicNode : MonoBehaviour
{
    public Toggle m_toggle;
    public UILineRenderer m_circuit;
    public LogicNode m_parentNode;
    public List<LogicNode> m_inputs;

    abstract public void Interact();

    abstract public void Check();

    // Recursively call Shuffle().
    public virtual bool Shuffle()
    {
        foreach (LogicNode input in m_inputs)
        {
            input.Shuffle();
        }
        return true;
    }

    public void AddInput(LogicNode input)
    {
        m_inputs.Add(input);
    }

    public bool IsOn()
    {
        return m_toggle.isOn;
    }

    // Set the path that the circuit needs to follow.
    public void SetCircuit(int height, int level)
    {
        if (height == -1)
            m_circuit.Points = new Vector2[] { new Vector2(37, 0), new Vector2(213, 0) };
        else if (height == 0)
            m_circuit.Points = new Vector2[] { new Vector2(37, 0), new Vector2(125, 0), new Vector2(125, (-50 * Mathf.Pow(2, level -1)) + 15), new Vector2(213, (-50 * Mathf.Pow(2, level - 1)) + 15) };
        else
            m_circuit.Points = new Vector2[] { new Vector2(37, 0), new Vector2(125, 0), new Vector2(125, (50 * Mathf.Pow(2, level - 1)) - 15), new Vector2(213, (50 * Mathf.Pow(2, level - 1)) - 15) };
    }

    public void SetParentNode(LogicNode parent)
    {
        m_parentNode = parent;
    }
}
