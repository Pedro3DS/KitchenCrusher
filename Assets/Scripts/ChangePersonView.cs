
using System;
using UnityEngine;

public class ChangePersonView : MonoBehaviour
{

    public static ChangePersonView Instance = null;

    [Header("Cam Views")]
    [SerializeField] private GameObject firstPersonView;
    [SerializeField] private GameObject topDownView;
    [Header("Script Views")]
    [SerializeField] private FirstPersonController firstPersonScript;
    [SerializeField] private TopViewMovement topDownScript;

    [SerializeField] private bool startInFirstPerson = true;

    void OnEnable()
    {
        GameManager.OnChangeToFirstPersonView += ChangeToFirstPersonView;
        GameManager.OnChangeToTopDownView += ChangeToTopDownView;
    }

    void OnDisable()
    {
        GameManager.OnChangeToFirstPersonView -= ChangeToFirstPersonView;
        GameManager.OnChangeToTopDownView -= ChangeToTopDownView;
    }

    void ChangeToFirstPersonView()
    {
        firstPersonView.SetActive(true);
        firstPersonScript.enabled = true;
        topDownView.SetActive(false);
        topDownScript.enabled = false;
    }
    void ChangeToTopDownView()
    {
        firstPersonView.SetActive(false);
        firstPersonScript.enabled = false;
        topDownView.SetActive(true);
        topDownScript.enabled = true;
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startInFirstPerson)
        {
            ChangeToFirstPersonView();
        }
        else
        {
            ChangeToTopDownView();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
