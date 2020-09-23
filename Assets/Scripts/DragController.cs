using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private Touch touch;
    private GameObject clone;
    private Vector3 positionOfScreen;
    private Vector3 bottlesPositionOfScreen;
    private Vector3 offsetValue;
    private bool isColorDragging = false;
    private bool isPillDragging = false;
    private bool isBottleDragging = false;
    private RaycastHit hit = new RaycastHit();
    private RaycastHit[] raycastHits = new RaycastHit[10];
    private Vector3 startedFrom = new Vector3();
    public static List<string> firstStepCheckList = new List<string>();

    void Update()
    {
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if(touch.phase == TouchPhase.Began) {
                raycastHits = Physics.RaycastAll(ray, Mathf.Infinity);
                foreach (var raycastHit in raycastHits)
                {
                    if (raycastHit.transform.tag == "Color") {
                        isColorDragging = true;
                        hit = raycastHit;
                        break;
                    }

                    if (raycastHit.transform.tag == "Pill") {
                        isPillDragging = true;
                        hit = raycastHit;
                        break;
                    }

                    if (raycastHit.transform.tag == "Bottle") {
                        isBottleDragging = true;
                        hit = raycastHit;
                        break;
                    }
                }
            }

            if (isPillDragging) {
                PillDragger(touch, ray, hit);
            }

            if (isColorDragging) {
                ColorDragger(touch, ray, hit);
            }

            if (isBottleDragging && Camera.main.GetComponent<CameraController>().movedToShelves) {
                BottleDragger(touch, ray, hit);
            }
        }
    }

    private void ColorDragger(Touch touch, Ray ray, RaycastHit raycastHit) {
        if (touch.phase == TouchPhase.Began) {
            Transform touchedTransform = raycastHit.transform;
            positionOfScreen = Camera.main.WorldToScreenPoint(raycastHit.transform.position);
            offsetValue = touchedTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, positionOfScreen.z));

            clone = Instantiate(touchedTransform.gameObject, touchedTransform.position, touchedTransform.rotation);
        }

        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
            Vector3 currentScreenSpace = new Vector3(touch.position.x, touch.position.y, positionOfScreen.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;

            clone.transform.position = currentPosition;
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++){
                RaycastHit hit = hits[i];

                if (hit.transform.gameObject.tag == "Bottle") {
                    GameObject cap = hit.transform.GetChild(0).gameObject;
                    Material capMaterial = cap.GetComponent<MeshRenderer>().sharedMaterial;
                    Material colorMaterial = clone.GetComponent<MeshRenderer>().sharedMaterial;

                    if (capMaterial == colorMaterial) {
                        GameObject body = hit.transform.GetChild(1).gameObject;
                        Material bodyMaterial = body.GetComponent<MeshRenderer>().material;
                        Material flippedBodyMaterial = body.transform.GetChild(0).GetComponent<MeshRenderer>().material;

                        Color color = capMaterial.color;
                        color.a = 0.4f;
                        if (!firstStepCheckList.Contains(body.name)) {
                            bodyMaterial.color = color;
                            flippedBodyMaterial.color = color;
                            firstStepCheckList.Add(body.name);
                            ScoreController.scoreValue += 3;
                        }

                        if (firstStepCheckList.Count == 3) {
                            GameObject firstPass = GameObject.Find("FirstPass");
                            firstPass.GetComponent<Pass>().OutTheScene();
                        }
                    } else ScoreController.scoreValue -= 3;
                }
            }
            isColorDragging = false;
            Destroy(clone);
        }
    }

    private void PillDragger(Touch touch, Ray ray, RaycastHit raycastHit) {
        if (touch.phase == TouchPhase.Began) {
            raycastHit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            
            GameObject Bottles = GameObject.Find("Bottles");
            bottlesPositionOfScreen = Camera.main.WorldToScreenPoint(Bottles.transform.position);
            positionOfScreen = Camera.main.WorldToScreenPoint(raycastHit.transform.position);
            offsetValue = raycastHit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, positionOfScreen.z));
            raycastHit.transform.rotation = Quaternion.Euler(0, 0, raycastHit.transform.name == "pill_2(Clone)" ? 90 : 0);

            if (raycastHit.transform.name == "pill_3(Clone)") 
                raycastHit.transform.localScale = new Vector3(50, 50, 50);
        }

        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
            Vector3 currentScreenSpace = new Vector3(touch.position.x, touch.position.y, positionOfScreen.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;
            currentPosition.z = -2.8f;

            raycastHit.transform.position = currentPosition;

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
               if (hit.transform.tag == "Bottle") {
                    raycastHit.transform.position = hit.transform.GetChild(2).transform.position; 
                    raycastHit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    isPillDragging = false;
                    return;            
               }
            }
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
            raycastHit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isPillDragging = false;
        }
    }

    private void BottleDragger(Touch touch, Ray ray, RaycastHit raycastHit) {
        if (touch.phase == TouchPhase.Began) {
            startedFrom = raycastHit.transform.position;
            positionOfScreen = Camera.main.WorldToScreenPoint(raycastHit.transform.position);

            offsetValue = raycastHit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, positionOfScreen.z));
        }

        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
            Vector3 currentScreenSpace = new Vector3(touch.position.x, touch.position.y, positionOfScreen.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;
            currentPosition.z = -2.8f;

            raycastHit.transform.position = currentPosition;
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
            isBottleDragging = false;

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform.tag == "Cell" && hit.collider.transform.childCount == 0) {
                    raycastHit.transform.GetChild(3).GetComponent<PillInsideChecker>().filled = false;
                    raycastHit.transform.GetChild(3).GetComponent<PillInsideChecker>().totalPillsInside = 0;
                    raycastHit.transform.gameObject.GetComponent<BottleHandler>().StackItself(hit.transform);
                } else {
                    raycastHit.transform.position = startedFrom;
                }
            }

            GameObject bottles = GameObject.Find("Bottles");
            if (bottles.transform.childCount == 0) {
                GameObject gameManager = GameObject.Find("GameManager");
                gameManager.GetComponent<GameManager>().ReInitializeRound();
            }
        }
    }
}
