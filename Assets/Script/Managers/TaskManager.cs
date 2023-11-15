using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TaskManager : NetworkBehaviour
{
    public static TaskManager Instance;

    //Não tem maneira de dar scroll no ui de task e 7 é o numero maximo que cabe se for mais ele fica fora do UI
    const int maxTasks = 7;
    int TaskCount { get { return activeTasksStatusList.Count; } }

    [SerializeField] List<Task> possibleTasksList;

    List<TaskStatus> activeTasksStatusList = new List<TaskStatus>();

    [SerializeField] float timeToCreateTask;

    [SerializeField] TaskStatus activeTask_Prefab;
    [SerializeField] Transform activeTask_Container;
    public override void OnNetworkSpawn()
    {

        Instance = this;
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
        Task task = PickRandomTask();
        TaskStatus taskStatus = Instantiate(activeTask_Prefab, activeTask_Container);
        taskStatus.AssignTask(task, TaskCount);
        activeTasksStatusList.Add(taskStatus);
        task.ActivateTask(taskStatus);
    }
    private Task PickRandomTask()
    {
        //Se não existirem quest que se possam repetir ele eventualmente da um erro por não haver mais tasks para criar
        //No jogo em si isto não devo ser um problema visto termos sempre uma quest repetivel mas se acontecer fica aqui marcado para dar fix
        int randomIndex = Random.Range(0, possibleTasksList.Count);
        Task Task = possibleTasksList[randomIndex];

        if (Task.IsRepeatable == false)
        {
            possibleTasksList.Remove(Task);
        }
        return Task;
    }
    [ServerRpc(RequireOwnership = false)]
    public void TaskCompletedServerRpc(int taskIndex)
    {
        DeleteTaskClientRpc(taskIndex);
    }
    [ServerRpc(RequireOwnership = false)]
    public void TaskFailedServerRpc(int taskIndex)
    {
        DeleteTaskClientRpc(taskIndex);
        //Energy bar diminuir ou modo de night time
    }
    [ClientRpc]
    private void DeleteTaskClientRpc(int taskIndex)
    {
        if (taskIndex + 1 < TaskCount)
        {
            for (int i = taskIndex + 1; i < TaskCount; i++)
            {
                activeTasksStatusList[i].ReduceIndex();
            }
        }

        TaskStatus taskStatus = activeTasksStatusList[taskIndex];
        if (taskStatus.TaskReference.IsRepeatable == false)
        {
            possibleTasksList.Add(taskStatus.TaskReference);
        }

        activeTasksStatusList.Remove(taskStatus);
        Destroy(taskStatus.gameObject);
    }
}
