using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Material mat;
    [SerializeField] GameObject hideText;
    [SerializeField] private ParticleSystem particle;
    private AudioSource _source;
    public float moveSpeed = 2;
    public float catchDistance = 0.5f;
    public UnityEvent catchEvents;
    private bool spawned = false;
    private bool On = true;
    private bool hunting;
    private bool firstTime = true;
    private Transform mainTarget;
    [SerializeField] float minX, maxX, minZ, maxZ;

    private void OnEnable()
    {
        StartCoroutine(showUpTimer());
    }
    private void Start()
    {
        mainTarget = target;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private System.Collections.IEnumerator showUpTimer()
    {
        while (On && !spawned)
        {                  
            int rand = UnityEngine.Random.Range(14, 24);     
            yield return new WaitForSeconds(rand);
            spawned = true;
            rand = UnityEngine.Random.Range(0, 2);
            Debug.Log(rand);
            if (rand == 0)
            {
            }
            else
            {
                spawned = false;
                continue;
            }
            if (moveSpeed > 0)
            {
                HideText(hideText);
            }
            transform.position = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
            _source.Play();
            yield return new WaitForSeconds(0.2f);
            while (mat.color.a < 1)
            {
                yield return new WaitForSeconds(0);
              float colorValue = mat.color.a;
              colorValue = Mathf.MoveTowards(colorValue, 1, 0.05f);
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, colorValue);
            }
            particle.Play();
            yield return new WaitForSeconds(2);
            hunting = true;
            StartCoroutine(GetTarget());
            yield return new WaitForSeconds(3);
            hunting = false;
            StopCoroutine(GetTarget());
            while (mat.color.a > 0)
            {
                yield return new WaitForSeconds(0);
                float colorValue = mat.color.a;
                colorValue = Mathf.MoveTowards(colorValue, 0, 0.05f);
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, colorValue);
            }
            particle.Stop();
            spawned = false;
            // Debug.Log("I see you");

            // Debug.Log("I DONT see you");
        }
    }
    private System.Collections.IEnumerator GetTarget()
    {
        while (hunting)
        {
            target = mainTarget;
            if (target.gameObject.activeSelf && moveSpeed != 0)
            {
                float distance = Vector3.Distance(transform.position, mainTarget.position);
                //Debug.Log(distance);
                yield return new WaitForSeconds(0);
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
                if (distance < catchDistance)
                {
                    Debug.Log("Killed by ghost");
                    catchEvents.Invoke();
                }
            }
            else
            {
                yield return new WaitForSeconds(0);
                target = transform;
            }
        }
    }

    public void StopIt(float speed)
    {
        StartCoroutine(StopperInteraction(speed));
    }
    private IEnumerator StopperInteraction(float speed)
    {
        yield return new WaitForSeconds(0);
        if (speed <= 0)
        {
            moveSpeed = 0;
        }
        else 
        {
            HideText(hideText);
            yield return new WaitForSeconds(3);        
            moveSpeed = speed;
        }
        
        
    }

    public void HideText(GameObject textt)
    {
        Debug.Log(spawned);
        if (firstTime && spawned)
        {
            firstTime = false;
            StartCoroutine(textTimerHide(textt));
        }
    }

    private System.Collections.IEnumerator textTimerHide(GameObject textt)
    {
        textt.SetActive(true);
        yield return new WaitForSeconds(1);
        textt.SetActive(false);
    }
}
