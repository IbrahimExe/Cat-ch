using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool IsPausePressed()
    {
        Keyboard keyboard = Keyboard.current;
        return keyboard != null && keyboard.escapeKey.wasPressedThisFrame;
    }

    public bool IsConfirmPressed()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return false;

        return keyboard.enterKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return Vector3.zero;

        Vector3 mouseScreenPosition = mouse.position.ReadValue();
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    public bool IsAnyDirectionalInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return false;

        return keyboard.wKey.wasPressedThisFrame ||
               keyboard.aKey.wasPressedThisFrame ||
               keyboard.sKey.wasPressedThisFrame ||
               keyboard.dKey.wasPressedThisFrame ||
               keyboard.upArrowKey.wasPressedThisFrame ||
               keyboard.downArrowKey.wasPressedThisFrame ||
               keyboard.leftArrowKey.wasPressedThisFrame ||
               keyboard.rightArrowKey.wasPressedThisFrame;
    }
}