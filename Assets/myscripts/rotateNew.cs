using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateNew : MonoBehaviour {

	//	public Transform target; //image

	public GameObject test; //circle
	public GameObject world;

	private Vector3 d1; //change it's initial forward direction
	private Vector3 d2; //change direction to rotate
	private float heading;

	void Start(){
		test = GameObject.Find ("userPosition");
		print ("this is where you are "+test.transform);
		topPoint ();
		//		pointNorth ();
	}

	void topPoint(){
		//		print ("old test " + test.transform.position);

		d1 = test.transform.position - world.transform.position; //where I am already at
		d2 = Vector3.zero - world.transform.position; //desired direction to point to

		Quaternion rotate = Quaternion.FromToRotation (d1, d2);
		world.transform.rotation = rotate;

		//		print ("final test " + test.transform.position);
	}

	void pointNorth(){
		Input.location.Start ();

		heading = getLocation.northRotation;

		world.transform.rotation = Quaternion.Euler (0, -heading, 0);
	}
}
