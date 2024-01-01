using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTaskStatus_Multiplayer : MonoBehaviour
{
    public TutorialTask_Multiplayer TaskReference { get; private set; }
    public string TaskDescription { get; private set; }

    Image image;
    UIAutoAnimation uiAnimation;

    public TextMeshProUGUI taskDescriptionTxt;

    public int TaskIndex { get; private set; }

    [SerializeField] Sprite taskFailed;
    [SerializeField] Sprite taskCompleted;

    [SerializeField] float showAfterCompletion;
    private void Awake()
    {
        uiAnimation = GetComponent<UIAutoAnimation>();
        image = GetComponent<Image>();
    }

    public void AssignTask(TutorialTask_Multiplayer task, int taskIndex)
    {
        TaskReference = task;
        TaskIndex = taskIndex;
        SetTaskDescription(TaskReference.TaskDescription);
    }
    private void SetTaskDescription(string taskDescription)
    {
        TaskDescription = taskDescription;
        taskDescriptionTxt.SetText(TaskDescription);
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
