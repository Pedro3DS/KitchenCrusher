using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para reiniciar a cena

public class ChaseWall : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float initialSpeed = 3.0f;
    [SerializeField] private float accelerationRate = 0.2f; // O quanto a velocidade sobe por segundo
    [SerializeField] private float maxWallSpeed = 8.0f;    // Velocidade máxima para não ficar impossível

    private float currentSpeed;

    [Header("Áudio e Música")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource jumpscareSource;
    [SerializeField] private AudioClip jumpscareClip;

    [Header("Distâncias de Controle")]
    [SerializeField] private float nearDistance = 10f;
    [SerializeField] private float criticalDistance = 3f;

    private bool isDead = false;

    void Start()
    {
        // Começa com a velocidade inicial definida
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        if (isDead) return;

        // 1. Aumenta a velocidade gradualmente ao longo do tempo
        if (currentSpeed < maxWallSpeed)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
        }

        MoveWall();
        ControlMusicPitch();
    }

    void MoveWall()
    {
        // Move a parede em direção ao player com a velocidade atualizada
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, currentSpeed * Time.deltaTime);
    }

    void ControlMusicPitch()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > nearDistance)
        {
            musicSource.pitch = 1.0f;
        }
        else if (distance <= nearDistance && distance > criticalDistance)
        {
            // Mapeia o pitch para aumentar conforme a distância diminui
            float t = Mathf.InverseLerp(nearDistance, criticalDistance, distance);
            musicSource.pitch = Mathf.Lerp(1.0f, 1.1f, t);
        }
        else if (distance <= criticalDistance)
        {
            // Pitch "murcha" quando está colado no player
            musicSource.pitch = 0.6f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        musicSource.Stop();

        if (jumpscareClip != null)
        {
            jumpscareSource.PlayOneShot(jumpscareClip);
        }

        // Reinicia a cena
        Invoke("RestartLevel", 1f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}