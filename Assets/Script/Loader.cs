using Unity.Netcode;
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
        Test
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
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(),LoadSceneMode.Single);
        //Debug.Log("Loaded Scene" + targetScene.ToString());
    }
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
