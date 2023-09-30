using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float timer = 2; // O per�odo de tempo em segundos antes da autodestrui��o

    private void Start() {
        // Quando o objeto � inicializado (no momento em que o script � ativado),
        // ele inicia uma rotina (coroutine) chamada "Timer".
        StartCoroutine(Timer());
    }

    // Coroutines s�o usadas para pausar a execu��o e esperar por um determinado per�odo de tempo.
    IEnumerator Timer() {
        // Essa linha pausa a execu��o da coroutine por "timer" segundos.
        yield return new WaitForSeconds(timer);
        // Ap�s o tempo especificado (no caso, "timer" segundos), o objeto � destru�do.
        Destroy(gameObject);
    }
}
