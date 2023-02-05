using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public GameObject helpPopup;
    public Button goBack;
    public GameManager gameManager;
    
    public void PlayGame()
    {
        gameManager.GoToNextScene();
    }

    public void Help()
    {
        helpPopup.SetActive(true);
    }

    public void CloseHelp()
    {
        helpPopup.SetActive(false);
    }
}
