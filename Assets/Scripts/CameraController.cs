using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 cameraLimit;

    float movX;

    private void Start() {
        cameraLimit = new Vector2(0, GameController.instance.GridController.GridSize.x);
    }
    private void Update() {
        movX = Input.GetAxis("Horizontal");
    }
    private void FixedUpdate() {
        if (GameController.instance.ActualState == GameState.GameOver)
            return;

        if (movX != 0 || Input.mousePosition.x >= Screen.width * 0.975f || Input.mousePosition.x <= Screen.width * 0.025f) {
            if (movX > 0 || Input.mousePosition.x - Screen.width * 0.5 > 0 && movX == 0) {
                if (transform.position.x >= cameraLimit.y)
                    return;
                Move(1);
            }
            else {
                if (transform.position.x <= cameraLimit.x)
                    return;
                Move(-1);
            }
        }
    }
    public void Move(float direction) {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + direction, transform.position.y, transform.position.z), moveSpeed * Time.fixedDeltaTime);
    }
}
