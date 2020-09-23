using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillInsideChecker : MonoBehaviour
{
    public int totalPillsInside = 0;
    public bool filled = false;

    void OnTriggerExit (Collider other) {
        if (other.transform.tag == "Pill") {
            totalPillsInside++;
            BottleHandler.catched = false;

            if (totalPillsInside >= 4) {
                filled = true;
            }
        }
    }
}
