using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] WindowController windowController;
    // Este método é chamado quando o objeto é criado.
    private void Start() {
        // Fecha todas as janelas controladas pelo WindowController.
        windowController.CloseAllWindows();
        // Abre a janela "Menu" usando o WindowController.
        windowController.OpenWindow("Menu");
    }

    // Este método é chamado quando um botão de carregar nível é clicado.
    public void LoadLevel(string levelName) {
        // Carrega o nível especificado no argumento 'levelName'.
        SceneManager.LoadScene(levelName);
    }

    // Este método é chamado quando um botão de sair do jogo é clicado.
    public void ExitGame() {
        // Encerra o aplicativo (neste caso, o jogo).
        Application.Quit();
    }
}