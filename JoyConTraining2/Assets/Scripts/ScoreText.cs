using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    Text scoreText;

    PlayerController playerCtrl;

	// Use this for initialization
	void Start () {
        scoreText = GetComponent<Text>();
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        scoreText.text = playerCtrl.score.ToString();
	}
}
