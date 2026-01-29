using UnityEngine;
using UnityEngine.Events;

public class Chaser : MonoBehaviour
{
   [SerializeField] Transform target;
    public float moveSpeed = 4;

    // Update is called once per frame
    void FixedUpdate()
    {
        GoTarget();
        
    }
    void GoTarget()
    {
        target.position = new Vector3 (target.position.x, transform.position.y, transform.position.z);
        
    }
   
}
