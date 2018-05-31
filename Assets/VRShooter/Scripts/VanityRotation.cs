using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanityRotation : MonoBehaviour {

	private Vector3 rotationFactor = new Vector3(1, -20, 1);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (rotationFactor);
	}
}
