using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager_Singleplayer : MonoBehaviour
{
    public static TaskManager_Singleplayer Instance;

    //Não tem maneira de dar scroll no ui de task e 7 é o numero maximo que cabe se for mais ele fica fora do UI
    const int maxTasks = 7;
    int TaskCount { get { return activeTasksStatusList.Count; } }

    [SerializeField] List<Task_Singleplayer> possibleTasksList;

    List<TaskStatus_Singleplayer> activeTasksStatusList = new List<TaskStatus_Singleplayer>();

    [SerializeField] float timeToCreateTask;

    [SerializeField] TaskStatus_Singleplayer activeTask_Prefab;
    [SerializeField] Transform activeTask_Container;

    [Header("Debug")]
    [SerializeField] bool noTasksStart = false;
    private void Awake()
    {
        Instance = this;
        if (noTasksStart)
        {
            Debug.Log("DebugTasksOn");
            return;
        }
        StartCoroutine(TaskCreator());
    }

    IEnumerator TaskCreator()
    {
        Debug.Log("Task Creator Start");
        while (true)
        {
            yield return new WaitForSeconds(timeToCreateTask);
            CreateTask();
        }
    }
    private void CreateTask()
    {
        Debug.Log("Creating Task");
        if (TaskCount >= maxTasks)
        {
            return;
        }
        Task_Singleplayer task = PickRandomTask();
        TaskStatus_Singleplayer taskStatus = Instantiate(activeTask_Prefab, activeTask_Container);
        taskStatus.AssignTask(task, TaskCount);
        activeTasksStatusList.Add(taskStatus);
        task.ActivateTask(taskStatus);
    }
    private Task_Singleplayer PickRandomTask()
    {
        //Se não existirem quest que se possam repetir ele eventualmente da um erro por não haver mais tasks para criar
        //No jogo em si isto não devo ser um problema visto termos sempre uma quest repetivel mas se acontecer fica aqui marcado para dar fix
        int randomIndex = Random.Range(0, possibleTasksList.Count);
        Task_Singleplayer Task = possibleTasksList[randomIndex];

        if (Task.IsRepeatable == false)
        {
            possibleTasksList.Remove(Task);
        }
        return Task;
    }
    public void TaskCompleted(int taskIndex)
    {
        DeleteTask(taskIndex);
    }
    public void TaskFailed(int taskIndex)
    {
        DeleteTask(taskIndex);
        //Energy bar diminuir ou modo de night time
    }
    private void DeleteTask(int taskIndex)
    {
        if (taskIndex + 1 < TaskCount)
        {
            for (int i = taskIndex + 1; i < TaskCount; i++)
            {
                activeTasksStatusList[i].ReduceIndex();
            }
        }

        TaskStatus_Singleplayer taskStatus = activeTasksStatusList[taskIndex];
        if (taskStatus.TaskReference.IsRepeatable == false)
        {
            possibleTasksList.Add(taskStatus.TaskReference);
        }

        activeTasksStatusList.Remove(taskStatus);
        Destroy(taskStatus.gameObject);
    }
}
