using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{

    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;

    private void Start() {
        if (SaveManager.instance.HasSavedData() == false) {
            continueButton.SetActive(false);
        }
    }
    
    public void ContinueGame() {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame() {
        SceneManager.LoadScene(sceneName);
        SaveManager.instance.DeleteSavedData();
    }

    public void ExitGame() {
        // Application.Quit();
        Debug.Log("Quit");
    }

}
