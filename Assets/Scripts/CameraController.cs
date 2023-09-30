using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;  // Velocidade de movimento da câmera
    [SerializeField] Vector2 cameraLimit;  // Limites de movimento da câmera

    float movX;  // Armazena a direção do movimento da câmera no eixo horizontal

    private void Start() {
        // Define os limites da câmera com base nas dimensões da grade do jogo
        cameraLimit = new Vector2(0, GameController.instance.GridController.GridSize.x);
    }

    private void Update() {
        // Obtém a entrada do eixo horizontal (teclado ou controles)
        movX = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        // Verifica o estado atual do jogo; se estiver no estado "GameOver", a câmera não se move
        if (GameController.instance.ActualState == GameState.GameOver)
            return;

        // Verifica se há entrada de movimento ou se o cursor do mouse está próximo às bordas da tela
        if (movX != 0 || Input.mousePosition.x >= Screen.width * 0.975f || Input.mousePosition.x <= Screen.width * 0.025f) {
            if (movX > 0 || Input.mousePosition.x - Screen.width * 0.5 > 0 && movX == 0) {
                // Move a câmera para a direita se a entrada for positiva ou o cursor estiver à direita da tela
                // Verifica se a câmera não ultrapassa o limite superior
                if (transform.position.x >= cameraLimit.y)
                    return;
                Move(1);
            }
            else {
                // Move a câmera para a esquerda se a entrada for negativa ou o cursor estiver à esquerda da tela
                // Verifica se a câmera não ultrapassa o limite inferior
                if (transform.position.x <= cameraLimit.x)
                    return;
                Move(-1);
            }
        }
    }

    // Move a câmera na direção especificada
    public void Move(float direction) {
        // Usa Lerp para suavizar o movimento da câmera
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + direction, transform.position.y, transform.position.z), moveSpeed * Time.fixedDeltaTime);
    }
}
