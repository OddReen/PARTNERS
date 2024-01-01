using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //Maybe change this to something else isto não vai funcionar se tivermos mais do que um menu na scene 
    //Não deve acontecer but you never know
    public static Menu Instance;
    //Main script usado para navegação de menus 
    protected Stack<UIAutoAnimation> navigationStack = new Stack<UIAutoAnimation>();
    [SerializeField] UIAutoAnimation firstMenu;
    UIAutoAnimation exitingMenu;
    private void Start()
    {
        VirtualStart();
    }
    protected virtual void VirtualStart()
    {
        Instance = this;
        navigationStack.Push(firstMenu);
    }
    public void GoToPreviousMenu()
    {
        UIAutoAnimation currentMenu = navigationStack.Pop();
        currentMenu.ExitAnimation();
        exitingMenu = currentMenu;
    }
    public void DeactivateExitingMenu()
    {
        exitingMenu.gameObject.SetActive(false);
        UIAutoAnimation previousMenu = navigationStack.Peek();
        previousMenu.gameObject.SetActive(true);
    }
    public void GoToNextMenu(UIAutoAnimation menu)
    {
        exitingMenu = navigationStack.Peek();
        exitingMenu.ExitAnimation();
        navigationStack.Push(menu);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
