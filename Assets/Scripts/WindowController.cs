using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] List<WindowBase> windows;

    #region Window Management

    public void CloseAllWindows() {
        foreach (WindowBase window in windows) {
            window.CloseWindow();
        }
    }

    public void OpenWindow(string windowName) {
        CloseAllWindows();
        foreach (WindowBase window in windows) {
            if (window.windowName == windowName) {
                window.OpenWindow();
                return;
            }
        }
    }

    #endregion
}
