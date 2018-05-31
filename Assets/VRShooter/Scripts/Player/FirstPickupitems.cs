using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPickupitems : MonoBehaviour {

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }

    public GameObject laserGunPickup;
    public GameObject teleportBallPickup;
	public GameObject tableWrapper;

    public GameObject laserGunController;
    public GameObject teleportBallController;

    public bool isRight;

    public float pickupDistance = 0.2f;



    // Use this for initialization
    void Start () {

        // Attach live controller
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
        // Check if Controller is available first
        if (controller == null)
            return;

        // Laser gun pickup
		if (laserGunPickup.gameObject.activeInHierarchy)
        {
            // Distance check
            float distanceLasergun = Vector3.Distance(laserGunPickup.transform.position, gameObject.transform.position);
            if (distanceLasergun < pickupDistance)
            {
                // Haptic pulse
                controller.TriggerHapticPulse(1999);

                // Z button check
                if (controller.GetPressDown(triggerButton))
                {
                    // Remove pickup object
                    laserGunPickup.SetActive(false);

                    // Activate controller
                    laserGunController.SetActive(true);

                    // Assign to correct hand
                    if (isRight)
                        gameObject.transform.parent.GetComponent<SteamVR_ControllerManager>().right = laserGunController;
                    else
                        gameObject.transform.parent.GetComponent<SteamVR_ControllerManager>().left = laserGunController;

                    // Copy index attribute
                    laserGunController.GetComponent<SteamVR_TrackedObject>().index = gameObject.transform.GetComponent<SteamVR_TrackedObject>().index;

                    // Remove original controller
                    Destroy(gameObject);

                    // Spawn enemies
                    FireSpawnEvent();

					// Destroy table
					//laserGunPickup.transform.parent.gameObject.SetActive(false);
					tableWrapper.SendMessage("increaseTableIndex");
                }
            }
        }


        // Teleport ball pickup
		if (teleportBallPickup.gameObject.activeInHierarchy) {
			// Distance check
			float distanceTeleportball = Vector3.Distance (teleportBallPickup.transform.position, gameObject.transform.position);
			if (distanceTeleportball < pickupDistance) {
				// Haptic puls
				controller.TriggerHapticPulse (500);

				// Z button check
				if (controller.GetPressDown (triggerButton)) {
					// Remove pickup object
					teleportBallPickup.SetActive (false);

					// Activate controller
					teleportBallController.SetActive (true);

					// Assign to correct hand
					if (isRight)
						gameObject.transform.parent.GetComponent<SteamVR_ControllerManager> ().right = teleportBallController;
					else
						gameObject.transform.parent.GetComponent<SteamVR_ControllerManager> ().left = teleportBallController;

					// Copy index attribute
					teleportBallController.GetComponent<SteamVR_TrackedObject> ().index = gameObject.transform.GetComponent<SteamVR_TrackedObject> ().index;

					// Specific for teleport ball: Make sure the Vive model is still loaded
					teleportBallController.transform.Find ("Model").GetComponent<SteamVR_RenderModel> ().index = gameObject.transform.GetComponent<SteamVR_TrackedObject> ().index;
					teleportBallController.transform.Find ("Model").gameObject.SetActive (true);

					// Remove original controller
					Destroy (gameObject);

					// Spawn enemies
					FireSpawnEvent ();

					// Destroy table
					//teleportBallPickup.transform.parent.gameObject.SetActive(false);
					tableWrapper.SendMessage ("increaseTableIndex");
				}
			}
		}
    }

    // Spawn enemies once both items have been picked up
    void FireSpawnEvent()
    {
        // Spawn enemies when both pickups are activated
        if (!laserGunPickup.gameObject.activeSelf && !teleportBallPickup.gameObject.activeSelf)
        {
            Destroy(laserGunPickup);
            Destroy(teleportBallPickup);

			// TODO Disabled enemy spawn for testing purposes
            //GameObject.FindGameObjectWithTag("World").SendMessage("Ready");
        }
    }
}
