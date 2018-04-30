using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class anchorWorld: MonoBehaviour
	{
		public GameObject universe;
		public GameObject manager;

		public static int countTap;

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					Debug.Log ("Got hit!");
					universe.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					universe.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
					Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", universe.transform.position.x, universe.transform.position.y, universe.transform.position.z));
					return true;
				}
			}
			return false;
		}

		void Start(){
			countTap = 0;
		}

		// Update is called once per frame
		void Update () {
			if (Input.touchCount > 0 && universe.transform != null && countTap < 1) //countTap < 1 to place the sphere once
			{
				//initialise getLocation
				manager.GetComponent<getLocation>().enabled = true;


				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

					// prioritize reults types
					ARHitTestResultType[] resultTypes = {
						ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
						// if you want to use infinite planes use this:
						//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
						ARHitTestResultType.ARHitTestResultTypeFeaturePoint
					}; 

					foreach (ARHitTestResultType resultType in resultTypes)
					{
						if (HitTestWithResultType (point, resultType))
						{
							return;
						}
					}
				}
				countTap++;
				enabled = false;
			}
		}
	}
}

