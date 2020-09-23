using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleHandler : MonoBehaviour
{
    public static bool catched = false;
    private bool closed = false;
    GameObject cap;
    Transform capPoint;

    void Start () {
        cap = transform.GetChild(0).gameObject;
        capPoint = transform.GetChild(4);
    }

    void Update () {
        if (transform.GetChild(3).GetComponent<PillInsideChecker>().filled && !closed) {
            StartCoroutine(CloseTheCap(transform, cap));
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Cell") {
            cap.transform.position = transform.GetChild(4).position;
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.name == "FourthPass" || other.name == "FifthPass" || other.tag == "Cell" || other.tag == "Untagged") return;

        if (transform.GetChild(3).GetComponent<PillInsideChecker>().filled) {
            StartCoroutine(SetKinematicAllChilds(transform));
            cap.transform.position = transform.GetChild(2).position;
        }

        if (!catched && other.tag == "Pill" && !transform.GetChild(3).GetComponent<PillInsideChecker>().filled) {
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.position = transform.GetChild(2).position;
            Transform parent = transform.GetChild(1).transform.GetChild(0).transform;
            other.transform.parent = parent;

            Material otherMaterial = other.GetComponent<MeshRenderer>().sharedMaterial;
            Material thisMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;
            if (otherMaterial.color == thisMaterial.color)
                ScoreController.scoreValue += 3;
            else ScoreController.scoreValue -= 3;
            catched = true;
        }
    }
    IEnumerator CloseTheCap (Transform transform, GameObject cap) {
        yield return new WaitForSeconds(1);
        cap.SetActive(true);

        float step = 2 * Time.deltaTime;
        cap.transform.position = Vector3.MoveTowards(cap.transform.position, new Vector3(capPoint.position.x, capPoint.position.y, capPoint.position.z), step);
        if (cap.transform.position.y <= capPoint.transform.position.y) 
            closed = true;
    }

    IEnumerator SetKinematicAllChilds (Transform parent) {
        yield return new WaitForSeconds(1);
        SetKinematicAllChildsRecursion(parent);
    }

    private void SetKinematicAllChildsRecursion(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if(child.gameObject.GetComponent<Rigidbody>() != null) {
                child.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                child.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
            SetKinematicAllChildsRecursion(child);
        }
    }

    public void StackItself(Transform cell){
        Vector3 scaleChange;
        transform.parent = cell.transform;

        if (gameObject.name == "bottle_1") {
            transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y - 0.7f, cell.transform.position.z);
            scaleChange = new Vector3(-10f, -10f, -10f);
            transform.localScale += scaleChange;

            if (cell.parent.name == "ThirdRow") ScoreController.scoreValue += 3;
            else ScoreController.scoreValue -= 3;
        }

        if (gameObject.name == "bottle_2") {
            transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y - 0.7f, cell.transform.position.z);
            scaleChange = new Vector3(-10f, -10f, -10f);
            transform.localScale += scaleChange;

            if (cell.parent.name == "SecondRow") ScoreController.scoreValue += 3;
            else ScoreController.scoreValue -= 3;
        }

        if (gameObject.name == "bottle_3") {
            transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y - 0.3f, cell.transform.position.z);
            scaleChange = new Vector3(-8f, -8f, -8f);
            transform.localScale += scaleChange;

            if (cell.parent.name == "FirstRow") ScoreController.scoreValue += 3;
            else ScoreController.scoreValue -= 3;
        }

        ChangeNamesAndTagsAsStacked(transform);
    }

    public void ChangeNamesAndTagsAsStacked(Transform cell){
        transform.name = transform.name + "_stacked";
        transform.tag = "Stacked";

        foreach (Transform item in transform)
        {
            item.name = item.name + "_stacked";
            item.tag = "Stacked";
        }

        foreach (Transform item in transform.GetChild(1).GetChild(0))
        {
            item.name = item.name + "_stacked";
            item.tag = "Stacked";
        }
    }
}
