using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public GUIText gameOverText, instructionsText, runnerText, distanceText, healthText, healthNumText;
	
	private static GUIManager instance;

	// Use this for initialization
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		gameOverText.enabled = false;
		healthText.enabled = false;
		healthNumText.enabled = false;
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
		healthText.enabled = true;
		healthNumText.enabled = true;
		enabled = false;
	}
	
	private void GameOver() {
		gameOverText.enabled = true;
		instructionsText.enabled = true;
		healthText.enabled = false;
		healthNumText.enabled = false;
		enabled = true;
	}
	
	public static void setDistance(float distance) {
		instance.distanceText.text = distance.ToString("f0");
	}
	
	public static void setHealth(int health) {
		instance.healthNumText.text = health.ToString();
	}
}
