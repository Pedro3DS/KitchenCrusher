using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static JoystickController Instance = null;


    #region Events
    public static event Action<int> OnSouthButtonPressed;
    #endregion
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool HasGamepad()
    {
        return Gamepad.all.Count > 0;
    }

    public Vector2 GetRightStick()
    {
        if (!HasGamepad())
            return Vector2.zero;

        return Gamepad.all[0].rightStick.ReadValue();
    }

    public bool IsSouthButtonPressed()
    {
        if (!HasGamepad())
            return false;

        return Gamepad.all[0].buttonSouth.isPressed;
    }
    public bool IsEastButtonPressed()
    {
        if (!HasGamepad())
            return false;

        return Gamepad.all[0].buttonEast.isPressed;
    }
    public bool IsSouthDPadPressed()
    {
        if (!HasGamepad())
            return false;

        return Gamepad.all[0].dpad.down.isPressed;
    }
    public bool IsWestButtonPressed()
    {
        if (!HasGamepad())
            return false;

        return Gamepad.all[0].buttonWest.isPressed;
    }
}
