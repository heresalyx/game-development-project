using UnityEngine;

public class LogicStartpoint : LogicNode
{
    private void Start()
    {
        Shuffle();
        parentNode.Check();
    }

    // Called from Unity Events.
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

    // Randomly set whether the toggle is on or off.
    public override bool Shuffle()
    {
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
        return true;
    }
}
