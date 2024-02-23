using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MelodyBehaviour : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    Transform player;
    Animator _animator;

    [SerializeField] float sightDistance = 10;
    [SerializeField] LayerMask layerMask;

    [SerializeField] Transform[] patrolPositions;


    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patrol());
    }
    private void Update()
    {
        Animate();
    }
    private void Animate()
    {
        _animator.SetFloat("Move", _navMeshAgent.velocity.magnitude/_navMeshAgent.speed);
    }
    private bool PlayerInSight()
    {
        if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit info, sightDistance, ~layerMask))
        {
            if (info.collider.CompareTag("Player"))
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    IEnumerator Patrol()
    {
        _navMeshAgent.SetDestination(patrolPositions[Random.Range(0, patrolPositions.Length)].position);
        while (true)
        {
            if (_navMeshAgent.remainingDistance < 1)
            {
                yield return new WaitForSeconds(Random.Range(1, 5));
                _navMeshAgent.SetDestination(patrolPositions[Random.Range(0, patrolPositions.Length)].position);
            }
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            if (PlayerInSight())
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}
