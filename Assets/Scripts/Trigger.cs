using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent walkedThrough;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            walkedThrough.Invoke();
        }
    }
}
