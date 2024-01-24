using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class TaskStatus_Multiplayer : NetworkBehaviour
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
        TaskDescription = TaskReference.TaskDescription;
        SetTaskDescription_ClientRpc(TaskDescription);
        SetTaskTime();
        StartCoroutine(Timer());
    }
    [ClientRpc]
    private void SetTaskDescription_ClientRpc(string taskDescription)
    {
        taskDescriptionTxt.SetText(taskDescription);
    }
    private void SetTaskTime()
    {
        int time = (int)TaskReference.TaskTime;
        timeMinutes = time / 60;
        timeSeconds = time % 60;
        SetTaskTimeTxt_ClientRpc(timeSeconds, timeMinutes);
    }
    [ClientRpc]
    private void SetTaskTimeTxt_ClientRpc(float timeSecons,float timeMinutes)
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
            SetTaskTimeTxt_ClientRpc(timeSeconds, timeMinutes);
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
        WasTaskCompleted_ClientRpc(wasTaskCompleted);
        Invoke(nameof(Delete), showAfterCompletion);
    }
    [ClientRpc]
    private void WasTaskCompleted_ClientRpc(bool wasTaskCompleted)
    {
        if (wasTaskCompleted)
        {
            image.sprite = taskCompleted;
        }
        else
        {
            image.sprite = taskFailed;
        }
    }
    private void Delete()
    {
        uiAnimation.ExitAnimation();
    }
}
