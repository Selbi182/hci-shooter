using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunUpgrades : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller
	{
		get
		{
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	public GameObject tableWrapper;
	public GameObject upgrade1Pickup;
	public GameObject upgrade2Pickup;
	public GameObject upgrade3Pickup;

	public float pickupDistance = 0.2f;

	private static ushort MAX_HAPTIC = 3999;

	// Use this for initialization
	void Start () {
		// Attach live controller
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		// Upgrade 1
		if (upgrade1Pickup.gameObject.activeInHierarchy) {
			// Distance check
			float distanceUpgrade = Vector3.Distance (upgrade1Pickup.transform.position, gameObject.transform.position);
			if (distanceUpgrade < pickupDistance) {
				// Haptic puls
				controller.TriggerHapticPulse (MAX_HAPTIC);

				// Activate Upgrade 1 and enable next
				this.gameObject.SendMessage ("activateUpgrade", 1);
				tableWrapper.SendMessage ("increaseTableIndex");
			}
		}

		// Upgrade 2
		if (upgrade2Pickup.gameObject.activeInHierarchy) {
			// Distance check
			float distanceUpgrade = Vector3.Distance (upgrade2Pickup.transform.position, gameObject.transform.position);
			if (distanceUpgrade < pickupDistance) {
				// Haptic puls
				controller.TriggerHapticPulse (MAX_HAPTIC);

				// Activate Upgrade 2 and enable next
				this.gameObject.SendMessage ("activateUpgrade", 2);
				tableWrapper.SendMessage ("increaseTableIndex");
			}
		}

		// Upgrade 3
		if (upgrade3Pickup.gameObject.activeInHierarchy) {
			// Distance check
			float distanceUpgrade = Vector3.Distance (upgrade3Pickup.transform.position, gameObject.transform.position);
			if (distanceUpgrade < pickupDistance) {
				// Haptic puls
				controller.TriggerHapticPulse (MAX_HAPTIC);

				// Activate Upgrade 3 and enable next
				this.gameObject.SendMessage ("activateUpgrade", 3);
				tableWrapper.SendMessage ("increaseTableIndex");
			}
		}
	}
}
