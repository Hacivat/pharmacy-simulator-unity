using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPillsDrop : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Pill") {
            ScoreController.scoreValue -= 3;
            GetBackDroppedPill(other.transform);
            Destroy(other);
        }
    }

    void GetBackDroppedPill (Transform pillTransform) {
        GameObject pillDropper = GameObject.Find("PillsDropPoint");

        if (pillTransform.name.Substring(0, 6) == "pill_3")
            pillTransform.localScale = new Vector3(90, 90, 90);

        GameObject newPill = Instantiate(pillTransform.gameObject, pillDropper.transform.position, Quaternion.identity);
        newPill.GetComponent<Rigidbody>().isKinematic = false;
        newPill .name = newPill.name.Substring(0, 13);
    }
}
