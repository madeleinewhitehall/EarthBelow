using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class viewManager : MonoBehaviour {

	public GameObject universe;
	public GameObject view1;
	public GameObject view2;
	public GameObject view3;
	public new GameObject camera;

	public Texture wait;
	public Texture tap;
	public Texture dragDown;
	public Texture dragUp;
	public Texture swipe;

	public Animator worldDwn;
	public Animator worldUp;
	public Animator floor1;
	public Animator floor2;
	public Animator floor3;
	public Animator floor4;

	public Text instruction;
	public Canvas canvas;
	public Image panel;
	public RawImage pointer;
	private Vector3 startPos;
	private Vector3 endPos;

	public static string viewStatus;
	public static int loop;
	private int textTime;
	private bool animationFinish;
	private bool earthPlaced;


	// Use this for initialization
	void Start () {
		viewStatus = "opening";
		loop = 0;
		textTime = 0;

		earthPlaced = false;
		animationFinish = false;

		//panel initialisation
		instruction.text = "";
		instruction.color = new Color (1, 1, 1, 1);
		pointer.color = new Color(0,0,0,0);
		panel.canvasRenderer.SetAlpha (1);
	}
		
	void Update(){ 
		//initial screen
		if(viewStatus == "opening"){
			textTime++;


			if (textTime >= 150) {
				viewStatus = "start";
				textTime = 0;
				panel.CrossFadeAlpha (0f, 0.7f, false);
			} else if (textTime < 100) {
				panel.rectTransform.localPosition = new Vector3(panel.rectTransform.localPosition.x,panel.rectTransform.localPosition.y-1.5f,panel.rectTransform.localPosition.z);
			}

		}
		//particle view
		else if (viewStatus == "start") {
			textTime++;


			if (textTime >= 900) {
				instruction.text = "";
				pointer.color = new Color(0,0,0,0);
				panel.CrossFadeAlpha (0f, 0.5f, false);
			} else if (textTime >= 500) {
				instruction.text = "Tap the floor to place the Earth";	
				pointer.texture = tap;
			}
			else if (textTime >= 150) {
				panel.rectTransform.sizeDelta = new Vector2 (2000f, 300f);
				panel.GetComponent<Image>().sprite = null;
				panel.rectTransform.localPosition = new Vector3 (0, 0, 0);
				pointer.color = new Color(1,1,1,1);
				panel.CrossFadeAlpha (0.5f, 0.5f, false);

				instruction.text = "Wait for the yellow dots to appear";
				instruction.color = new Color (1, 1, 1, 1);
			}

			//tapped to place the Earth
			if((Input.touchCount > 0) && ((Input.GetTouch (0).phase == TouchPhase.Began))||(Input.GetKeyDown("space")==true)){ 
				worldUp.enabled = true;
				loop = 1;
			}

			if (loop > 0) {
				if (checkAnimating (worldUp)) {
					loop++;
				} else {
					animationFinish = true;
				}
			}

			if(animationFinish == true){
				//disable animation
				worldUp.enabled = false;

				view1.SetActive (true);
				viewStatus = "full globe";

				animationFinish = false;
				loop = 0;
				textTime = 0;
				panel.CrossFadeAlpha (0.5f, 0.5f, false);
			}

		}
		//View 1
		else if (viewStatus == "full globe") {
			textTime++;


			instruction.text = "Swipe horizontally to rotate Earth";
			pointer.color = new Color(1,1,1,1);
			pointer.texture = swipe;


			if (textTime >= 600) {
				instruction.text = "";
				pointer.color = new Color(0,0,0,0);
				panel.CrossFadeAlpha (0f, 0.5f, false);
			}else if (textTime >= 300) {
				instruction.text = "Drag down with two fingers to view below";	
				pointer.texture = dragDown;
			}

			//swipe Earth down
			if ((Input.touchCount == 2) && ((Input.GetTouch (0).deltaPosition.y < 0))||(Input.GetKeyDown("space")==true)) {
				worldDwn.enabled = true;
				loop = 1;
			}

			if (loop > 0) {
				if (checkAnimating (worldDwn)) { //if still animating
					loop++;
				} else {
					animationFinish = true;
				}
			}

			if(animationFinish == true){
				worldDwn.enabled = false;

				if (earthPlaced == false) {
					placeBelow ();
				}

				view2.SetActive (true);
				view1.SetActive (false);
				viewStatus = "earth below";	

				view1.transform.GetChild (0).transform.localPosition = new Vector3 (0, 12, 0);
				animationFinish = false;

				textTime = 0;
				panel.CrossFadeAlpha (0.5f, 0.5f, false);
			}

		}
		//View 2
		else if(viewStatus == "earth below"){
			textTime++;


			instruction.text = "Tap to explore below";
			pointer.color = new Color(1,1,1,1);
			pointer.texture = tap;

			if (textTime >= 300) {
				instruction.text = "";
				pointer.color = new Color(0,0,0,0);
				panel.CrossFadeAlpha (0f, 0.5f, false);
			}

			//tapped floor
			if((Input.touchCount > 0) && ((Input.GetTouch (0).phase == TouchPhase.Began))||(Input.GetKeyDown("space")==true)){

				view2.SetActive (false);
				view3.SetActive (true);	
				viewStatus = "layers";

				floor1.enabled = false;
				floor2.enabled = false;
				floor3.enabled = false;
				floor4.enabled = false;

				textTime = 0;
				panel.CrossFadeAlpha (0.5f, 0.5f, false);
			}

		}
		//View 3
		else if(viewStatus == "layers"){
			textTime++;


			instruction.text = "Tap to dig. Swipe up to reset.";
			pointer.color = new Color(1,1,1,1);
			pointer.texture = tap;

			if (textTime >= 300) {
				instruction.text = "";
				pointer.color = new Color(0,0,0,0);
				panel.CrossFadeAlpha (0f, 0.5f, false);
			} else if (textTime >= 150) {
				pointer.texture = dragUp;
			}

			//user dragged up
			if((Input.touchCount > 0) && ((Input.GetTouch(0).deltaPosition.y > 0))||(Input.GetKeyDown("space")==true)){
				view3.SetActive (false);
				view1.SetActive (true);
				viewStatus = "full globe";	

				canvas.enabled = false;
				textTime = 0;
				earthPlaced = false;
			}
		}
	}

	bool checkAnimating(Animator anim){
		return (anim.GetCurrentAnimatorStateInfo (0).length > anim.GetCurrentAnimatorStateInfo (0).normalizedTime);		
	}

	void fadeCanvasGroup(CanvasGroup cg){
		for (int i = 0; i < 100; i++) {
			cg.alpha = 1.0f - (i * 1f / 100f);
		}
	}

	void placeBelow(){
		view2.transform.position = new Vector3 (camera.transform.position.x, view2.transform.position.y, camera.transform.position.z);
		earthPlaced = true;
	}

}
