using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layerReveal : MonoBehaviour {

	public GameObject outCore;
	public GameObject lowMant;
	public GameObject upMant;
	public GameObject layer1;
	public GameObject layer2;

	public Animator floor1;
	public Animator floor2;
	public Animator floor3;
	public Animator floor4;

	private TextMesh layerText1;
	private TextMesh layerText2;

	public static int taps;

	// Use this for initialization
	void Start () {
		taps = 0;
		layerText1 = layer1.GetComponent<TextMesh> ();
		layerText2 = layer2.GetComponent<TextMesh> ();
		layerText1.text = "";
		layerText2.text = "";
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				taps++;

				//determines the current layer
				switch (taps) {
				case(1):
					//enable the floor animation
					floor1.enabled = true;
					floor2.enabled = true;
					floor3.enabled = true;
					floor4.enabled = true;
					layerText1.text = "Upper Mantle";
					break;
				case(2):
					layerText1.text = "Upper Mantle";
					changeLayer (upMant, true);
					break;
				case(3):
					layerText1.text = "Lower Mantle";
					changeLayer (lowMant, true);
					changeLayer (upMant, false);
					break;
				case(4):
					layerText1.text = "Outer Core";
					changeLayer (outCore, true);
					changeLayer (lowMant, false);
					break;
				case(5):
					layerText1.text = "Inner Core";
					changeLayer (outCore, false);
					break;
				default:
					layerText1.text = "Upper Mantle";
					changeLayer (upMant, true);
					changeLayer (lowMant, true);
					changeLayer (outCore, true);
					break;
				}
				layerText2.text = layerText1.text;

				//if the user reaches the innerCore then they cant delve any further, return to the surface
				if (taps > 5) {
					taps = 2;
				}
			}
		}		
	}

	public void changeLayer(GameObject layer, bool active){
		layer.SetActive(active);
	}
}
