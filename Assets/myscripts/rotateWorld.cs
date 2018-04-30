using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateWorld : MonoBehaviour {

	private string view;

	void Start () {
		view = "full globe";
	}

	// Update is called once per frame
	void Update () {
		if (view == "full globe") {
			//allows for the Earth to be rotated by the user
			if ((Input.touchCount == 1) && ((Input.GetTouch (0).phase == TouchPhase.Moved))) { 

				float deltaPos = Input.GetTouch (0).deltaPosition.x;

				gameObject.transform.Rotate(gameObject.transform.up, -deltaPos * 0.3f,Space.Self);
			}
		}
	}
}
