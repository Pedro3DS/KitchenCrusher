using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Configurações de Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Função principal que você chamará
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        // 1. Inicia o Fade Out (Tela fica preta)
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
