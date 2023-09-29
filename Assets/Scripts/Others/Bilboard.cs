using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour
{
    private void FixedUpdate() {
        Vector3 rotation = new Vector3(transform.position.y - Camera.main.transform.position.x, 0, 0);
        transform.LookAt(rotation);
    }
}
