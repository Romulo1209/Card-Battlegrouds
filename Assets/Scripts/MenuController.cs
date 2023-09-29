using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] WindowController windowController;
    private void Start() {
        windowController.CloseAllWindows();
        windowController.OpenWindow("Menu");
    }

    public void LoadLevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
