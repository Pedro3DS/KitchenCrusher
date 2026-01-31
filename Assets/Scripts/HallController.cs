using System;
using UnityEngine;


[Serializable]
public class HallConfigs
{
    public GameObject hallwayPrefab;
    public Transform hallwayPosition;
    public Transform newFreezerPosition;
    public Transform hallwayNewPlayerPosition;
    public bool changeCamera;
    // public

}
public class HallController : MonoBehaviour
{

    [SerializeField] private HallConfigs[] hallwaysConfigs;

    [SerializeField] private GameObject currentFreezer;
    [SerializeField] private GameObject player;

    public static HallController Instance = null;

    public int currentHallIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetHallIndex(int index)
    {
        if (index < 0 || index >= hallwaysConfigs.Length)
        {
            Debug.LogError("Invalid hallway index: " + index);
        }
        currentHallIndex = index;
        SetHallwayActive(index, true);
    }




    public HallConfigs GetHallConfig(int index)
    {
        if (index < 0 || index >= hallwaysConfigs.Length)
        {
            Debug.LogError("Invalid hallway index: " + index);
            return null;
        }
        return hallwaysConfigs[index];
    }

    public void SetHallwayActive(int index, bool isActive)
    {
        HallConfigs config = GetHallConfig(index);
        if (config != null && config.hallwayPrefab != null)
        {
            
        }
    }
}
