using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameManager manager;
    public GameObject menu, loseWindow;

    public TMP_Text tryes, lastTry;
    public TMP_Dropdown difLevel;

    public void DifficultChanged(int difficult)
    {
        manager.ChangeDifficultLevel(difficult);
    }

    private void ChangeDropdownDif()
    {
        switch (manager.difficult)
        {
            case GameManager.DifficultLevelEnum.easy:
                difLevel.value = 0;
                break;
            case GameManager.DifficultLevelEnum.medium:
                difLevel.value = 1;
                break;
            case GameManager.DifficultLevelEnum.hard:
                difLevel.value = 2;
                break;
        }
    }
    public void SetMenuValues()
    {
        tryes.text = "Общее число попыток: " + manager.tryes.ToString();
        lastTry.text = "Время последней попытки: " + manager.gameTimer.ToString();
        ChangeDropdownDif();
    }
    public void DisableAllMenus()
    {
        menu.SetActive(false);
        loseWindow.SetActive(false);
    }
    
    public void LoseMenu()
    {
        SetMenuValues();
        loseWindow.SetActive(true);
    }
    public void StartButton()
    {
        StateMachine.instance.ChangeState(StateMachine.States.game);
    }
}
