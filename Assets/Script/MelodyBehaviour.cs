using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MelodyBehaviour : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;

    [SerializeField] Transform[] patrolPositions;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        _navMeshAgent.SetDestination(patrolPositions[Random.Range(0, patrolPositions.Length)].position);
        while (true)
        {
            if (_navMeshAgent.remainingDistance < 1)
            {
                yield return new WaitForSeconds(Random.Range(1,5));
                _navMeshAgent.SetDestination(patrolPositions[Random.Range(0, patrolPositions.Length)].position);
            }
            yield return null;
        }
    }

}
