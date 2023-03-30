using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    // Simple script that prevents distruction  of its GameObject when loading scenes.
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
