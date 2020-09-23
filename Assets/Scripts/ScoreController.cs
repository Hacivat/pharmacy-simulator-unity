using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static int scoreValue = 0;
    private GameObject score;
    void Start () {
        scoreValue = 0;
    }
    void Update () {
        score = GameObject.Find ("Score");
        score.GetComponent<Text>().text = "Score: " + scoreValue.ToString();
    }
}
