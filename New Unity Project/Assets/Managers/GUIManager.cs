using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public GUIText gameOverText, instructionsText, runnerText, distanceText, healthText;
	
	private static GUIManager instance;

	// Use this for initialization
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
	
	private void GameStart() {
		gameOverText.enabled = false;
		instructionsText.enabled = false;
		runnerText.enabled = false;
		enabled = false;
	}
	
	private void GameOver() {
		gameOverText.enabled = true;
		instructionsText.enabled = true;
		enabled = true;
	}
	
	public static void setDistance(float distance) {
		instance.distanceText.text = distance.ToString("f0");
	}
}
