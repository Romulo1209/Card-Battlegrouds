using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float timer = 2;
    private void Start() {
        StartCoroutine(Timer());
    }
    IEnumerator Timer() {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
