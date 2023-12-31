using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskStatus_Singleplayer : MonoBehaviour
{
    public Task_Singleplayer TaskReference { get; private set; }
    public string TaskDescription { get; private set; }

    public TextMeshProUGUI taskDescriptionTxt;
    public TextMeshProUGUI taskTimeTxt;

    float timeSeconds;
    float timeMinutes;

    public int TaskIndex { get; private set; }
    public void AssignTask(Task_Singleplayer task, int taskIndex)
    {
        TaskReference = task;
        TaskIndex = taskIndex;
        SetTaskDescription(TaskReference.TaskDescription);
        SetTaskTime((int)TaskReference.TaskTime);
        StartCoroutine(Timer());
    }

    private void SetTaskDescription(string taskDescription)
    {
        TaskDescription = taskDescription;
        taskDescriptionTxt.SetText(TaskDescription);
    }
    private void SetTaskTime(int time)
    {
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
}
