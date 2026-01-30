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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count > 0)
        {

            if (Gamepad.all[0].leftShoulder.wasPressedThisFrame)
            {
                OnSouthButtonPressed?.Invoke(0);
            }
            if (Gamepad.all[0].rightShoulder.wasPressedThisFrame)
            {
                OnSouthButtonPressed?.Invoke(1);
            }
        }
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
}
