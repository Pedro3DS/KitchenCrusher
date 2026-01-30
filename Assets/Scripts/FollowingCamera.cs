using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target; // Referência para o transform do jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera
    public Vector3 offset; // Distância entre a câmera e o jogador
    public float limitRight, limitLeft, limitUp, limitDown;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.y = transform.position.y; // Manter a posição Y da câmera fixa
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.position = new Vector3(
                Mathf.Clamp(smoothedPosition.x, limitLeft, limitRight),
                smoothedPosition.y,
                Mathf.Clamp(smoothedPosition.z, limitDown, limitUp)
            );

        }
    }
}
