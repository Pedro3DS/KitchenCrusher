using UnityEngine;

public class HENRIQUEmove : MonoBehaviour
{
    private Vector3 moveDir;
    public float moveSpeed = 5f;
    private Rigidbody rb;
    [SerializeField] private Transform cameraPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
       GetDir();
    }

    private void FixedUpdate()
    {
        cameraPos.position = new Vector3 (transform.position.x, cameraPos.position.y, cameraPos.position.z);
        rb.linearVelocity = moveDir * moveSpeed;
    }
    private void GetDir()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");
        moveDir.Normalize();
    }
}
