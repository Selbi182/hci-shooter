using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderInteract : MonoBehaviour {

	// Provided by parent object (controller)
	public bool zTriggerIsActive;

	// The slider that is attached for manipulation of the status
	private GameObject attachedSlider;
	private const GameObject NO_SLIDER_ATTACHED = null;

	// Misc
	public float maximumDistance;
	private Vector3 rotationFactor = new Vector3(1, 2, 3);
	private const ushort MAX_PULSE = 3999;

	// Use this for initialization
	void Start () {
		// Set no slider to be attached by default
		attachedSlider = NO_SLIDER_ATTACHED;
	}
	
	// Update is called once per frame
	void Update () {
		// Only do something when a slider is attached
		if (attachedSlider != NO_SLIDER_ATTACHED) {
			
			// If a slider is attached but Z-trigger has been released, cancel attachment
			if (!zTriggerIsActive) {
				//TODO haptic pulse
				//HapticPulseDo ();

				// Kill attachment
				attachedSlider = NO_SLIDER_ATTACHED;

				// Exit early
				return;
			}
				
			// Otherwise manipulate position of the slider:
			// 1. Copy ABSOLUTE position of controller to slider
			// 2. Then manually set the RELATIVE X and Y axis to 0.
			// 3. Limit the maximum distance of the slider to 3 in both directions
			attachedSlider.transform.position = this.transform.position;
			Vector3 tempLP = attachedSlider.transform.localPosition;
			tempLP.x = 0f;
			tempLP.y = 0f;
			tempLP.z = Mathf.Min(Mathf.Max (tempLP.z, -maximumDistance), maximumDistance);

			// Store the result back into the slider
			attachedSlider.transform.localPosition = tempLP;
		}
	}

	// Called at a fixed point
	void FixedUpdate() {
		// Rotate for visual effects
		transform.Rotate(rotationFactor);
	}

	// Called every frame while any trigger is being touched
	void OnTriggerStay (Collider other) 
	{
		// Check if the triggered object is a Slider AND if the Z-button is pressed
		if (other.gameObject.CompareTag("Slider")) {
			// Show Slider interactor
			gameObject.transform.Find("VisualRepresentationON").gameObject.SetActive(true);

			// Check if Z-Trigger is pressed
			if (zTriggerIsActive) {
				// Attach slider to slider interactor
				attachedSlider = other.gameObject;

				//TODO haptic pulse
				//HapticPulseDo ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		// Reset color
		gameObject.transform.Find("VisualRepresentationON").gameObject.SetActive(false);
	}

	void HapticPulseDo()
	{
		//controller.TriggerHapticPulse(MAX_PULSE);
	}

}
