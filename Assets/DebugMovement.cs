using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovement : MonoBehaviour {

	public bool enableDebugMovement;
	public float factor = 0.001f;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}
	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (enableDebugMovement && controller.GetPress( SteamVR_Controller.ButtonMask.Touchpad )) {
			Vector3 temp = this.transform.rotation.eulerAngles;
			temp.x -= 180f;
			temp.y -= 180f;
			temp.z -= 180f;

			Vector3 orientation = Vector3.Scale(temp.normalized, new Vector3(factor, factor, factor));
			this.transform.parent.gameObject.transform.Translate(orientation);
		}
	}



}
