using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BottlesController : MonoBehaviour
{
    public float speed;
    public List<Material> materials;
    GameObject bottle_1_cap;
    GameObject bottle_2_cap;
    GameObject bottle_3_cap;
    private bool isMoving = true;
    private const float PIVOT_DISTANCE = 1.6f;
    private bool inFourthPass = false;
    private Vector3[] mainCapPositions = new Vector3[3];

    void Awake()
    {
        DoItWhenReinitialized();
    }

    void Update()
    {
         if (ThreeBottlesAreFilled() && !inFourthPass) {
            StartCoroutine(PrepareAndGoFourthPass());
        }

        if (isMoving) {
            float step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(70f, transform.position.y, transform.position.z), step);
        }
    }

    public void DoItWhenReinitialized () {
        bottle_1_cap = transform.GetChild(0).GetChild(0).gameObject;
        bottle_2_cap = transform.GetChild(1).GetChild(0).gameObject;
        bottle_3_cap = transform.GetChild(2).GetChild(0).gameObject;
        RandomizeCaps();
    }

    private IEnumerator PrepareAndGoFourthPass() {
        yield return new WaitForSeconds(2);

        GameObject thirdPass = GameObject.Find("ThirdPass");
        thirdPass.GetComponent<Pass>().OutTheScene();

        inFourthPass = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pass") {
            if (other.name == "SecondPass") {
                PrepareSecondStepForBottles();
            }

            if (other.name == "ThirdPass") {
                GameObject pillsDropPoint;
                pillsDropPoint = GameObject.Find("PillsDropPoint");
                pillsDropPoint.GetComponent<PillDropper>().DropPills();
            }

            if (other.name == "FourthPass") {
                PrepareThirdStepForBottles(transform);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pass") {
            if (other.gameObject.activeSelf && transform.position.x - PIVOT_DISTANCE >= other.transform.position.x) {
                transform.position = new Vector3(other.transform.position.x + PIVOT_DISTANCE ,transform.position.y ,transform.position.z);
                isMoving = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pass") {

            if (other.name == "FifthPass") {
                return;
            }

            if (other.name == "SecondPass") {
                ReadyToSecondStepForBottles();
            }

            if (other.name == "FourthPass") {
                ReadyToThirdStepForBottles(transform);
            }

            isMoving = true;
        }
    }

    private void PrepareSecondStepForBottles()
    {
        foreach (Transform child in transform)
        {
            child.GetChild(0).transform.position = child.GetChild(2).transform.position;
            foreach (Transform item in child)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void ReadyToSecondStepForBottles()
    {
        foreach (Transform child in transform)
        {
            bool isCap = true;
            foreach (Transform item in child)
            {
                if (isCap) { isCap = false;  continue; };
                item.gameObject.SetActive(true);
            }
        }
    }
    private void PrepareThirdStepForBottles(Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>() != null)
                child.GetComponent<MeshRenderer>().enabled = false;

            PrepareThirdStepForBottles(child);
        }
    }

    private void ReadyToThirdStepForBottles(Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>() != null)
                child.GetComponent<MeshRenderer>().enabled = true;

            ReadyToThirdStepForBottles(child);
        }
    }

    private void RandomizeCaps () {
        materials = materials.OrderBy(i => Random.value).ToList();
        bottle_1_cap.GetComponent<MeshRenderer>().material = materials[0];
        bottle_2_cap.GetComponent<MeshRenderer>().material = materials[1];
        bottle_3_cap.GetComponent<MeshRenderer>().material = materials[2];
    }

    public bool ThreeBottlesAreFilled() {
        if (transform.childCount < 3) return false;

        return 
        transform.GetChild(0).GetChild(3).GetComponent<PillInsideChecker>().filled &&
        transform.GetChild(1).GetChild(3).GetComponent<PillInsideChecker>().filled &&
        transform.GetChild(2).GetChild(3).GetComponent<PillInsideChecker>().filled;
    }
}