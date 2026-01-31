using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(JoystickController))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [Header("Camera Movement Multiply")]
    public float cameraMovementMultiply = 1.5f;

    #region Events
    public static event Action OnChangeToFirstPersonView;
    public static event Action OnChangeToTopDownView;
    public static Action OnChangeToMomView;
    #endregion

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void OnEnable()
    {
        JoystickController.OnSouthButtonPressed += HandleSouthButtonPressed;
    }
    void OnDisable()
    {
        JoystickController.OnSouthButtonPressed -= HandleSouthButtonPressed;
    }
    void HandleSouthButtonPressed(int joystickIndex)
    {
        // Toggle between views on button press
        if (joystickIndex == 0) 

            OnChangeToFirstPersonView?.Invoke();
        if (joystickIndex == 1) 

            OnChangeToTopDownView?.Invoke();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
