using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RapidFire : MonoBehaviour {
    // PUBLIC
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonHeld = false;
    public bool fullAutomatic = true;

    public GameObject laser;
    public GameObject lasergun;
    public GameObject particles;
    public GameObject ammoDisplay;
    public GameObject damageDisplay;

    public AudioClip LaserSound;

    public int fireRate = 10;
    public float projectileSpeed = 2500.0F;
    public float spreadFactor = 20.0F;
	public int maxAmmo;
    public int currentAmmo;
    public int ammoCost;
    public float reloadThreshold = 5F;
    public int reloadVolume = 1;
    public float Damage = 10f;


    // PRIVATE
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

    private System.Random rnd;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller {
        get {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }
    
    private int pulseCount, pulseCountMax;
    private const ushort MAX_PULSE = 3999;

    private Vector3 reloadStartPos;

    

    // Use this for initialization
    void Start () {
        rnd = new System.Random();

        // Set ReadyToFire
        pulseCount = 0;
        pulseCountMax = pulseCount;

        // Attach live controller
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        // Show start ammo
        //currentAmmo = maxAmmo;
        UpdateTextDisplays();

        // Show start damage
        damageDisplay.GetComponent<TextMesh>().text = CalculateDamage().ToString();
    }


    // Update is called once per frame
    void Update () {

		// TODO immer perfekt
		//spreadFactor = 0f;

        // Check if Controller is available first
        if (controller == null)
            return;

        // Get ammo count
        //int currentAmmo = int.Parse(ammoDisplay.GetComponent<TextMesh>().text);

        // Check if grip buttons are pressed for reload
        if (controller.GetPressDown(gripButton))
        {
            particles.SetActive(false);
            reloadStartPos = gameObject.transform.position;

			gameObject.transform.Find("LaserGunPrefab").GetComponent<VisualsHandler>().isReloading = true;
        }

        if (controller.GetPress(gripButton))
        {
            // Check if the controller has traveled a certain distance since the grip button was held down first
            Vector3 currentPos = gameObject.transform.position;
            float distance = Vector3.Distance(reloadStartPos, currentPos);

            // If it has, refill ammo
            if (distance > reloadThreshold)
            {
                reloadStartPos = currentPos;

				if (currentAmmo < maxAmmo)
                {
                    // Perform haptic feedback
                    SetPulseTo(5);
                    HapticPulseDo();

                    // Set ammo
                    currentAmmo = Math.Min(currentAmmo + reloadVolume, maxAmmo);
                }
            }

            // Skip firing
            UpdateTextDisplays();
            return;
        }
		gameObject.transform.Find("LaserGunPrefab").GetComponent<VisualsHandler>().isReloading = false;

        // Get trigger controller input for this frame (down = just pressed ; held = held down)
        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonHeld = controller.GetPress(triggerButton);

        // Only allow firing when above minimum required ammo

        if (currentAmmo >= ammoCost)
        {
            // Decide if this is a semi- or full automatic
            bool trigger = fullAutomatic ? triggerButtonHeld : triggerButtonDown;
            if (triggerButtonUp)
            {
                particles.SetActive(false);
            }
			gameObject.transform.Find("LaserGunPrefab").GetComponent<VisualsHandler>().isFullAutomatic = fullAutomatic;

            // Fire if you can / Peform haptic feedback and block refiring for X frames
            if (trigger && ReadyToFire())
            {
                // Set fire rate
                SetPulseTo(fireRate);

                // Show particles
                particles.SetActive(true);

                // Spawn laser
				//TODO als instanz von World->Projectiles erstellen
                GameObject newLaser = Instantiate(laser.gameObject, lasergun.transform.position, lasergun.transform.rotation) as GameObject;
                //AudioSource.PlayClipAtPoint(LaserSound, transform.position);
                // Applay random spread
                Vector3 rotation = new Vector3(0, 0, 0);
                rotation.x += GetBalancedRnd();
                rotation.y += GetBalancedRnd();
                rotation.z += GetBalancedRnd();
                newLaser.transform.Rotate(rotation);

                // Apply projectile speed
				//float scaleSpeed = projectileSpeed * 0.0001f;
				//newLaser.GetComponent<Rigidbody>().velocity = Vector3.Scale(newLaser.transform.forward, new Vector3(scaleSpeed, scaleSpeed, scaleSpeed));
                newLaser.GetComponent<Rigidbody>().AddForce(newLaser.transform.forward * projectileSpeed);

                // Reduce ammo
                currentAmmo = Math.Max(currentAmmo - ammoCost, 0);
            }
        } else {
            particles.SetActive(false);
        }

        // Perform haptic feedback
        HapticPulseDo();

        // Update text displays;
        UpdateTextDisplays();
    }
    
    void UpdateTextDisplays()
    {
		CalculateAmmoCost ();
		if (ammoCost <= 0) {
			ammoCost = 1;
		}

		String ammoChar = "-";
		String ammoLineBreak = "\n";

		String ammoText = "";

		int tmp = 0;

		// Show ammo itself
		for (int i = 0; i < currentAmmo; i++) {
			ammoText += ammoChar + ammoLineBreak;
			tmp++;
		}

		// Insert line breaks
		for (int i = currentAmmo; i > 0; i -= ammoCost) {
			ammoText = ammoText.Insert (i * 2, ammoLineBreak);
			tmp++;
		}

		// TODO LINEBREAKS
		// *2, da Linebreaks auch als Char zählen

		float newLineSpacing = ((float)(currentAmmo) / (float)(tmp * 10));

		//ammoText += ammoLineBreak + ammoLineBreak + ammoLineBreak +  currentAmmo.ToString ();

		// Display ammo and adjust background
		ammoDisplay.GetComponent<TextMesh>().text = ammoText;
		ammoDisplay.GetComponent<TextMesh>().lineSpacing = newLineSpacing;

        //ammoDisplay.GetComponent<TextMesh>().text = currentAmmo.ToString() + " (-" + CalculateAmmoCost().ToString() + ")";
        damageDisplay.GetComponent<TextMesh>().text = CalculateDamage().ToString();
    }

    int CalculateDamage()
    {
        // Higher projectile speed -> More damage
        float baseDamage = 0.005f;

        int adjustedDamage =  (int)Mathf.Round( (float)projectileSpeed * baseDamage );

        Damage = adjustedDamage;
        
        return adjustedDamage;
    }

    int CalculateAmmoCost()
    {
        // Lower spread factor -> Higher ammo cost
        // Higher projectile speed -> Higher ammo cost

		// Per 2000 Projectile Speed -> 1 Score (min 0, max 5)
		// Per -10 Degrees Spread Factor -> 1 Score (min 0, max 5)
		int projectileSpeedScore = (int) (projectileSpeed / 2000f);
		int spreadFactorScore = (int) (Mathf.Abs(spreadFactor - 50f) / 10f);

		// TODO tmp perfekt
		//spreadFactorScore = 0;

		int score = projectileSpeedScore + spreadFactorScore;

		// Map Score to actual ammo cost (min 0, max 30)
		ammoCost = Math.Max(0, Math.Min(30, score));


        //float projectileSpeedAmmoCost = projectileSpeed * 0.0001f;
        //float spreadFactorAmmoCost = (float)(100 - (2 * (int)spreadFactor));
        //int adjustedAmmoCost =  (int)Mathf.Round(spreadFactorAmmoCost * projectileSpeedAmmoCost);

        //int adjustedAmmoCost = (int)Mathf.Ceil( baseAmmoCost * (float)projectileSpeed - (float)spreadFactor) );

        //ammoCost = adjustedAmmoCost / 10;
    
		return ammoCost;
    }

    float GetBalancedRnd()
    {
        return (float) (rnd.NextDouble() - 0.5F) * spreadFactor;
    }


    void SetPulseTo(int pulseCount)
    {
        this.pulseCount = 0;
        this.pulseCountMax = pulseCount;
    }

    /**
     * Check if the gun is ready to fire again (by checking if the pulseCount matches the max pulse count).
     */
    bool ReadyToFire()
    {
        return pulseCount >= pulseCountMax;
    }

    /**
     * Trigger a haptic pulse with force of the time.
     */
    void HapticPulseDo()
    {
        // Only if the gun is currently being fired.
        if (!ReadyToFire())
        {
            // Increase pulse factor
            pulseCount++;

            // Calculate fall-off factor over time and then actual power (capping off at MAX_PULSE)
            float timeFalloffFactor = 1 - ((float)pulseCount / pulseCountMax);
            ushort pulseForce = Convert.ToUInt16(Mathf.Min(MAX_PULSE * timeFalloffFactor, MAX_PULSE));

            // Send calculated pulse force to controller for this frame
            controller.TriggerHapticPulse(pulseForce);
        }
    }
}
