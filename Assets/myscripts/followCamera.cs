using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour {

	public Camera mainCamera;

	private Vector3 newPos;
	private Vector3 offset;
//	public GameObject platform;

	void Start(){
//		mainCamera = gameObject.transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
//		newPos = new Vector3 (mainCamera.transform.position.x, gameObject.transform.position.y, mainCamera.transform.position.z); //without offset
//		offset = mainCamera.transform.TransformDirection(new Vector3(0,0,-1));

		offset = mainCamera.transform.TransformPoint (new Vector3 (0, 0, -0.5f));
//		newPos = new Vector3 (mainCamera.transform.position.x, gameObject.transform.position.y, offset.z);
		newPos = new Vector3 (offset.x, gameObject.transform.position.y, offset.z);

		gameObject.transform.position = newPos;

//		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, platform.transform.position.y-mainCamera.position.y, gameObject.transform.localPosition.z); 
//		//gameObject.transform.position.y;

//		gameObject.transform.position = new Vector3 (mainCamera.transform.position.x, gameObject.transform.position.y, mainCamera.transform.position.z); 
//		//gameObject.transform.position.y;
	}
}
