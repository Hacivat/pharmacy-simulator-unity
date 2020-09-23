using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject bottles;
    private GameObject bottlesForNewRound;
    private Vector3 bottlesStartPosition;

    public static int round = 1;

    void Awake () {
        bottlesForNewRound = new GameObject();
        bottlesStartPosition = new Vector3();
    }
    void Start () {
        bottlesForNewRound = Instantiate(bottles);
        bottlesForNewRound.name = "newBottles";
        bottlesForNewRound.SetActive(false);
        bottlesStartPosition = bottles.transform.position;
    }

    public void ReInitializeRound () {
        round++;
        if (round == 3) {
            Time.timeScale = 0;
            GameObject gameOver = GameObject.Find("GameOver");
            gameOver.GetComponent<Text>().enabled = true;
        }

        StartCoroutine(ReInitializeRoundOneSecLater());
    }

    private IEnumerator ReInitializeRoundOneSecLater() {
        ReInitializeAllPasses();
        yield return new WaitForSeconds(1);
        Destroy(GameObject.Find("Bottles").gameObject);
        
        DragController.firstStepCheckList = new List<string>();
        ReInitializeBottles();
        ReInitializeCamera();
        RemoveClonePills();
        
        GameObject pillsDropPoint = GameObject.Find("PillsDropPoint");
        pillsDropPoint.GetComponent<PillDropper>().allPills = new List<GameObject>();
    }

    private void ReInitializeBottles () {
        bottlesForNewRound.transform.position = bottlesStartPosition;
        bottlesForNewRound.name = "Bottles";
        bottlesForNewRound.SetActive(true);
        bottlesForNewRound.GetComponent<BottlesController>().DoItWhenReinitialized();
    }

    private void ReInitializeCamera () {
        Transform cameraStartPoint = GameObject.Find("CameraStartPoint").transform;
        Camera.main.transform.position = cameraStartPoint.transform.position;
        Camera.main.GetComponent<CameraController>().movedToPetri = false;
        Camera.main.GetComponent<CameraController>().movedToShelves = false;
    }

    private void ReInitializeAllPasses () {
        GameObject firstPass = GameObject.Find("FirstPass");    
        firstPass.GetComponent<Pass>().InTheScene();

        GameObject secondPass = GameObject.Find("SecondPass");
        secondPass.GetComponent<Pass>().InTheScene();

        GameObject thirdPass = GameObject.Find("ThirdPass");
        thirdPass.GetComponent<Pass>().InTheScene();

        GameObject fourthPass = GameObject.Find("FourthPass");
        fourthPass.GetComponent<Pass>().InTheScene();
    }

    private void RemoveClonePills() {
        List<GameObject> mainObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(mainObjects);

        foreach (GameObject item in mainObjects)
        {
            if (item.tag == "Pill" && item.name.Contains("(Clone)")) {
                Destroy(item);   
            }
        }
    }
}
