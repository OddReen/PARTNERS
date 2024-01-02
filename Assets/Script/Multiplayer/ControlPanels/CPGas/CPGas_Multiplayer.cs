using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CPGas_Multiplayer : NetworkBehaviour
{
    public static CPGas_Multiplayer Instance;

    [SerializeField] bool isShowingPattern = false;
    [SerializeField] float timeBetweenColors;
    [SerializeField] float timeColorOnScreen;
    [SerializeField] int patternQuantity;
    [SerializeField] int targetColorIndex;
    [SerializeField] string targetColorString;
    [SerializeField] GameObject[] colors;
    [SerializeField] List<GameObject> colorPattern;
    [SerializeField] GameObject showPatternButton;

    [SerializeField] SFX_List sFX_List;

    int[] colorIndexArray = new int[10];

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

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            RestartPatternMinigame();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ColorMiniGame_ServerRpc(string name)
    {
        if (isShowingPattern && !isBroken)
            return;

        if (name == showPatternButton.name)
        {
            CreatePattern_ServerRpc();
            return;
        }

        if (name == targetColorString)
        {
            Correct();
            targetColorIndex++;
            if (targetColorIndex == patternQuantity)
            {
                Fix();
                return;
            }
            targetColorString = colorPattern[targetColorIndex].name;
        }
        else
        {
            Wrong();
            RestartPatternMinigame();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreatePattern_ServerRpc()
    {
        RestartPatternMinigame();
        for (int i = 0; i < patternQuantity; i++)
        {
            int randomIndex = Random.Range(0, colors.Length);
            colorIndexArray[i] = randomIndex;
            colorPattern.Add(colors[colorIndexArray[i]]);
        }
        targetColorString = colorPattern[0].name;
        ShowPattern_ClientRpc(colorIndexArray);
    }

    [ClientRpc]
    private void ShowPattern_ClientRpc(int[] colorIndexArray)
    {
        StartCoroutine(ShowPattern(colorIndexArray));
    }
    IEnumerator ShowPattern(int[] colorIndexArray)
    {
        isShowingPattern = true;
        for (int i = 0; i < patternQuantity; i++)
        {
            colors[colorIndexArray[i]].SetActive(true);
            yield return new WaitForSeconds(timeColorOnScreen);
            colors[colorIndexArray[i]].SetActive(false);
            yield return new WaitForSeconds(timeBetweenColors);
        }
        isShowingPattern = false;
    }
    void RestartPatternMinigame()
    {
        targetColorString = "";
        colorPattern.Clear();
        targetColorIndex = 0;
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
    public void Break()
    {
        isBroken = true;
    }
}
