using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public static float distanceTraveled;
	
	public float acceleration;
	public Vector3 jumpVelocity;
	public float gameOverY;
	
	private bool touchingPlatform;
	private Vector3 startPosition;
	// Use this for initialization
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		startPosition = transform.localPosition;
		renderer.enabled = false;
		rigidbody.isKinematic = true;
		enabled = false;
	}
	
	private void GameStart() {
		distanceTraveled = 0f;
		GUIManager.setDistance(distanceTraveled);
		transform.localPosition = startPosition;
		renderer.enabled = true;
		rigidbody.isKinematic = false;
		enabled = true;
	}
	
	private void GameOver() {
		renderer.enabled = false;
		rigidbody.isKinematic = true;
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(touchingPlatform && Input.GetButtonDown("Jump")){
			rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
			touchingPlatform = false;
		}
		distanceTraveled = transform.localPosition.x;
		GUIManager.setDistance(distanceTraveled);
		
		if(transform.localPosition.y < gameOverY) {
			GameEventManager.TriggerGameOver();
		}
		float obstX = PlatformManager.getObstacleX();
		Debug.Log(obstX);
		if(distanceTraveled >= obstX) {
			PlatformManager.removeObstacle(distanceTraveled);
		}
	}
	
	void FixedUpdate () {
		if(touchingPlatform) {
			rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
		}
	}
	
	void OnCollisionEnter () {
		touchingPlatform = true;
	}
	
	void OnCollisionExit () {
		touchingPlatform = false;
	}
}
