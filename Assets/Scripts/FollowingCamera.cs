using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target; // Referência para o transform do jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera
    public Vector3 offset; // Distância entre a câmera e o jogador
    public float limitRight, limitLeft, limitUp, limitDown;

    void LateUpdate() // Melhor que FixedUpdate para câmeras
    {
        if (target != null)
        {
            // 1. Calculamos a posição desejada (X e Z do alvo + offset)
            // Mantemos o Y da própria câmera para ele não mudar
            Vector3 desiredPosition = new Vector3(
                target.position.x + offset.x,
                transform.position.y,
                target.position.z + offset.z
            );

            // 2. Suavizamos o movimento
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // 3. Aplicamos os limites (Clamp) no X e no Z
            float clampedX = Mathf.Clamp(smoothedPosition.x, limitLeft, limitRight);
            float clampedZ = Mathf.Clamp(smoothedPosition.z, limitDown, limitUp);

            // 4. Aplicamos a posição final
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
        }
    }
}
