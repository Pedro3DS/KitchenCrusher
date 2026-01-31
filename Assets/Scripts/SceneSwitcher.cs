using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
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
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void SetPref(int value)
    {
        PlayerPrefs.SetInt("Points", value);
    }
}
