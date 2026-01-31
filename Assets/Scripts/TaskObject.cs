using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskObject : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text taskNameText;

    public KitchenTask TaskData { get; private set; }

    public event Action<TaskObject> OnTaskCompleted;
    public event Action<TaskObject> OnTaskFailed;

    private float currentTime;
    private bool isTimerPaused = false;

    public void Setup(KitchenTask task)
    {
        TaskData = task;

        currentTime = task.timeToComplete;
        timeSlider.maxValue = currentTime;
        timeSlider.value = currentTime;

        quantityText.text = task.quantity.ToString();
        taskNameText.text = task.taskName;

        // Reseta o estado para tasks normais
        isTimerPaused = false;
        timeSlider.gameObject.SetActive(true);

        // Se for uma glitch task (pão), pausa o timer
        if (task.hasGlitchTask)
        {
            isTimerPaused = true;
            timeSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Não atualiza o timer se estiver pausado (glitch task)
        if (isTimerPaused) return;

        currentTime -= Time.deltaTime;
        timeSlider.value = currentTime;
        if (timeSlider.value < timeSlider.maxValue * 0.3f)
        {
            timeSlider.fillRect.GetComponent<Image>().color = Color.red;
        }

        if (currentTime <= 0)
        {
            FailTask();
        }
    }

    /// <summary>
    /// Pausa ou retoma o timer da task.
    /// </summary>
    public void SetTimerPaused(bool paused)
    {
        isTimerPaused = paused;
    }

    // Chamado por outra classe (ex: quando cozinha ovo)
    public void ReduceQuantity(int amount)
    {
        TaskData.quantity -= amount;

        quantityText.text = TaskData.quantity.ToString();

        if (TaskData.quantity <= 0)
        {
            CompleteTask();
        }
    }

    void CompleteTask()
    {
        OnTaskCompleted?.Invoke(this);
    }

    void FailTask()
    {
        OnTaskFailed?.Invoke(this);
    }
    public void CompleteTaskExternally()
    {
        CompleteTask();
    }
}
