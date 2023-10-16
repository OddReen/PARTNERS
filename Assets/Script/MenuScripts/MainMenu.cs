using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    Stack<GameObject> navigationStack = new Stack<GameObject>();
    [SerializeField] GameObject mainMenu;
    private void Start()
    {
        navigationStack.Push(mainMenu);
    }
    public void GoToPreviousMenu()
    {
        navigationStack.Pop().SetActive(false);
        navigationStack.Peek().SetActive(true);
    }
    public void GoToMenu(GameObject menu)
    {
        Debug.Log("Moved to " + menu.name);
        navigationStack.Peek().SetActive(false);
        menu.SetActive(true);
        navigationStack.Push(menu);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void DebugCloseMenu()
    {
        gameObject.SetActive(false);
    }
}
