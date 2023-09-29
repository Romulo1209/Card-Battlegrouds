using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBase : MonoBehaviour {
    public string windowName;

    public void OpenWindow() {
        gameObject.SetActive(true);
    }

    public void CloseWindow() {
        gameObject.SetActive(false);
    }
}
