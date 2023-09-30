using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] List<WindowBase> windows; // Uma lista de janelas que podem ser gerenciadas

    #region Window Management

    public void CloseAllWindows() {
        foreach (WindowBase window in windows) {
            window.CloseWindow(); // Fecha todas as janelas na lista
        }
    }

    public void OpenWindow(string windowName) {
        CloseAllWindows(); // Fecha todas as janelas antes de abrir uma específica
        foreach (WindowBase window in windows) {
            if (window.windowName == windowName) {
                window.OpenWindow(); // Abre a janela com o nome correspondente
                return;
            }
        }
    }

    #endregion
}
