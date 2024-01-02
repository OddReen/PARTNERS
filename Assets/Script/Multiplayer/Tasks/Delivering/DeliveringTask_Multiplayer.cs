using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveringTask_Multiplayer : Task_Multiplayer
{
    public static DeliveringTask_Multiplayer Instance;
    [SerializeField] DeliverObject_Multiplayer deliverObjectPrefab;
    [SerializeField] Transform deliverSpawnLocation;
    [SerializeField] Transform deliveryLocation;

    private void Awake()
    {
        Instance = this;
    }
    public override void ActivateTask(TaskStatus_Multiplayer taskInfo)
    {
        base.ActivateTask(taskInfo);
        DeliverObject_Multiplayer temp = Instantiate(deliverObjectPrefab);
        temp.transform.position = deliverSpawnLocation.position;
        temp.AssignDeliveryDestination(deliveryLocation);
    }
    public void Deliver()
    {
        CompleteTask();
    }
    public override void FailTask()
    {
        base.FailTask();
    }
}
