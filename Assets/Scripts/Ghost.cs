using UnityEngine;
using UnityEngine.Events;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Material mat;
    private AudioSource _source;
    public float moveSpeed = 8;
    public float catchDistance = 0.5f;
    public UnityEvent catchEvents;
    private bool spawned;
    private bool On = true;
    [SerializeField] float minX, maxX, minZ, maxZ;

    private void OnEnable()
    {
        StartCoroutine(showUpTimer());
    }
    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private System.Collections.IEnumerator showUpTimer()
    {
        while (On && !spawned)
        {
            transform.position = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
            spawned = true;
            int rand = UnityEngine.Random.Range(10, 14);
            yield return new WaitForSeconds(rand);

            while (mat.color.a > 0)
            {
              float colorValue = mat.color.a;
              colorValue = Mathf.MoveTowards(colorValue, 1, 0.1f);
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, colorValue);
            }

            yield return new WaitForSeconds(3);
            while (mat.color.a < 1)
            {
                float colorValue = mat.color.a;
                colorValue = Mathf.MoveTowards(colorValue, 0, 0.1f);
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, colorValue);
            }
            // Debug.Log("I see you");

            // Debug.Log("I DONT see you");
        }
    }
    private System.Collections.IEnumerator GetTarget()
    {
        if (target.gameObject.activeSelf)
        {
            Vector3 targetCorrected = new Vector3(target.transform.position.x, transform.position.y, target.position.z);
            float distance = Vector3.Distance(transform.position, targetCorrected);
            Debug.Log(distance);
            yield return new WaitForSeconds(0);
            transform.position = Vector3.MoveTowards(transform.position, targetCorrected, moveSpeed * 0.020f);
            if (distance < catchDistance)
            {
                catchEvents.Invoke();
            }
        } else
        {
            target = transform;
            for(int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1);
                if (target.gameObject.activeSelf)
                {
                    break;
                }
            }
        }
    }
}
