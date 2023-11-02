using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class InGameManager : NetworkBehaviour
{
    public static InGameManager Instance;
    [Header("Player Spawn")]
    [SerializeField] Transform playerPrefab;
    [SerializeField] Transform[] spawnPosition;

    [Header("Menus/UI")]
    [SerializeField] MenuPause pauseMenu;
    [SerializeField] GameObject interactionHint;

    //Players Controllers used when i figure out how lol
    //MultiplayerPlayerController[] playerControllers; 

    bool isPaused = false;

    private void Awake()
    {
        //May take an L
        Instance = this;
    }
    //Beware the dumbass who doesnt read the docs
    //Qualquer func do Netcode que seja necessaria no awake deve ser metida dentro de OnNetworkSpawn
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            //playerControllers = new MultiplayerPlayerController[MultiplayerManager.Instance.MaxPlayers];
        }
    }

    private void MultiplayerPlayerInput_PauseAction(object sender, System.EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Confined;
            pauseMenu.Show();
            MultiplayerPlayerInput.Instance.SwitchToPauseMap();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.Hide();
            MultiplayerPlayerInput.Instance.SwitchToGameplayMap();
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        //Debug.Log("GameManager Start Player Spawn");
        //Client ids começam a 0
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            //Debug.Log("Spawned "+clientId+" Player");
            Transform playerTransform = Instantiate(playerPrefab);
            //playerControllers[clientId] = playerController;
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            playerTransform.transform.position = spawnPosition[clientId].position;
        }
        PlayerInitializationClientRpc();
    }
    [ClientRpc]
    private void PlayerInitializationClientRpc()
    {
        MultiplayerPlayerInput.Instance.PauseAction += MultiplayerPlayerInput_PauseAction;

    }
}
