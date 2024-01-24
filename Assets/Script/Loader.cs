using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu,     
        MenuRelay,
        MenuLAN,
        MenuLobby,
        LoadingScene,
        MultiplayerTest,
        Test,
        Tutorial,
        FmodTest
    }

    private static Scene targetScene;
    //Da load a scenes singleplayer
    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    //Da load a scenes multiplayer
    public static void LoadNetwork(Scene targetScene)
    {
        Debug.Log("Loading Scene" + targetScene.ToString());
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(),LoadSceneMode.Single);
        Debug.Log("Scene Loaded" + targetScene.ToString());
    }
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
