using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMovement : MonoBehaviour {

	public float upDownSpeed = 0.5f;
	public float rotateAngle = 30f;

	private float startY;

	// Use this for initialization
	void Start () {
		startY = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		float newVal = Mathf.Sin(upDownSpeed * Time.frameCount);

		Vector3 pos = this.transform.position;
		pos.y = startY + newVal;
		this.transform.position = pos;

		this.transform.Rotate(new Vector3(0f, newVal * rotateAngle, 0f));
	}
}
