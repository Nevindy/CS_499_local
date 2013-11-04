﻿using UnityEngine;
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
	
	// Use this for initialization
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		objectQueue = new Queue<Transform>(numberOfObjects);
		obstacleQueue = new Queue<Transform>(numberOfObjects);
		for (int i = 0; i < numberOfObjects; i++) {
			objectQueue.Enqueue((Transform)Instantiate(prefab,
					new Vector3(0f, 0f, -100f), Quaternion.identity));
			obstacleQueue.Enqueue((Transform)Instantiate(prefab,
					new Vector3(0f, 0f, -100f), Quaternion.identity));
			enabled = false;
		}
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
		obstaclePosition.x += Random.Range(0, scale.x);
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
}