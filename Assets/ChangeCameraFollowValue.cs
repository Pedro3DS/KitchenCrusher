using UnityEngine;

public class ChangeCameraFollowValue : MonoBehaviour
{
    public enum MovementDirection { PositiveX, NegativeX, PositiveZ, NegativeZ }

    [SerializeField] private FollowingCamera cameraFollow;

    [Header("Condição de Ativação")]
    [SerializeField] private MovementDirection requiredDirection;
    [SerializeField] private float sensitivity = 0.5f; // Quanto "reto" ele precisa estar (0 a 1)

    [Header("Novos Limites")]
    [SerializeField] private Vector2 newXLimits;
    [SerializeField] private Vector2 newZLimits;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Pegamos a direção do movimento do Player
            // Se usar Rigidbody:
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Vector3 playerVelocity = rb != null ? rb.linearVelocity : Vector3.zero;

            // Se não usar Rigidbody, podemos calcular a direção baseada no Input ou posição anterior
            // Mas o Velocity é o mais seguro.

            if (playerVelocity.magnitude < 0.1f) return; // Player parado não ativa

            Vector3 moveDir = playerVelocity.normalized;
            Vector3 targetDir = GetTargetVector();

            // 2. O Calculo: Dot Product
            // Se o resultado for 1, as direções são idênticas. 
            // Se for 0, são perpendiculares. Se for -1, opostas.
            float dot = Vector3.Dot(moveDir, targetDir);

            if (dot > sensitivity)
            {
                Debug.Log($"Direção validada! Mudando câmera para {requiredDirection}");
                cameraFollow.SetXLimits(newXLimits);
                cameraFollow.SetZLimits(newZLimits);
            }
        }
    }

    // Converte o Enum para um Vetor real do Unity
    private Vector3 GetTargetVector()
    {
        switch (requiredDirection)
        {
            case MovementDirection.PositiveX: return Vector3.right;   // (1, 0, 0)
            case MovementDirection.NegativeX: return Vector3.left;    // (-1, 0, 0)
            case MovementDirection.PositiveZ: return Vector3.forward; // (0, 0, 1)
            case MovementDirection.NegativeZ: return Vector3.back;    // (0, 0, -1)
            default: return Vector3.zero;
        }
    }
}
