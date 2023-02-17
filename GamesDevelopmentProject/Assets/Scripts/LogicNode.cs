using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class LogicNode : MonoBehaviour
{
    public LogicNode parentNode;
    public Toggle currentToggle;
    public List<Toggle> inputs;

    public LogicNode(LogicNode parentNode)
    {
        this.parentNode = parentNode;
        currentToggle = gameObject.GetComponent<Toggle>();
    }

    abstract public void Interact();

    abstract public void Check();

    public void AddInput(Toggle input)
    {
        inputs.Add(input);
    }
}
