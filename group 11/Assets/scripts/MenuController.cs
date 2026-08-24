using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

//This script took the longest and was the hardest to write because everytime I clicked on the mouse 

public class MenuController : MonoBehaviour
{
    private enum MenuState { None, Paused, Home, Restart }
    private MenuState currentMenuState = MenuState.None;

    void Update()
    {
        // Left mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleMenuAction();
        }

        // East button on gamepad 
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            HandleMenuAction();
        }
    }

    private void HandleMenuAction()
    {
        switch (currentMenuState)
        {
            case MenuState.Paused:
                Debug.Log("Paused: Showing pause options");
                break;
            case MenuState.Home:
                Debug.Log("Home: Going to home screen");
                break;
            case MenuState.Restart:
                Debug.Log("Restart: Restarting the game");
                break;
        }
    }

    // Methods to change state
    public void SetStateToPaused()
    {
        currentMenuState = MenuState.Paused;
    }

    public void SetStateToHome()
    {
        currentMenuState = MenuState.Home;
    }

    public void SetStateToRestart()
    {
        currentMenuState = MenuState.Restart;
    }
}

