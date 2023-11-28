using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TaskManager_Multiplayer : NetworkBehaviour
{
    public static TaskManager_Multiplayer Instance;

    //Não tem maneira de dar scroll no ui de task e 7 é o numero maximo que cabe se for mais ele fica fora do UI
    const int maxTasks = 7;
    int TaskCount { get { return activeTasksStatusList.Count; } }

    [SerializeField] List<Task_Multiplayer> possibleTasksList;

    List<TaskStatus_Multiplayer> activeTasksStatusList = new List<TaskStatus_Multiplayer>();

    [SerializeField] float timeToCreateTask;

    [SerializeField] TaskStatus_Multiplayer activeTask_Prefab;
    [SerializeField] Transform activeTask_Container;

    [Header("Debug")]
    [SerializeField] bool noTasksStart = false;
    public override void OnNetworkSpawn()
    {
       
        Instance = this;
        if (noTasksStart)
        {
            Debug.Log("DebugTasksOn");
        }
        if (IsServer)
        {
            StartCoroutine(TaskCreator());
        }
    }
    //Isto tem que ser com tempos que podem variar find a way to do that later
    IEnumerator TaskCreator()
    {
        Debug.Log("Task Creator Start");
        while (true)
        {
            yield return new WaitForSeconds(timeToCreateTask);
            CreateTaskClientRpc();
        }
    }
    [ClientRpc]
    private void CreateTaskClientRpc()
    {
        Debug.Log("Creating Task");
        if (TaskCount >= maxTasks)
        {
            return;
        }
        Task_Multiplayer task = PickRandomTask();
        TaskStatus_Multiplayer taskStatus = Instantiate(activeTask_Prefab, activeTask_Container);
        taskStatus.AssignTask(task, TaskCount);
        activeTasksStatusList.Add(taskStatus);
        task.ActivateTask(taskStatus);
    }
    private Task_Multiplayer PickRandomTask()
    {
        //Se não existirem quest que se possam repetir ele eventualmente da um erro por não haver mais tasks para criar
        //No jogo em si isto não devo ser um problema visto termos sempre uma quest repetivel mas se acontecer fica aqui marcado para dar fix
        int randomIndex = Random.Range(0, possibleTasksList.Count);
        Task_Multiplayer Task = possibleTasksList[randomIndex];

        if (Task.IsRepeatable == false)
        {
            possibleTasksList.Remove(Task);
        }
        return Task;
    }
    [ServerRpc(RequireOwnership = false)]
    public void TaskCompletedServerRpc(int taskIndex)
    {
        DeleteTask_ClientRpc(taskIndex);
    }
    [ServerRpc(RequireOwnership = false)]
    public void TaskFailed_ServerRpc(int taskIndex)
    {
        DeleteTask_ClientRpc(taskIndex);
        //Energy bar diminuir ou modo de night time
    }
    [ClientRpc]
    private void DeleteTask_ClientRpc(int taskIndex)
    {
        if (taskIndex + 1 < TaskCount)
        {
            for (int i = taskIndex + 1; i < TaskCount; i++)
            {
                activeTasksStatusList[i].ReduceIndex();
            }
        }

        TaskStatus_Multiplayer taskStatus = activeTasksStatusList[taskIndex];
        if (taskStatus.TaskReference.IsRepeatable == false)
        {
            possibleTasksList.Add(taskStatus.TaskReference);
        }

        activeTasksStatusList.Remove(taskStatus);
        Destroy(taskStatus.gameObject);
    }
}
