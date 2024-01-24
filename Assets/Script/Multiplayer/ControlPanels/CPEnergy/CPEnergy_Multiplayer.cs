using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using FMODUnity;

public class CPEnergy_Multiplayer : NetworkBehaviour
{
    //HMMMMM multiplayer is fucky mas acho que 
    public static CPEnergy_Multiplayer Instance;

    [SerializeField] int patternQuantity;
    [SerializeField] int targetArrowIndex = 0;
    [SerializeField] string targetArrowString;
    [SerializeField] Transform grid;
    [SerializeField] GameObject[] arrowPrefs;
    [SerializeField] List<GameObject> arrowPattern;

    //Cant use this eventReference.Path so funciona no editor por alguma razão de deus
    //se eu encontrar quem escreveu os docs do Fmod someone gonna die
    [SerializeField] SFX_List sFX_List;

    [SerializeField] VideoController panelVideo;

    bool isBroken = false;
    private void Start()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        BlackoutManager_Multiplayer.Instance.StartBlackout += BlackoutManager_StartBlackout;
        BlackoutManager_Multiplayer.Instance.EndBlackout += BlackoutManager_EndBlackout;
    }
    private void BlackoutManager_EndBlackout(object sender, System.EventArgs e)
    {
        Standby();
    }

    private void BlackoutManager_StartBlackout(object sender, System.EventArgs e)
    {
        Break();
    }
    [ServerRpc(RequireOwnership = false)]
    public void PatternMiniGame_ServerRpc(string button)
    {
        if (!isBroken) return;

        SFX_Manager_Multiplayer.Instance.PlaySoundLocal_ServerRpc(sFX_List.ControlPanelClickPath, transform.position);
        bool correctButton = button == targetArrowString;
        if (correctButton)
        {
            Correct();
            targetArrowIndex++;
            if (targetArrowIndex == patternQuantity)
            {
                Fix();
                RestartPatternMinigame();
                return;
            }
            targetArrowString = arrowPattern[targetArrowIndex].name;
        }
        else
        {
            Wrong();
            RestartPatternMinigame();
        }
    }
    void RestartPatternMinigame()
    {
        for (int i = 0; i < arrowPattern.Count; i++)
        {
            Destroy(arrowPattern[i]);
        }
        arrowPattern.Clear();
        for (int i = 0; i < patternQuantity; i++)
        {
            int randomNum = Random.Range(0, arrowPrefs.Length);
            CreateArrows_ClientRpc(randomNum);
        }
        targetArrowIndex = 0;
        targetArrowString = arrowPattern[targetArrowIndex].name;
    }
    [ClientRpc]
    void CreateArrows_ClientRpc(int randomNum)
    {
        GameObject gameObject = Instantiate(arrowPrefs[randomNum], grid);
        gameObject.name = arrowPrefs[randomNum].name;

        arrowPattern.Add(gameObject);
    }
    void Standby()
    {
        panelVideo.ChangeVideo_ClientRpc(0);
    }
    void Fix()
    {
        panelVideo.ChangeVideo_ClientRpc(3);
        isBroken = false;
        BlackoutManager_Multiplayer.Instance.EndBlackOut_ServerRpc();
    }
    void Correct()
    {
        panelVideo.ChangeVideo_ClientRpc(2);
        SFX_Manager_Multiplayer.Instance.PlaySoundLocal_ServerRpc(sFX_List.ControlPanelCorrectPath, transform.position);
    }
    void Wrong()
    {
        panelVideo.ChangeVideo_ClientRpc(1);
        SFX_Manager_Multiplayer.Instance.PlaySoundLocal_ServerRpc(sFX_List.ControlPanelWrongPath, transform.position);
    }

    void Break()
    {
        panelVideo.ChangeVideo_ClientRpc(1);
        isBroken = true;
        RestartPatternMinigame();
    }

}
