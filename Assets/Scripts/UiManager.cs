using System.Collections;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private Animator taskPanelAnimator;
    [SerializeField] private TMP_Text points;
    [SerializeField] private GameObject uiPanel;

    private bool isTaskPanelOpen = false;

    private int maxInteractionsToShowUi = 2;
    private int currentInteractions = 0;

    private int pointsValue = 0;

    void OnEnable()
    {
        GameEvents.OnHandleTrigger += ShowUiPanel;
        GameEvents.OnHandleTriggerRelease += HideUiPanel;
        GameEvents.OnTaskCompleted += UpdatePointsDisplay;
        // GameEvents.OnT += UpdatePointsDiscountDisplay;
    }

    void OnDisable()
    {
        GameEvents.OnHandleTrigger -= ShowUiPanel;
        GameEvents.OnHandleTriggerRelease -= HideUiPanel;
        GameEvents.OnTaskCompleted -= UpdatePointsDisplay;
    }

    void UpdatePointsDisplay(KitchenTask task)
    {
        pointsValue += task.quantity * 10; // Exemplo: 10 pontos por ingrediente na task
        
        points.text = pointsValue + " X";
    }
    void UpdatePointsDiscountDisplay(KitchenTask task)
    {
        pointsValue -= task.quantity * 11; // Exemplo: 10 pontos por ingrediente na task
        
        points.text = pointsValue + " X";
    }
    void ShowUiPanel()
    {
        if (currentInteractions > maxInteractionsToShowUi) return;
        uiPanel.SetActive(true);
        currentInteractions++;

    }
    void HideUiPanel()
    {
        uiPanel.SetActive(false);

    }

    void Update()
    {
        if (InteractPressed())
        {
            ToggleTaskPanel();
        }
    }

    void ToggleTaskPanel()
    {
        isTaskPanelOpen = !isTaskPanelOpen;
        taskPanelAnimator.SetBool("IsOpen", isTaskPanelOpen);
    }

    private bool interactionLocked;
    bool InteractPressed()
    {
        if (interactionLocked) return false;
        if (Input.GetKeyDown(KeyCode.T) || JoystickController.Instance != null && JoystickController.Instance.IsSouthDPadPressed()) // Adicione seu Joystick aqui
        {
            StartCoroutine(InteractionCooldown());
            return true;
        }
        return false;
    }
    IEnumerator InteractionCooldown()
    {
        interactionLocked = true;
        yield return new WaitForSeconds(0.2f);
        interactionLocked = false;
    }



}
