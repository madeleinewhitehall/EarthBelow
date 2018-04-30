using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeObjects : MonoBehaviour {

	public GameObject galaxy;
	public GameObject earth;
	public GameObject world;
	public GameObject image;
	public GameObject posPin;
	public GameObject userPin;

	public Vector3 origin;
	public Vector3 ecefPos;
	public Vector2 latlongPos;
	public Vector2 userPos;
	public Vector2[] cities;

	private float scaling;
	public float height;
	public float radius;
	public float flattening;
	private string view;
	private float pinSize;


	// Use this for initialization
	void Start () {

		view = viewManager.viewStatus;

		//determines pin size
		if (view == "earth below") {
			pinSize = 0.2f;
		} else {
			pinSize = 0.7f;
		}

		flattening = 1.0f/298.257224f;
		height = 0.0f; //altitude from the face of the earth
		radius = earth.transform.localScale.x*100; //this is in km. As object is sphere all components are the same. remember to scale to fit unity units
		origin = earth.transform.position;

		scaling = image.transform.localScale.x * gameObject.transform.localScale.x;

		//get user latLong from getLocation.cs
		userPos = new Vector2(getLocation.userLatLong.x, getLocation.userLatLong.y);

		//this fixes inaccurate Earth texture
		if (userPos.x > 30){
			userPos.x = userPos.x + (userPos.x - 30.362f) / 2;
		}
		else if (userPos.x < -30){
			userPos.x = userPos.x + (userPos.x + 30.362f) / 2;
		}

		//assign cities
		cities = new Vector2[7];
		cities [0] = userPos;
		cities [1] = new Vector2 ((51.509865f+((51.509865f-30f)/2f)), -0.118092f); //London
		cities [2] = new Vector2 ((48.864716f+((48.864716f-30f)/2f)), 2.349014f); //Paris 
		cities [3] = new Vector2 ((40.730610f+((40.730610f-30f)/2f)), -73.935242f); //New York
		cities [4] = new Vector2 ((41.906204f+((41.906204f-30f)/2f)), 12.507516f); //Rome
		cities [5] = new Vector2 ((35.652832f+((35.652832f-30f)/2f)), 139.839478f); //Tokyo
		cities [6] = new Vector2 ((-35.2809f+((-35.2809f+30f)/2f)), 149.1300f); //Canberra

		//create pins for the cities
		createObj (cities);
	}

	void Update(){
		//this is needed if the getLocation script doesn't retrieve information fast enough
		if (getLocation.userLatLong.x != 0 && getLocation.userLatLong.y != 0) {
			userPos = new Vector2(getLocation.userLatLong.x, getLocation.userLatLong.y);

			if (userPos.x > 30){
				userPos.x = userPos.x + (userPos.x - 30.362f) / 2;
			}
			else if (userPos.x < -30){
				userPos.x = userPos.x + (userPos.x + 30.362f) / 2;
			}
			setObj (ecefConvertPos (userPos));
			enabled = false;
		}
	}

	void createObj(Vector2[] cities){
		for (var i = 0; i < cities.Length; i++) {
			
			//create an object
			GameObject pin;

			if (i == 0) { //if placing user position
				pin = Instantiate(userPin);
				pin.AddComponent<MeshRenderer>();
				pin.name = "userPosition";
			} else { 
				pin = Instantiate(posPin);
				pin.AddComponent<MeshRenderer>();
				pin.name = "city"+i;
			}

			//scale depending on parents
			pin.transform.localScale = new Vector3 (pinSize, pinSize, pinSize)*(scaling/20);

			//assign parent object
			pin.transform.parent = gameObject.transform;

			//convert latlong to ecef
			ecefPos = ecefConvertPos (cities [i]);

			//position pin
			pin.transform.localPosition = ecefPos;

			//orientate pins to look to Earth centre
			pin.transform.LookAt (gameObject.transform.position);
		}
	}
		
	void setObj(Vector3 newPosition){
		//reassigns userPosition when location data comes
		GameObject.Find ("userPosition").transform.localPosition = newPosition; 
	}

	Vector3 ecefConvertPos(Vector2 pos){
		//need to convert to radians
		pos = pos*(Mathf.PI)/180.0f;

		//apply latlong -> ecef conversion formula
		var c = 1 / Mathf.Sqrt (Mathf.Cos (pos.x) * Mathf.Cos (pos.x) + (1 - flattening) * (1 - flattening) * Mathf.Sin (pos.x) * Mathf.Sin (pos.x));

		var s = (1 - flattening) * (1 - flattening) * c; 

		var X = (radius*c + height)* Mathf.Cos (pos.x) * Mathf.Cos (pos.y);
		var Y = (radius*c + height)* Mathf.Cos (pos.x) * Mathf.Sin (pos.y);
		var Z = (radius*s + height) * Mathf.Sin (pos.x);

		ecefPos = new Vector3 (-Y, Z, X);

		//needs to be scaled to match environment proportions 
		return ecefPos*(1.0f/(100.0f*2.0f));
	}		
}
