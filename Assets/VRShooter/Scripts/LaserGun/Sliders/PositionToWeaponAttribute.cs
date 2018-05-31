using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionToWeaponAttribute : MonoBehaviour {

	public GameObject sliderObject;
	public float translatedValue;
	public float maxDistance;

	private float previousPos;

	// Use this for initialization
	void Start () {
		previousPos = GetSliderPos();
	}
	
	// Update is called once per frame
	void Update () {
		// Only update when the position has changed
		if (previousPos != GetSliderPos()) {
			// Update previous position to current one
			previousPos = GetSliderPos ();

			// Do the conversion from the local position to a percentage (0.0f to 1.0f).
			// The lowest negative value is min and the highest positive one is max.
			float zPos = previousPos;
			float zPosAbs = zPos + maxDistance;
			float calculated = (zPosAbs / maxDistance) * 0.5f;
			translatedValue = Mathf.Max (Mathf.Min (calculated, maxDistance), -maxDistance);
		}
	}

	float GetSliderPos() {
		// Round to three digits to avoid floating point errors
		float rawZ = sliderObject.transform.localPosition.z * 1000.0f;
		float rounded = (float)Mathf.Round(rawZ);
		return rounded / 1000.0f;
	}
}
