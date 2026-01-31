using UnityEngine;

public class HallwayTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "Cena_Corredor_01";


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.ChangeScene(sceneName);
            }
        }
    }
}
