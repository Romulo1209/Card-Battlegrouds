using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBase : MonoBehaviour
{
    public string windowName; // Nome da janela, pode ser usado para identificar a janela

    public void OpenWindow() {
        gameObject.SetActive(true); // Ativa o objeto (janela) associado a esta classe
    }

    public void CloseWindow() {
        gameObject.SetActive(false); // Desativa o objeto (janela) associado a esta classe
    }
}
