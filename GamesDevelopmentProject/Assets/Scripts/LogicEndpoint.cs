public class LogicEndpoint : LogicNode
{
    public LogicGenerator logicGenerator;
    private bool isActive = false;

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
            if (!input.IsOn())
                isTrue = false;
        }

        currentToggle.isOn = isTrue;
    }

    // Invoked from Unity Events.
    public void CheckToggle()
    {
        if (isActive)
        {
            if (currentToggle.isOn)
            {
                logicGenerator.SetLogicComplete();
            }
            if (!currentToggle.isOn)
            {
                StartCoroutine(logicGenerator.InteruptLogic(0));
            }
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }
}
