using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject newGameButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    public void ExitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
