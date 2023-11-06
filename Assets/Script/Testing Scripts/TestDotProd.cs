using UnityEngine;

public class TestDotProd : MonoBehaviour
{
    [SerializeField] bool gizmos;

    [SerializeField] bool isInView;
    [SerializeField] bool canTeleport;

    [Range(-1, 1)]
    [SerializeField] float fieldOfView;
    [SerializeField] float distanceTeleported;
    [SerializeField] float minDistanceTeleported;
    [SerializeField] float maxDistanceTeleported;

    [SerializeField] GameObject player;
    [SerializeField] GameObject jumpScareObject;

    private void Update()
    {
        Rotation();
        DotProd();
        JumpScare();
    }
    void DotProd()
    {
        float dotProduct = Vector3.Dot(player.transform.forward, (jumpScareObject.transform.position - player.transform.position).normalized);
        isInView = dotProduct > fieldOfView;
    }
    void JumpScare()
    {
        if (isInView)
        {
            canTeleport = true;
        }
        else if (canTeleport)
        {
            distanceTeleported = Random.Range(minDistanceTeleported, maxDistanceTeleported);
            canTeleport = false;
            jumpScareObject.transform.position = player.transform.position + (player.transform.position - jumpScareObject.transform.position).normalized * distanceTeleported;
        }
    }
    void Rotation()
    {
        transform.LookAt(player.transform.position, transform.up);
    }
    private void OnDrawGizmos()
    {
        if (gizmos)
        {
            Gizmos.color = Color.green;
            Vector3 playerBackNormalized = -player.transform.forward.normalized;
            Gizmos.DrawSphere(new Vector3(player.transform.position.x + (playerBackNormalized.x * distanceTeleported), player.transform.position.y, player.transform.position.z + (playerBackNormalized.z * distanceTeleported)), .5f);

            Gizmos.DrawLine(player.transform.position, jumpScareObject.transform.position);
            Gizmos.DrawLine(player.transform.position, player.transform.position + playerBackNormalized * -5);
        }
    }
}
