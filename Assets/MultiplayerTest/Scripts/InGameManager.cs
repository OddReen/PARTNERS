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
            MultiplayerPlayerInput.OwnerInstance.SwitchToPauseMap();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.Hide();
            MultiplayerPlayerInput.OwnerInstance.SwitchToGameplayMap();
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        //Client ids começam a 0
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            //MODIFICAR A POSIÇÃO DIRETAMENTE DEPOIS DE ELE SPAWNAR MANDAO PARA UMA COORDENADA DIFERENTE QUE A METIDA WHY?? FUCK IF I KNOW
            //Spawnar na posição coreta logo no instantiate em vez de modificar depois 
            Transform playerTransform = Instantiate(playerPrefab, spawnPosition[clientId].position, Quaternion.Euler(0,-90,0));
           
            //Eu sei que posso pegar diretamente sem ter que estar a dar assign a uma variavel mas quando meti diferente deu um erro e depois desapareceu 
            //Deixei assim para respeitar os sinais que deus mandou
            NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();
            networkObject.SpawnAsPlayerObject(clientId, true);
        }
        PlayerInitializationClientRpc();
    }
    [ClientRpc]
    private void PlayerInitializationClientRpc()
    {
        MultiplayerPlayerInput.OwnerInstance.PauseAction += MultiplayerPlayerInput_PauseAction;

    }
}
