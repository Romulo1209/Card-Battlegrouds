using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float timer = 2; // O período de tempo em segundos antes da autodestruição

    private void Start() {
        // Quando o objeto é inicializado (no momento em que o script é ativado),
        // ele inicia uma rotina (coroutine) chamada "Timer".
        StartCoroutine(Timer());
    }

    // Coroutines são usadas para pausar a execução e esperar por um determinado período de tempo.
    IEnumerator Timer() {
        // Essa linha pausa a execução da coroutine por "timer" segundos.
        yield return new WaitForSeconds(timer);
        // Após o tempo especificado (no caso, "timer" segundos), o objeto é destruído.
        Destroy(gameObject);
    }
}
