using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Stopper : MonoBehaviour
{
    [SerializeField] Volume filter;
    [SerializeField] TopViewMovement player;
    private AudioSource _source;
    public List<KeyCode> viewedInputs = new List<KeyCode>();
    public UnityEvent activeEvents;
    public UnityEvent inactiveEvent;
    public UnityEvent catchEvents;
    private bool isOn = true;
    private bool isWatching;
    private bool spotted;
    private bool firstTime = true;
    private void OnEnable()
    {
        StartCoroutine(AppearTimer());
    }
    private void Start()
    {      
        _source = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        if (isWatching && !spotted)
        {
            WatchBehaviour();
        }
    }
    // Update is called once per frame
    private System.Collections.IEnumerator AppearTimer()
    {
        while (isOn && !spotted)
        {
            int rand = UnityEngine.Random.Range(7, 14);
            yield return new WaitForSeconds(rand);
            rand = UnityEngine.Random.Range(0, 2);
            Debug.Log(rand);
            if (rand == 0)
            {
            } else
            {
                continue;
            }
                // Debug.Log("I see you");
                activeEvents.Invoke();
            yield return new WaitForSeconds(1);
            isWatching = true;
            
            rand = UnityEngine.Random.Range(2, 4);
            yield return new WaitForSeconds(rand);
            if (!spotted)
            {
                isWatching = false;
                inactiveEvent.Invoke();
            }
           // Debug.Log("I DONT see you");
        }
    }

    void WatchBehaviour()
    {
        for (int i = 0; i < viewedInputs.Count; i++)
        {
            if (player.inputDirection.magnitude != 0 && !spotted && !player.gameObject.GetComponent<Rigidbody>().isKinematic)
            {
                spotted = true;
                StartCoroutine(Kill());
               // Debug.Log("ah gottem ggs");
            }
        }
    }
    public void DontMoveText(GameObject textt)
    {
        if (firstTime) {
            StartCoroutine(textTimer(textt));
                }
    }
    private System.Collections.IEnumerator textTimer(GameObject textt)
    {
        firstTime = false;  
        textt.SetActive(true);
        yield return new WaitForSeconds(1);
        textt.SetActive(false);
    }
    private System.Collections.IEnumerator Kill()
    {
        if (filter.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            while (colorAdjustments.colorFilter.value != Color.black)
            {
                yield return new WaitForSeconds(0);
                colorAdjustments.colorFilter.value = Vector4.MoveTowards(colorAdjustments.colorFilter.value, Color.black, 0.01f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Killed by stopper");
        catchEvents.Invoke();
    }
}
