using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalucinationManager_Temp : MonoBehaviour
{
    public static HalucinationManager_Temp Instance;
    //Pedir ao Leo para pegar automaticamente no player so this script wont be needed

    //As halucinaçoes ficarem durante o blackout inteiro foi feito a pressa depois se for necessario mudase

    //TODO Ask leo se é uma boa ideia adicionar variação no tempo que a halucinação demora a aparecer
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
    [SerializeField] float minTimeBetweenHalucinations;
    [SerializeField] float maxTimeBetweenHalucinations;
    [SerializeField] float maxTimeUnseen;

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
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        Rotation();
    }
    public void ActivateHalucination()
    {
        player = PlayerController_Multiplayer.OwnerInstance.gameObject;
        isHalucinating = true;
        StartCoroutine(Halucinating());
    }
    public void StopHalucination()
    {
        StopAllCoroutines();
        isHalucinating = false;
        if (halucinatingObjectInstance != null)
        {
            Destroy(halucinatingObjectInstance);
        }
    }
    //IEnumerator HalucinatingTime()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        if (isHalucinating == false)
    //        {
    //            StartCoroutine(Halucinating());
    //        }
    //    }
    //}
    IEnumerator Halucinating()
    {
        float timeUnseen = 0f;
        isHalucinating = true;
        //Begin
        _state = State.Begin;
        yield return new WaitForSeconds(beginOffSet);

        //Instantiate
        distanceTeleported = Random.Range(minDistanceTeleported, maxDistanceTeleported);
        Vector3 behindPlayer = player.transform.position - player.transform.forward * distanceTeleported;
        halucinatingObjectInstance = Instantiate(halucinatingObjectPrefab, behindPlayer, Quaternion.identity);

        while (!IsInView() && timeUnseen < maxTimeUnseen)
        {
            timeUnseen += Time.deltaTime;
            _state = State.WaitingToBeSighted;
            yield return new WaitForEndOfFrame();
        }

        _state = State.Sighted;
        yield return new WaitForSeconds(timeInView);
        isInView = false;
        Destroy(halucinatingObjectInstance);
        _state = State.NonActive;
        isHalucinating = false;
        float timeBetweenHalucinations = Random.Range(minTimeBetweenHalucinations, maxTimeBetweenHalucinations);
        yield return new WaitForSeconds(timeBetweenHalucinations);
        StartCoroutine(Halucinating());

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
