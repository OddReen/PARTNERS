using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //Main script usado para navegação de menus 
    protected Stack<GameObject> navigationStack = new Stack<GameObject>();
    [SerializeField] GameObject firstMenu;
    private void Start()
    {
        VirtualStart();
    }
    protected virtual void VirtualStart()
    {
        navigationStack.Push(firstMenu);
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
}
