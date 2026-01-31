using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Chaser : MonoBehaviour
{
    [SerializeField] Renderer boundingArea;
   [SerializeField] private Transform target;
    private AudioSource _source;
    public float moveSpeed = 6;
    private bool enraged = false;
    public float catchDistance = 0.5f;
    public UnityEvent catchEvents;
    private void Start()
    {
        
        _source = GetComponent<AudioSource>();
        moveSpeed /= 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!enraged)
        {
            StartCoroutine(RageMode());
        }
       StartCoroutine(GoTarget());
        
    }
    private System.Collections.IEnumerator GoTarget()
    {
        Vector3 targetCorrected = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
       float distance = Vector3.Distance(transform.position, targetCorrected);
        Debug.Log(distance);
        yield return new WaitForSeconds(0);
        transform.position = Vector3.MoveTowards(transform.position, targetCorrected, moveSpeed * 0.020f);
        if(distance < catchDistance)
        {
            catchEvents.Invoke();
        }
    }
   
    private System.Collections.IEnumerator RageMode()
    {
        Vector3 targetCorrected = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
        float distance = Vector3.Distance(transform.position, targetCorrected);
            if (boundingArea.isVisible && distance < 15 || distance > 30)
            {
                enraged = true;
            yield return new WaitForSeconds(1);
            _source.pitch = 2f;
                moveSpeed *= 2;
            }       
    }
}
