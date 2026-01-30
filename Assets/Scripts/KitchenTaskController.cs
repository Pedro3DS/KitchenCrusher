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

    [Header("Config")]
    [SerializeField] private float maxRandomTaskTime = 5f;
    [SerializeField] private int maxTaskInList = 3;

    private readonly List<TaskObject> activeTasks = new List<TaskObject>();

    private void Start()
    {
        StartCoroutine(TaskRoutine());
    }

    private void OnEnable()
    {
        GameEvents.OnPlateDelivered += CheckTasks;
    }

    private void OnDisable()
    {
        GameEvents.OnPlateDelivered -= CheckTasks;
    }

    IEnumerator TaskRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, maxRandomTaskTime));

            if (activeTasks.Count < maxTaskInList)
            {
                CreateRandomTask();
            }
        }
    }

    void CreateRandomTask()
    {
        KitchenTask randomTask =
            possibleTasks[Random.Range(0, possibleTasks.Length)];

        TaskObject taskUI = Instantiate(taskPrefab, taskListField);
        taskUI.Setup(randomTask);

        taskUI.OnTaskCompleted += HandleTaskCompleted;
        taskUI.OnTaskFailed += HandleTaskFailed;

        activeTasks.Add(taskUI);
    }

    void CheckTasks(Plate plate)
    {
        for (int i = activeTasks.Count - 1; i >= 0; i--)
        {
            TaskObject taskObject = activeTasks[i];
            KitchenTask taskData = taskObject.TaskData;

            // Usamos a nova função que criamos no Plate.cs
            int count = plate.CountCookedIngredient(taskData.ingredientRequire);

            if (count >= taskData.quantity)
            {
                taskObject.CompleteTaskExternally();
                // Removi o break para caso um prato complete mais de uma task (raro, mas possível)
            }
        }
    }

    void HandleTaskCompleted(TaskObject task)
    {
        activeTasks.Remove(task);

        if (task.TaskData.hasGlitchTask)
        {
            GameEvents.OnTaskCompleted?.Invoke(task.TaskData);
        }

        Destroy(task.gameObject);
    }

    void HandleTaskFailed(TaskObject task)
    {
        activeTasks.Remove(task);
        Destroy(task.gameObject);
    }
}






