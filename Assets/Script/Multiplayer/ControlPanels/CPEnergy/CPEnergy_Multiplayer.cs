using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CPEnergy_Multiplayer : MonoBehaviour
{
    public static CPEnergy_Multiplayer Instance;

    [SerializeField] int patternQuantity;
    [SerializeField] int targetArrowIndex = 0;
    [SerializeField] string targetArrowString;
    [SerializeField] Transform grid;
    [SerializeField] GameObject[] arrowPrefs;
    [SerializeField] List<GameObject> arrowPattern;

    [SerializeField] SFX_List sFX_List;

    bool isBroken = false;
    private void Start()
    {
        Instance = this;
        BlackoutManager_Multiplayer.Instance.StartBlackout += BlackoutManager_StartBlackout;
    }
    private void BlackoutManager_StartBlackout(object sender, System.EventArgs e)
    {
        Break();
    }
    [ServerRpc(RequireOwnership =false)]
    public void PatternMiniGame_ServerRpc(string button)
    {
        if (!isBroken)
        {
            return;
        }
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
    void Fix()
    {
        isBroken = false;
        BlackoutManager_Multiplayer.Instance.EndBlackOut_ServerRpc();
    }
    void Correct()
    {
        SFX_Manager_Multiplayer.Instance.PlaySoundLocal_ServerRpc(sFX_List.CPGasCorrect.Path, transform.position);
    }
    void Wrong()
    {
        SFX_Manager_Multiplayer.Instance.PlaySoundLocal_ServerRpc(sFX_List.CPGasWrong.Path, transform.position);
    }
    void Break()
    {
        isBroken = true;
        RestartPatternMinigame();
    }

}
