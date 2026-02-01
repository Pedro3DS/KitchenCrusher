using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private Slider stressBar;
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] Animator pointAnim;

    [Header("Tasks")]
    [SerializeField] private KitchenTask[] possibleTasks;
    [SerializeField] private KitchenTask glitchTask; // Arraste a task do armazém aqui
    private int pointQueue;
    private int points = 1;
    private bool pointBusy;

    [Header("Configuração Progressiva")]
    [SerializeField] private float currentSpawnTime = 8f;
    [SerializeField] private int maxTasks = 1; // Começa com apenas 1

    [Header("Sistema de Estresse")]
    private int failedTasksCount = 0;
    private int stressThreshold;
    private bool isStressed = false;
    // private int stressLevel = 0;
    

    private readonly List<TaskObject> activeTasks = new List<TaskObject>();

    public UnityEvent loseEvent;
    [SerializeField] GameObject finalTrigger;

    private void Start()
    {
        // Define o limite de estresse entre 3 e 5
        stressThreshold = Random.Range(3, 6);
        StartCoroutine(TaskRoutine());
        points = PlayerPrefs.GetInt("Points");
        if (points <= 0)
        {
            points = 1;
        }
        pointText.text = $"{points}x";
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

        if(maxTasks < 4 && (points >= 15 && points <= 16)) maxTasks = maxTasks+1;

        
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
        AddPoints(true, task.TaskData);
        activeTasks.Remove(task);
        // Acelera o jogo a cada acerto
        currentSpawnTime = Mathf.Max(3f, currentSpawnTime - 0.5f);
        Destroy(task.gameObject);
    }

    void HandleTaskFailed(TaskObject task)
    {
        activeTasks.Remove(task);
        AddPoints(false, task.TaskData);
        failedTasksCount++;

        Debug.Log($"Tasks falhadas: {failedTasksCount} / {stressThreshold}");

        if (failedTasksCount >= stressThreshold && !isStressed)
        {
            TriggerStressEvent();
        }

        if (stressBar.gameObject.activeSelf)
        {
            SliderValue(0.2f);
        }
        Destroy(task.gameObject);
    }

    void TriggerStressEvent()
    {
        isStressed = true;
        stressBar.gameObject.SetActive(true);
        SliderValue(0.2f);
        // Limpa tasks normais para focar no horror
        foreach (var t in activeTasks) Destroy(t.gameObject);
        activeTasks.Clear();

        // Spawna a Task do Glitch (Armazém)
        SpawnTaskUI(glitchTask);
        finalTrigger.gameObject.SetActive(true);
        Debug.Log("SISTEMA DE ESTRESSE ATIVADO. Vá para o armazém.");
    }


    public void SliderValue(float value)
    {
        StartCoroutine(SliderRoutine(value));
    }
    private IEnumerator SliderRoutine(float value)
    {
        float finalValue = stressBar.value + value;
        while (stressBar.value < finalValue)
        {
            yield return new WaitForSeconds(0);
            Mathf.MoveTowards(stressBar.value, finalValue, 0.1f);
        }
    }
    void AddPoints(bool add, KitchenTask task)
    {
        pointQueue++;
        StartCoroutine(PointsAnim(add, task));
    }
    private IEnumerator PointsAnim(bool add, KitchenTask task)
    {
        while (pointQueue > 0) {
            yield return new WaitForSeconds(0);
            if (!pointBusy)
            {
                pointBusy = true;

                if (add)
                {
                    points += task.quantity;
                    pointText.text = $"{points}x";
                    pointAnim.Play("pointAnim");
                    yield return new WaitForSeconds(0.5f);
                    pointQueue--;
                    pointBusy = false;
                    break;
                }
                else
                {
                    if (points > task.quantity)
                    {
                        points -= task.quantity;
                        pointText.text = $"{points}x";
                    } else
                    {
                        points = 1;
                        pointText.text = $"{points}x";
                        loseEvent.Invoke();
                    }
                        pointAnim.Play("failAnim");
                    yield return new WaitForSeconds(0.5f);
                    pointQueue--;
                    pointBusy = false;
                    break;
                }

            }
        }
    }
    public void SetPref()
    {
        PlayerPrefs.SetInt("Points", points);
    }
}






