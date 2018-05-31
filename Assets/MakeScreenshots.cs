using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeScreenshots : MonoBehaviour {



	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

	private int scr = 0;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.GetPressDown (gripButton)) {
			ScreenCapture.CaptureScreenshot("C:\\Users\\HCI\\Desktop\\HCI Shooter VR - V5\\screenshots\\screenshot" + scr++ + ".png");
		}
	}
}
