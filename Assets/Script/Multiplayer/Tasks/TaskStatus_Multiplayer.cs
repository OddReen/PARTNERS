using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskStatus_Multiplayer : MonoBehaviour
{
    public Task_Multiplayer TaskReference { get; private set; }
    public string TaskDescription { get; private set; }

    Image image;
    UIAutoAnimation uiAnimation;

    [SerializeField] TextMeshProUGUI taskDescriptionTxt;
    [SerializeField] TextMeshProUGUI taskTimeTxt;

    [SerializeField] Sprite taskFailed;
    [SerializeField] Sprite taskCompleted;
    
    float timeSeconds;
    float timeMinutes;

    public int TaskIndex { get; private set; }

    [SerializeField] float showAfterCompletion;
    private void Awake()
    {
        uiAnimation = GetComponent<UIAutoAnimation>();
        image = GetComponent<Image>();
    }
    public void AssignTask(Task_Multiplayer task, int taskIndex)
    {
        TaskReference = task;
        TaskIndex = taskIndex;
        SetTaskDescription();
        SetTaskTime();
        StartCoroutine(Timer());
    }

    private void SetTaskDescription()
    {
        TaskDescription = TaskReference.TaskDescription;
        taskDescriptionTxt.SetText(TaskDescription);
    }
    private void SetTaskTime()
    {
        int time = (int)TaskReference.TaskTime;
        timeMinutes = time / 60;
        timeSeconds = time % 60;
        SetTaskTimeTxt();
    }
    private void SetTaskTimeTxt()
    {
        taskTimeTxt.SetText($"{timeMinutes}:{timeSeconds}");
    }
    //This feels wrong somehow se souberes uma forma melhor por favor avisa
    IEnumerator Timer()
    {
        do
        {
            yield return new WaitForSeconds(1f);
            timeSeconds--;
            if (timeSeconds <= 0 && timeMinutes > 0)
            {
                timeMinutes--;
                timeSeconds = 60;
            }
            SetTaskTimeTxt();
        } while (timeSeconds > 0);
        FailTask();
    }
    public void FailTask()
    {
        TaskReference.FailTask();
    }
    public void ReduceIndex()
    {
        TaskIndex--;
    }
    public void DeleteTaskStatus(bool wasTaskCompleted)
    {
        StopAllCoroutines();
        taskTimeTxt.SetText("");
        if (wasTaskCompleted)
        {
            image.sprite = taskCompleted;
        }
        else
        {
            image.sprite = taskFailed;
        }
        Invoke(nameof(Delete), showAfterCompletion);
    }
    private void Delete()
    {
        uiAnimation.ExitAnimation();
    }
}
