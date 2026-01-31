using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class KitchenTask
{
    public string taskName;
    public int quantity;
    public Ingredient ingredientRequire;
    public bool hasGlitchTask;
    public float timeToComplete;
}

public class KitchenTaskController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform taskListField;
    [SerializeField] private TaskObject taskPrefab;

    [Header("Tasks")]
    [SerializeField] private KitchenTask[] possibleTasks;
    [SerializeField] private KitchenTask glitchTask; // Arraste a task do armazém aqui

    [Header("Configuração Progressiva")]
    [SerializeField] private float currentSpawnTime = 8f;
    [SerializeField] private int maxTasks = 1; // Começa com apenas 1

    [Header("Sistema de Estresse")]
    private int failedTasksCount = 0;
    private int stressThreshold;
    private bool isStressed = false;

    private readonly List<TaskObject> activeTasks = new List<TaskObject>();

    private void Start()
    {
        // Define o limite de estresse entre 3 e 5
        stressThreshold = Random.Range(3, 6);
        StartCoroutine(TaskRoutine());
    }

    private void OnEnable() => GameEvents.OnPlateDelivered += CheckTasks;
    private void OnDisable() => GameEvents.OnPlateDelivered -= CheckTasks;

    IEnumerator TaskRoutine()
    {
        while (!isStressed)
        {
            yield return new WaitForSeconds(Random.Range(currentSpawnTime * 0.8f, currentSpawnTime));

            if (activeTasks.Count < maxTasks)
            {
                CreateRandomTask();
            }
        }
    }

    void CreateRandomTask()
    {
        KitchenTask randomTask = possibleTasks[Random.Range(0, possibleTasks.Length)];
        SpawnTaskUI(randomTask);

        // Dificuldade progressiva suave
        if (maxTasks < 4) maxTasks = (failedTasksCount / 2) + 1;
    }

    void SpawnTaskUI(KitchenTask data)
    {
        TaskObject taskUI = Instantiate(taskPrefab, taskListField);
        taskUI.Setup(data);
        taskUI.OnTaskCompleted += HandleTaskCompleted;
        taskUI.OnTaskFailed += HandleTaskFailed;
        activeTasks.Add(taskUI);
    }

    // --- O CONSERTO DA ENTREGA ---
    void CheckTasks(Plate plate)
    {
        // Procuramos a tarefa MAIS ANTIGA (índice 0) que combine com o prato
        for (int i = 0; i < activeTasks.Count; i++)
        {
            TaskObject taskObject = activeTasks[i];
            KitchenTask taskData = taskObject.TaskData;

            int count = plate.CountCookedIngredient(taskData.ingredientRequire);

            if (count >= taskData.quantity)
            {
                taskObject.CompleteTaskExternally();
                return; // IMPORTANTE: Para o loop aqui para não completar duas tasks!
            }
        }
    }

    void HandleTaskCompleted(TaskObject task)
    {
        activeTasks.Remove(task);
        // Acelera o jogo a cada acerto
        currentSpawnTime = Mathf.Max(3f, currentSpawnTime - 0.5f);
        Destroy(task.gameObject);
    }

    void HandleTaskFailed(TaskObject task)
    {
        activeTasks.Remove(task);
        failedTasksCount++;

        Debug.Log($"Tasks falhadas: {failedTasksCount} / {stressThreshold}");

        if (failedTasksCount >= stressThreshold && !isStressed)
        {
            TriggerStressEvent();
        }

        Destroy(task.gameObject);
    }

    void TriggerStressEvent()
    {
        isStressed = true;
        // Limpa tasks normais para focar no horror
        foreach (var t in activeTasks) Destroy(t.gameObject);
        activeTasks.Clear();

        // Spawna a Task do Glitch (Armazém)
        SpawnTaskUI(glitchTask);

        Debug.Log("SISTEMA DE ESTRESSE ATIVADO. Vá para o armazém.");
    }
}






