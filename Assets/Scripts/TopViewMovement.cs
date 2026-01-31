using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

[RequireComponent(typeof(Rigidbody))]
public class TopViewMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;
    public ParticleSystem moveParticles;

    [Header("Physics")]
    public bool usePhysics = true;

    private Rigidbody rb;
    [SerializeField] private Animator animator;
    private Vector3 inputDirection;

    private float yValue;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // animator = GetComponent<Animator>();

        // Importante para Top Down
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ;
        yValue = transform.position.y;
    }

    void Update()
    {
        // Entrada padrão (teclado + controle)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        inputDirection = new Vector3(h, 0f, v);

        // Se houver direção, rotaciona para ela
        if (inputDirection.magnitude > 0.1f)
        {
            RotateTowards(inputDirection);
        }
        else
        {
            StopRotation();
        }
    }

    void FixedUpdate()
    {
        if (inputDirection.magnitude > 0.1f)
        {
            MoveForward();
        }
        else StopMovement();
    }

    void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void MoveForward()
    {
        Vector3 forwardMovement = transform.forward * moveSpeed;
        if (usePhysics)
        {
            animator?.SetBool("Moving", true);
            moveParticles?.Play();
            rb.linearVelocity = new Vector3(
                forwardMovement.x,
                rb.linearVelocity.y,
                forwardMovement.z
            );
        }
        else
        {
            moveParticles?.Play();
            animator?.SetBool("Moving", true);
            transform.position += forwardMovement * Time.deltaTime;
        }
    }
    void StopMovement()
    {
        if (usePhysics)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            animator?.SetBool("Moving", false);
            moveParticles?.Stop();
        }
    }
    void StopRotation()
    {
        if (usePhysics)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}
