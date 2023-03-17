using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AntiVirus : MonoBehaviour
{
    public LogicGenerator logicGenerator;
    public List<Key> keys = new List<Key> {Key.UpArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow};

    public int currentDifficulty;

    // Start is called before the first frame update
    public void Activate(int difficulty)
    {
        currentDifficulty = difficulty;
    }

    public IEnumerator SendPrompt()
    {
        yield return new WaitForSecondsRealtime((Random.value * 50) / currentDifficulty);
        //Create Prompt
        yield return new WaitForSecondsRealtime(3);
        StartCoroutine(SendPrompt());
    }

    public void CreatePrompt()
    {
        //Instantiate Prompt Prefab.
        //While (progress < 100)
            //progress += increase 
            //if (button press)
                //break
        //if (progress >= 100)
            //Create Sequence Puzzle
        //Delete Prefab
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard.current[Key.Space].wasPressedThisFrame;
    }
}
