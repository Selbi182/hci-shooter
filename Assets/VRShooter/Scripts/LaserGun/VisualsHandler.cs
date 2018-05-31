using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsHandler : MonoBehaviour {

	public bool isReloading = false;
	public bool isFullAutomatic = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isReloading) {
			gameObject.transform.Find ("ReloadIndicator").gameObject.SetActive (true);
			gameObject.transform.Find ("Sliders").gameObject.SetActive (false);
		} else {
			gameObject.transform.Find ("ReloadIndicator").gameObject.SetActive (false);
			gameObject.transform.Find ("Sliders").gameObject.SetActive (true);
		}

		if (isFullAutomatic) {
			gameObject.transform.Find ("Laser Pointer").gameObject.GetComponent<LineRenderer>().enabled = false;
		} else {
			gameObject.transform.Find ("Laser Pointer").gameObject.GetComponent<LineRenderer>().enabled = true;
		}
	}
}
