using UnityEngine;
using System.Collections;
using System;

public class BallBehaviorInteraction : MonoBehaviour {

	public GameObject sliderSphere;
	public GameObject throwBall;
    private GameObject instantiatedThrowBall;

    public GameObject teleportIndicator;

    //public GameObject teleporterOld;
    public GameObject teleporterNew;
    
    //private GameObject instantiatedTeleporterOld;
    private GameObject instantiatedTeleporterNew;

    public float velocityMultiplier;
    public int velocityCalcBuffer;
    public bool fullyCharged = false;
    public bool hasBeenThrown = false;
    public float pickupDistance = 0.1f;
    public float teleportHandRaiseDistance = 0.4f;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId dpadDownButton = Valve.VR.EVRButtonId.k_EButton_DPad_Down;

    private Queue teleportYBuffer;
    private Queue previousVelocities;
    
    public int chargeupTime;
    private int currentCharge;
    private const ushort MAX_PULSE = 3999;

	private float teleportRaiseThreshold;



    // Use this for initialization
    void Start () {
		instantiatedTeleporterNew = Instantiate(teleporterNew.gameObject, this.transform.position, Quaternion.identity) as GameObject;
		instantiatedTeleporterNew.SetActive (false);

        currentCharge = 0;

		teleportRaiseThreshold = 0f;

        // Create queues and fill with slots
        teleportYBuffer = new Queue();
        previousVelocities = new Queue();
        for (int i = 0; i< velocityCalcBuffer; i++)
        {
            teleportYBuffer.Enqueue(100f);
            teleportYBuffer.Enqueue(100f);
            teleportYBuffer.Enqueue(100f);
            previousVelocities.Enqueue(Vector3.zero);
        }

		// Attach live controller
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        // If ball has been thrown away and is ready for teleportation
        if (hasBeenThrown)
        {
            // Disallow interaction with the gun
            sliderSphere.SetActive(false);

            // Check distance to the thrown teleport ball
            float distance = Vector3.Distance(gameObject.transform.position, instantiatedThrowBall.gameObject.transform.position);
            if (distance < pickupDistance)
            {
				HapticPulseDo(Math.Min(pickupDistance - distance, 0.5f));

                // Script to pick it up again if Z button is pressed
				if (controller.GetPress(gripButton))
                {
                    ReturnBallToHand();
                    return;
                }
                
            }

            // Safety check to automatically return the ball when it fell below the map
            if (instantiatedThrowBall.gameObject.transform.position.y < -10f)
            {
                ReturnBallToHand();
                return;
            }

			// Hide teleporter indicator when the button is released
			if (controller.GetPressUp (gripButton)) {
				//instantiatedTeleporterNew.SetActive (false);
			}

			// Set teleportation boundary on first button down frame
			if (controller.GetPressDown (gripButton)) {
				ReturnBallToHand ();
				//InitiateTeleportThreshold ();
			}

            // Perform teleportation
			if (controller.GetPress(triggerButton))
            {
				// Show teleport indicator and update its position
				Vector3 newPos = this.transform.position;
				float currentPosition = newPos.y;
				newPos.y = teleportRaiseThreshold;
				instantiatedTeleporterNew.transform.position = newPos;

				// Haptic pulse the closer you get
				HapticPulseDo(0.5f + currentPosition - (teleportRaiseThreshold));

                // Check if minimum distance + threshold is surpassed and perform teleportation
				if (true || currentPosition > teleportRaiseThreshold)
                {
                    // Reset buffer
                    for (int i = 0; i < velocityCalcBuffer; i++)
                    {
                        teleportYBuffer.Enqueue(100f);
                        teleportYBuffer.Dequeue();
                    }

                    // Calculate teleport position, preserve Y
                    Vector3 teleportPosition = instantiatedThrowBall.gameObject.transform.position;
                    // DISABLED DUE TO NEW MAP -- teleportPosition.y = transform.parent.position.y;

                    // Spawn objects for visual indication
                    //Destroy(instantiatedTeleporterOld);
                    //Destroy(instantiatedTeleporterNew);
                    //instantiatedTeleporterOld = Instantiate(teleporterOld.gameObject, this.transform.position, Quaternion.identity) as GameObject;
                    //instantiatedTeleporterNew = Instantiate(teleporterNew.gameObject, teleportPosition, Quaternion.identity) as GameObject;
					instantiatedTeleporterNew.SetActive(false);

                    // Teleport
                    transform.parent.position = teleportPosition;

					// copy Velocity
					transform.parent.GetComponent<Rigidbody>().velocity = instantiatedThrowBall.gameObject.GetComponent<Rigidbody>().velocity;

					// Update teleport threshold
					InitiateTeleportThreshold();
					InitiateTeleportThreshold();
					InitiateTeleportThreshold();
					InitiateTeleportThreshold();
					InitiateTeleportThreshold();

					ReturnBallToHand ();
                } else
                {
                    // Otherwise, enqueue
                    teleportYBuffer.Enqueue(currentPosition);
                    teleportYBuffer.Dequeue();
                }
            }
            
            // Skip all other interaction
            return;
        }

        // Store velocity and throw out last one
		if (controller.connected) {
			previousVelocities.Enqueue (controller.velocity);
			previousVelocities.Dequeue ();
		}

		// Set bool if Z Trigger is pushed
		sliderSphere.GetComponent<SliderInteract> ().zTriggerIsActive = controller.GetPress(triggerButton);

        // Check if grip button is held down to charge up ball-tossing
		if (controller.GetPress(triggerButton)) {
            if (currentCharge <= chargeupTime)
            {
                currentCharge++;
            } else
            {
                fullyCharged = true;
            }
            // Perform haptic feedback
            HapticPulseDo();

            //throwBall.transform.GetComponent<FixedJoint> ().connectedBody = null;
            //throwBall.transform.GetComponent<Rigidbody> ().useGravity = true;
        }


		if (controller.GetPressUp(triggerButton))
        {
            if (fullyCharged)
            {
				float extraMultiplier = 1.0f;
				if (controller.GetPress(dpadDownButton)) {
					extraMultiplier = 0.5f;
				}

                GameObject newThrowBall = Instantiate(throwBall.gameObject, this.transform.position, Quaternion.identity) as GameObject;
				newThrowBall.transform.GetComponent<Rigidbody>().velocity = averageVelocity(previousVelocities) * velocityMultiplier * extraMultiplier;
                instantiatedThrowBall = newThrowBall;
                hasBeenThrown = true;
                teleportIndicator.SetActive(true);
            }

            currentCharge = 0;
            fullyCharged = false;
        }

		if (controller.GetPress (dpadDownButton)) {
			gameObject.transform.Find ("ChargeIndicator").GetComponent<ChargeBallBehavior> ().chargeFactor = 0f;
		} else {
			gameObject.transform.Find ("ChargeIndicator").GetComponent<ChargeBallBehavior> ().chargeFactor = (float)currentCharge / (float)chargeupTime;
		}
    }


	void InitiateTeleportThreshold() {
		// Find out the distance to peform the teleport
		Vector3 firstPos = this.transform.position;
		teleportRaiseThreshold = firstPos.y + teleportHandRaiseDistance;

		// Set indicator
		instantiatedTeleporterNew.SetActive (true);

		return;
	}

    void ReturnBallToHand()
    {
        sliderSphere.SetActive(true);
        hasBeenThrown = false;
        Destroy(instantiatedThrowBall);
        teleportIndicator.SetActive(false);
    }

    Vector3 averageVelocity(Queue velocityQueue)
    {
        float length = (float)velocityQueue.Count;
        Vector3 average = Vector3.zero;
        foreach (Vector3 velocity in velocityQueue)
        {
            average += velocity;
        }
        return average / length;
    }

    float averagePosition(Queue positionQueue)
    {
        float length = (float)positionQueue.Count;
        float average = 0f;
        foreach (float position in positionQueue)
        {
            average += position;
        }
        return average / length;
    }


    /**
     * Trigger a haptic pulse with force of the time.
     */
    void HapticPulseDo()
    {
        float timeFalloffFactor = (fullyCharged ? 1.0f : Mathf.Pow((float)currentCharge / (float)chargeupTime, 3f));

        HapticPulseDo(timeFalloffFactor);
    }

    void HapticPulseDo(float factor)
    {
        float inBounds = Mathf.Max(0f, Mathf.Min(1f, factor));

        // Calculate power based on factor 0.0..1.0
        ushort pulseForce = Convert.ToUInt16(Mathf.Min(MAX_PULSE * inBounds, MAX_PULSE) * 0.2f);

        // Send calculated pulse force to controller for this frame
        controller.TriggerHapticPulse(pulseForce);
    }

}
