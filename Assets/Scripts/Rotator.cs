using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 2;
    void Update() {
        RotateItself();
    }

    void RotateItself () {
        transform.Rotate(new Vector3(0, 0, -25 * speed * Time.deltaTime));
    }

}
