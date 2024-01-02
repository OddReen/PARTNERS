using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSewer_Multiplayer : Task_Multiplayer
{
    public static TaskSewer_Multiplayer Instance;

    [SerializeField] Animator pipe1, pipe2, pipe3,pipe4;
    bool isFilled1 = false, isFilled2 = false, isFilled3 = false, isFilled4;

    private void Start()
    {
        FillPipe();
        Instance = this;
    }
    public override void ActivateTask(TaskStatus_Multiplayer taskInfo)
    {
        base.ActivateTask(taskInfo);
        isFilled1 = false;
        isFilled2 = false;
        isFilled3 = false;
        FillPipe();
    }
    public void FillMinigame(string buttonString)
    {
        if (isTaskActive == false)
        {
            return;
        }
        switch (buttonString)
        {
            case "Button1":
                isFilled1 ^= true;
                isFilled2 ^= true;
                break;
            case "Button2":
                isFilled2 ^= true;
                isFilled4 ^= true;
                break;
            case "Button3":
                isFilled1 ^= true;
                break;
            case "Button4":
                isFilled1 ^= true;
                isFilled2 ^= true;
                isFilled3 ^= true;
                break;
        }
        FillPipe();
        Fix();
    }
    //This doesnt work multiplayer do later
    public void FillPipe()
    {
        if (isFilled1)
            pipe1.SetTrigger("Fill");
        else
            pipe1.SetTrigger("Empty");

        if (isFilled2)
            pipe2.SetTrigger("Fill");
        else
            pipe2.SetTrigger("Empty");

        if (isFilled3)
            pipe3.SetTrigger("Fill");
        else
            pipe3.SetTrigger("Empty");

        if (isFilled4)
            pipe4.SetTrigger("Fill");
        else
            pipe4.SetTrigger("Empty");

        StartCoroutine(ResetTrigger(pipe1));
        StartCoroutine(ResetTrigger(pipe2));
        StartCoroutine(ResetTrigger(pipe3));
        StartCoroutine(ResetTrigger(pipe4));
    }
    IEnumerator ResetTrigger(Animator _animator)
    {
        yield return new WaitForEndOfFrame();
        _animator.ResetTrigger("Fill");
        _animator.ResetTrigger("Empty");
    }
    public void Fix()
    {
        if (isFilled1 && isFilled2 && isFilled3 && isFilled4)
        {
            CompleteTask();
        }
    }
}
