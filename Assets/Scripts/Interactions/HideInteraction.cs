using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class HideInteraction : MonoBehaviour
{
    public UnityEvent hideEvents;
    public UnityEvent showEvents;
    bool hiding;
    private bool interactionLocked;
    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Handler") && hiding == false)
        {
            Debug.Log("Player stayed in hiding spot interaction area.");
            if (InteractPressed())
            {
                Debug.Log("Player interacted with the hiding spot.");
                hideEvents.Invoke();
                
                hiding = true;
                StartCoroutine(hidingRoutine());
            }
        }
    }


    private IEnumerator hidingRoutine()
    {
        while (hiding)
        {
            yield return new WaitForSeconds(0);
            if (InteractPressed())
            {
                showEvents.Invoke();
                hiding = false;
            }
        }

    }
    bool InteractPressed()
    {
        if (interactionLocked) return false;
        if (Input.GetKey(KeyCode.E) || JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed())
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
