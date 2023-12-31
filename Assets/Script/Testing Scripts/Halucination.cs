using System.Collections;
using UnityEngine;

public class Halucination : MonoBehaviour
{
    [Header("Gizmo")]
    [SerializeField] bool gizmos;

    [Header("Booleans")]
    [SerializeField] bool isInView;
    [SerializeField] bool canTeleport;
    [SerializeField] bool isHalucinating = false;

    [Header("Timers")]
    [SerializeField] float halucinatingTime;
    [SerializeField] float timeInView;
    [SerializeField] float beginOffSet;

    [Header("Teleport")]
    [SerializeField] float distanceTeleported;
    [SerializeField] float minDistanceTeleported;
    [SerializeField] float maxDistanceTeleported;

    [Header("GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject halucinatingObjectPrefab;
    [SerializeField] GameObject halucinatingObjectInstance;

    [SerializeField] State _state;
    enum State
    {
        NonActive,
        Begin,
        WaitingToBeSighted,
        Sighted,
    }
    private void Update()
    {
        Rotation();
        if (Input.GetKeyDown(KeyCode.J) && !isHalucinating)
            ActivateHalucination();
    }
    void ActivateHalucination()
    {
        isHalucinating = true;
        StartCoroutine(HalucinatingTime());
        StartCoroutine(Halucinating());
    }
    IEnumerator HalucinatingTime()
    {
        yield return new WaitForSeconds(halucinatingTime);
        isHalucinating = false;
    }
    IEnumerator Halucinating()
    {
        //Begin
        _state = State.Begin;
        yield return new WaitForSeconds(beginOffSet);

        //Instantiate
        distanceTeleported = Random.Range(minDistanceTeleported, maxDistanceTeleported);
        Vector3 behindPlayer = player.transform.position - player.transform.forward * distanceTeleported;
        halucinatingObjectInstance = Instantiate(halucinatingObjectPrefab, behindPlayer, Quaternion.identity);

        while (!IsInView())
        {
            _state = State.WaitingToBeSighted;
            yield return null;
        }
        if (IsInView())
        {
            _state = State.Sighted;
            yield return new WaitForSeconds(timeInView);
            isInView = false;
            Destroy(halucinatingObjectInstance);
            _state = State.NonActive;
        }
    }
    void Rotation()
    {
        if (halucinatingObjectInstance != null)
        {
            halucinatingObjectInstance.transform.LookAt(player.transform.position, transform.up);
        }
    }
    bool IsInView()
    {
        float dotProduct = Vector3.Dot(player.transform.forward, (halucinatingObjectInstance.transform.position - player.transform.position).normalized);
        isInView = dotProduct > Mathf.InverseLerp(0, 179, Camera.main.fieldOfView);
        return isInView;
    }
    private void OnDrawGizmos()
    {
        if (gizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Camera.main.transform.position, Quaternion.AngleAxis(180 * Mathf.InverseLerp(0, 179, Camera.main.fieldOfView), Vector3.up) * Camera.main.transform.forward.normalized * 5 + Camera.main.transform.position);
            Gizmos.DrawLine(Camera.main.transform.position, Quaternion.AngleAxis(180 * -Mathf.InverseLerp(0, 179, Camera.main.fieldOfView), Vector3.up) * Camera.main.transform.forward.normalized * 5 + Camera.main.transform.position);

            Gizmos.color = Color.green;
            Vector3 playerBackNormalized = player.transform.forward.normalized;
            Gizmos.DrawSphere(new Vector3(player.transform.position.x - playerBackNormalized.x * distanceTeleported, player.transform.position.y, player.transform.position.z - playerBackNormalized.z * distanceTeleported), .5f);

            if (halucinatingObjectInstance != null)
            {
                Gizmos.color = IsInView() ? Color.green : Color.red;
                Gizmos.DrawLine(player.transform.position, halucinatingObjectInstance.transform.position);
            }
        }
    }
}
