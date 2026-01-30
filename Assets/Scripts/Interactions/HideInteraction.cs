using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class HideInteraction : MonoBehaviour
{
    public UnityEvent hideEvents;
    public UnityEvent showEvents;
    bool hiding;
    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Handler") && hiding == false)
        {
            Debug.Log("Player stayed in hiding spot interaction area.");
            if (Input.GetKeyDown(KeyCode.E) || JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed())
            {
                HandleObjectsPlayer handleObjectsPlayer = other.GetComponent<HandleObjectsPlayer>();
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
            if (Input.GetKeyDown(KeyCode.E) || JoystickController.Instance != null && JoystickController.Instance.IsSouthButtonPressed())
            {
                showEvents.Invoke();
                hiding = false;
            }
        }

    }
}
