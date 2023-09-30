using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] WindowController windowController;
    // Este m�todo � chamado quando o objeto � criado.
    private void Start() {
        // Fecha todas as janelas controladas pelo WindowController.
        windowController.CloseAllWindows();
        // Abre a janela "Menu" usando o WindowController.
        windowController.OpenWindow("Menu");
    }

    // Este m�todo � chamado quando um bot�o de carregar n�vel � clicado.
    public void LoadLevel(string levelName) {
        // Carrega o n�vel especificado no argumento 'levelName'.
        SceneManager.LoadScene(levelName);
    }

    // Este m�todo � chamado quando um bot�o de sair do jogo � clicado.
    public void ExitGame() {
        // Encerra o aplicativo (neste caso, o jogo).
        Application.Quit();
    }
}