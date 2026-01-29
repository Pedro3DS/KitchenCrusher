using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void ResetScene(float time)
    {
        StartCoroutine(ResetTimer(time));
    }
    private IEnumerator ResetTimer(float time)
    {
        yield return new WaitForSeconds(time);
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
