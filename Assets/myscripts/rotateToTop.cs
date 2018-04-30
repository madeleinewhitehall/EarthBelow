using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToTop : MonoBehaviour {
	
	public GameObject world;
	public GameObject galaxy;
	public GameObject universe;
	public new GameObject camera;

	private Vector3 d1; //change it's initial forward direction
	private Vector3 d2; //change direction to rotate

	private float heading;

	void Update(){
		//only if successfully obtained user position
		if (getLocation.userLatLong.x != 0 && getLocation.userLatLong.y != 0) {			
			GameObject test = GameObject.Find ("userPosition");

			topPoint (test);
			enabled = false;
		}
	}

	//rotates the Earth so that it points in the right direction
	void topPoint(GameObject test){
		//get reference points
		GameObject north = GameObject.Find ("north");
		GameObject top = GameObject.Find ("top");


		//rotate user position to the top of the globe
		d1 = test.transform.position - world.transform.position; //current direction
		d2 = top.transform.position - world.transform.position; //desired direction

		Quaternion rotate1 = Quaternion.FromToRotation (d1, d2);
		world.transform.rotation = rotate1;



		//rotate so that north pole is aligned with z axis
		float theta = 0f; 
		Vector3 tempAxis = galaxy.transform.up;

		//get new reference points
		Vector3 point = new Vector3(top.transform.position.x, world.transform.position.y, top.transform.position.z);
		Vector3 forwardPoint = new Vector3 (north.transform.position.x, point.y, north.transform.position.z);

		d1 = point - world.transform.position; //where you are
		d2 = forwardPoint - world.transform.position; //where you want to go

		Quaternion rotate2 = Quaternion.FromToRotation (d1, d2);
		rotate2.ToAngleAxis (out theta, out tempAxis);
		world.transform.RotateAround (world.transform.position, galaxy.transform.up,theta);



		//rotate galaxy so virtual North aligns with real North
		galaxy.transform.Rotate (0,-getLocation.northRotation,0);
	}
}
