using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass : MonoBehaviour
{
    Vector3 startPosition;

    void Start () {
        startPosition = transform.position;
    }
    public void OutTheScene() {
        transform.Translate(new Vector3(0, 10f, 0));
    }

    public void InTheScene() {
        transform.position = startPosition;
    }
}
