using Unity.VisualScripting;
using UnityEngine;

public class SpawnPlate : MonoBehaviour
{

    [SerializeField] private Plate platePrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Configuração do Salto")]
    [SerializeField] private float upForce = 5f;        // Força para cima
    [SerializeField] private float sideForce = 1f;      // Força aleatória para os lados (opcional)
    [SerializeField] private float torqueForce = 2f;    // Força de rotação para o prato "girar" no ar

    void OnEnable()
    {
        GameEvents.OnPlateDelivered += HandlePlateDelivered;
        GameEvents.OnPlateBraked += InstantiatePlate;
    }

    void OnDisable() // É melhor usar OnDisable para parear com OnEnable
    {
        GameEvents.OnPlateDelivered -= HandlePlateDelivered;
        GameEvents.OnPlateBraked -= InstantiatePlate;
    }

    void HandlePlateDelivered(Plate plate)
    {
        // 1. Instancia o prato na posição do spawnPoint
        InstantiatePlate();
    }

    void InstantiatePlate()
    {
        Plate newPlate = Instantiate(platePrefab, spawnPoint.position, spawnPoint.rotation);
        if (newPlate.GetComponent<Rigidbody>() == null) newPlate.AddComponent<Rigidbody>();
        Rigidbody rb = newPlate.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.AddForce(0f, upForce, 0f, ForceMode.Impulse);
    }
}
