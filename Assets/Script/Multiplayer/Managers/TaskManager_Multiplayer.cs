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

    List<TaskStatus_Multiplayer> activeTasksStatusList = new();

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
            return;
        }
        if (IsServer)
        {
            StartCoroutine(TaskCreator());
            BlackoutManager_Multiplayer.Instance.StartBlackout += BlackoutManager_StartBlackout;
            BlackoutManager_Multiplayer.Instance.EndBlackout += BlackoutManager_EndBlackout; ;
        }
    }

    private void BlackoutManager_EndBlackout(object sender, System.EventArgs e)
    {
        StartCreatingTasks_ServerRpc();
    }

    private void BlackoutManager_StartBlackout(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        for (int i = 0; i < activeTasksStatusList.Count; i++)
        {
            TaskFailed_ServerRpc(i);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartCreatingTasks_ServerRpc()
    {
        CreateTasks_ServerRpc();
        StartCoroutine(TaskCreator());
    }
    //Isto tem que ser com tempos que podem variar find a way to do that later
    //Tem que ser executada pelo server
    IEnumerator TaskCreator()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToCreateTask);
            CreateTasks_ServerRpc();
        }
    }
    [ServerRpc]
    private void CreateTasks_ServerRpc()
    {
        Debug.Log("Creating Task");
        if (TaskCount >= maxTasks)
        {
            return;
        }
        Task_Multiplayer task = PickRandomTask();
        TaskStatus_Multiplayer taskStatus = Instantiate(activeTask_Prefab, activeTask_Container);
        NetworkObject netObject = taskStatus.gameObject.GetComponent<NetworkObject>();
        netObject.Spawn();
        netObject.TrySetParent(activeTask_Container);
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
    public void TaskCompleted_ServerRpc(int taskIndex)
    {
        DeleteTask_ServerRpc(taskIndex, true);
    }
    [ServerRpc(RequireOwnership = false)]
    public void TaskFailed_ServerRpc(int taskIndex)
    {
        DeleteTask_ServerRpc(taskIndex, false);
        EnergyManager_Multiplayer.Instance.ChangeEnergy_ServerRpc(-20f);
    }
    [ServerRpc]
    private void DeleteTask_ServerRpc(int taskIndex, bool wasTaskCompleted)
    {
        //Check if the task completed was the last task added and if false reduces every task index 
        //In front of it
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

        taskStatus.TaskReference.ClearTaskFromList();

        activeTasksStatusList.Remove(taskStatus);
        taskStatus.DeleteTaskStatus(wasTaskCompleted);
    }
}
