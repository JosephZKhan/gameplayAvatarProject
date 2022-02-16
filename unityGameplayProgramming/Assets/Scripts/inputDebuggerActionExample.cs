using UnityEngine;
using UnityEngine.InputSystem;

public class inputDebuggerActionExample : MonoBehaviour
{
    public InputAction exampleAction;
    
    void OnEnable()
    {
       exampleAction.Enable();
    }
}