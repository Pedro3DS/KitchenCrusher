using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class SceneSwitcher : MonoBehaviour
{

    [Header("Configurações de Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;
    public void ResetScene(float time)
    {
        StartCoroutine(ResetTimer(time));
    }
    private IEnumerator ResetTimer(float time)
    {
        yield return new WaitForSeconds(time);
        yield return StartCoroutine(Fade(1));

        // 2. Carrega a cena de forma assíncrona
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        // Impede que a cena apareça antes de terminar de carregar
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Se o carregamento chegou em 90%, ele está pronto
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        // 3. Espera um microsegundo para garantir que a nova cena iniciou
        yield return new WaitForSeconds(0.2f);

        // 4. Inicia o Fade In (Tela volta ao normal)
        yield return StartCoroutine(Fade(0));
    }
    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    IEnumerator TransitionRoutine(string sceneName)
    {
        yield return StartCoroutine(Fade(1));

        // 2. Carrega a cena de forma assíncrona
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Impede que a cena apareça antes de terminar de carregar
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Se o carregamento chegou em 90%, ele está pronto
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        // 3. Espera um microsegundo para garantir que a nova cena iniciou
        yield return new WaitForSeconds(0.2f);

        // 4. Inicia o Fade In (Tela volta ao normal)
        yield return StartCoroutine(Fade(0));
    }

}
