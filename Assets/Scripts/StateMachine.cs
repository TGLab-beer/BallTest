using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public GameManager gameManager;
    public MenuManager menuManager;

    public static StateMachine instance;

    void Start()
    {
        instance = this;
    }
    public enum States
    {
        game,
        lose,
        startMenu
    }

    public States state = States.startMenu;

    public void ChangeState(States newState)
    {
        if (newState == States.lose && state == States.game)
        {
            menuManager.LoseMenu();
            PoolManager.instance.ClearAll();
        }

        if (newState == States.game && (state == States.startMenu || state == States.lose))
        {
            gameManager.StartGame();
            menuManager.DisableAllMenus();
        }

        state = newState;
    }
}
