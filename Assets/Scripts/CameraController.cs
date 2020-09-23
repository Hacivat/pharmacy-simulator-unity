using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    private Transform target;
    private GameObject bottles;
    public bool movedToPetri = false;
    public bool movedToShelves = false;

    void Start () {
    }
    void Update () {
        if (DragController.firstStepCheckList.Count == 3 && !movedToPetri) {
            StartCoroutine(moveToPetri());
        }

        bottles = GameObject.Find("Bottles");
        if (bottles && bottles.GetComponent<BottlesController>().ThreeBottlesAreFilled() && !movedToShelves) {
            StartCoroutine(moveToShelves());
        }
    }
    IEnumerator moveToPetri() {
        yield return new WaitForSeconds(1);

        target = GameObject.Find("Petri").gameObject.transform;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), 0.1f);
        if ((transform.position.x - target.position.x) > -0.001f) {
            movedToPetri = true;
            transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

            GameObject secondPass = GameObject.Find("SecondPass");
            secondPass.GetComponent<Pass>().OutTheScene();
        }
    }
    IEnumerator moveToShelves () {
        yield return new WaitForSeconds(3);

        target = GameObject.Find("Shelves").gameObject.transform;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), 0.1f);
        if ((transform.position.x - target.position.x) > -0.001f) {
            movedToShelves = true;
            transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

            GameObject fourthPass = GameObject.Find("FourthPass");
            fourthPass.GetComponent<Pass>().OutTheScene();
        }
    }
}
