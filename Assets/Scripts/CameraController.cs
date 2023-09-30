using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;  // Velocidade de movimento da c�mera
    [SerializeField] Vector2 cameraLimit;  // Limites de movimento da c�mera

    float movX;  // Armazena a dire��o do movimento da c�mera no eixo horizontal

    private void Start() {
        // Define os limites da c�mera com base nas dimens�es da grade do jogo
        cameraLimit = new Vector2(0, GameController.instance.GridController.GridSize.x);
    }

    private void Update() {
        // Obt�m a entrada do eixo horizontal (teclado ou controles)
        movX = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        // Verifica o estado atual do jogo; se estiver no estado "GameOver", a c�mera n�o se move
        if (GameController.instance.ActualState == GameState.GameOver)
            return;

        // Verifica se h� entrada de movimento ou se o cursor do mouse est� pr�ximo �s bordas da tela
        if (movX != 0 || Input.mousePosition.x >= Screen.width * 0.975f || Input.mousePosition.x <= Screen.width * 0.025f) {
            if (movX > 0 || Input.mousePosition.x - Screen.width * 0.5 > 0 && movX == 0) {
                // Move a c�mera para a direita se a entrada for positiva ou o cursor estiver � direita da tela
                // Verifica se a c�mera n�o ultrapassa o limite superior
                if (transform.position.x >= cameraLimit.y)
                    return;
                Move(1);
            }
            else {
                // Move a c�mera para a esquerda se a entrada for negativa ou o cursor estiver � esquerda da tela
                // Verifica se a c�mera n�o ultrapassa o limite inferior
                if (transform.position.x <= cameraLimit.x)
                    return;
                Move(-1);
            }
        }
    }

    // Move a c�mera na dire��o especificada
    public void Move(float direction) {
        // Usa Lerp para suavizar o movimento da c�mera
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + direction, transform.position.y, transform.position.z), moveSpeed * Time.fixedDeltaTime);
    }
}
