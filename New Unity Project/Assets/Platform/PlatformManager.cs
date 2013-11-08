using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

	public Transform prefab;
	public Transform obstacle;
	public int numberOfObjects;
	public float recycleOffset;
	public Vector3 startPosition;
	public Vector3 minSize, maxSize, minGap, maxGap;
	public float minY, maxY;
	public Material[] materials;
	public PhysicMaterial[] physicMaterials;
	
	private Vector3 nextPosition;
	private Queue<Transform> objectQueue;
	private Queue<Transform> obstacleQueue;
	private static PlatformManager instance;
	private float obstacleX;
	private float obstacleY;
	private float obstacleHeight;
	
	// Use this for initialization
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		objectQueue = new Queue<Transform>(numberOfObjects);
		obstacleQueue = new Queue<Transform>(numberOfObjects);
		for (int i = 0; i < numberOfObjects; i++) {
			objectQueue.Enqueue((Transform)Instantiate(prefab,
					new Vector3(0f, 0f, -100f), Quaternion.identity));
			obstacleQueue.Enqueue((Transform)Instantiate(obstacle,
					new Vector3(0f, 0f, -100f), Quaternion.identity));
			enabled = false;
		}
		obstacleHeight = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (objectQueue.Peek().localPosition.x + recycleOffset < Runner.distanceTraveled) {
			Recycle();
		}
	}
	
	private void GameStart() {
		nextPosition = startPosition;
		for(int i = 0; i < numberOfObjects; i++){
			Recycle();
		}
		enabled = true;
		getNextObstacleLoc(0f);
	}
	
	private void GameOver() {
		enabled = false;
	}
	
	private void Recycle() {
		Vector3 scale = new Vector3(
				Random.Range(minSize.x, maxSize.x),
				Random.Range(minSize.y, maxSize.y),
				Random.Range(minSize.z, maxSize.z));
				
				
		Vector3 position = nextPosition;
		position.x += scale.x * 0.5f;
		position.y += scale.y * 0.5f;
		
		Vector3 obstaclePosition = position;
		obstaclePosition.x += Random.Range(0, scale.x * 0.5f);
		obstaclePosition.y += scale.y;
		
		Transform o = objectQueue.Dequeue();
		o.localScale = scale;
		o.localPosition = position;
		nextPosition.x += scale.x;
		int materialIndex = Random.Range(0, materials.Length);
		o.renderer.material = materials[materialIndex];
		o.collider.material = physicMaterials[materialIndex];
		objectQueue.Enqueue(o);
		
		Transform obst = obstacleQueue.Dequeue();
		obst.localPosition = obstaclePosition;
		obstacleQueue.Enqueue(obst);
		
		nextPosition += new Vector3(
				Random.Range(minGap.x, maxGap.x),
				Random.Range(minGap.y, maxGap.y),
				Random.Range(minGap.z, maxGap.z));
				
		
		if(nextPosition.y < minY) {
			nextPosition.y = minY + maxGap.y;
		}
		else if(nextPosition.y > maxY){
			nextPosition.y = maxY - maxGap.y;
		}
	}
	
	//This method returns the location of the next obstacle
	//Input: location of the last obstacle location
	//Output: none, obstacleX mutated
	//This method should be called each time an obstacle is removed
	private void getNextObstacleLoc(float initX) {
		bool isFound = false;
		float returnX = 0f;
		foreach(Transform obj in instance.obstacleQueue){
			if(!isFound) {
				if(obj.position.x > initX){
					isFound = true;
					obstacleX = obj.position.x;
					obstacleY = obj.position.y;
				}
			}
		}
	}
	
	public static float getObstacleX() {
		return instance.obstacleX;
	}
	
	public static float getObstacleY() {
		return instance.obstacleY;
	}
	
	public static float getObstacleHeight() {
		return instance.obstacleHeight;
	}
	
	public static void removeObstacle(float position) {
		Vector3 dissapear = new Vector3(
					instance.obstacle.position.x,
					instance.obstacle.position.y,
					instance.obstacle.position.z + 1000);
					
		bool isFound = false;
		foreach(Transform obj in instance.obstacleQueue){
			if(!isFound) {
				if(obj.position.x >= position - 1){
					isFound = true;
					obj.position = dissapear;
				}
			}
		}
		
		instance.getNextObstacleLoc(dissapear.x);
	}
}