using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateStats : MonoBehaviour {

	/* SLIDER für später
     * oben: feuerrate <-> präzision (toggle semi/full)
     * seite innen: schaden <-> projektilgeschwindigkeit (slider größe)
     * seite außen: magazingröße <-> reload volume/distance (slider max ammo)
     */

	// Slider GameObjects with translated values (percentages)
	public GameObject sliderValueLeft;
	public GameObject sliderValueTop;
	public GameObject sliderValueRight;

	// Upgrades Toggler
	public bool enableLeftSlider  = false;
	public bool enableRightSlider = false;
	public bool enableTopSlider   = false;

	// Various default min/max values for stats
	public int minFireRate = 1;
	public int maxFireRate = 60;

	public int minProjectileSpeed = 100;
	public int maxProjectileSpeed = 10000;

	public int minSpreadFactor = 0;
	public int maxSpreadFactor = 90;

	private float defaultMultiply = 0.5f;


	// RapidFire script responsible for the weapon behavior
	private RapidFire weapon;


	// Use this for initialization
	void Start() {
		weapon = transform.GetComponent<RapidFire>();

		weapon.fireRate =        CalculateWeaponStat(minFireRate,        maxFireRate,        defaultMultiply);
		weapon.projectileSpeed = CalculateWeaponStat(minProjectileSpeed, maxProjectileSpeed, defaultMultiply);
		weapon.spreadFactor =    CalculateWeaponStat(minSpreadFactor,    maxSpreadFactor,    defaultMultiply);

		// TODO default perfekt
		//weapon.spreadFactor = 0f;

	}
	
	// Update is called once per frame
	void Update() {
		weapon.fireRate =        CalculateWeaponStat(minFireRate,        maxFireRate,        sliderValueLeft);
		weapon.projectileSpeed = CalculateWeaponStat(minProjectileSpeed, maxProjectileSpeed, sliderValueTop);
		weapon.spreadFactor =    CalculateWeaponStat(minSpreadFactor,    maxSpreadFactor,    sliderValueRight);

		// Special semi-auto mode
		if (weapon.fireRate == maxFireRate && weapon.projectileSpeed == maxProjectileSpeed && weapon.spreadFactor == minSpreadFactor) {
			weapon.fullAutomatic = false;
		} else {
			weapon.fullAutomatic = true;
		}

		// Show or don't show upgrades
		sliderValueLeft.SetActive(enableLeftSlider);
		sliderValueRight.SetActive(enableRightSlider);
		sliderValueTop.SetActive(enableTopSlider);
	}

	// Calculate the correct value between the min and max value of a stat, based on the percentage
	int CalculateWeaponStat(int min, int max, float multiplierRaw) {
		//float multiplier = Mathf.Log(multiplierRaw + 9f);
		float multiplier = multiplierRaw;

		int difference = max - min;
		float adjusted = (float)difference * multiplier;

		return (int) adjusted + min;
	}

	// Overloaded method
	int CalculateWeaponStat(int min, int max, GameObject slider) {
		return CalculateWeaponStat(min, max, TranslatedValueOf(slider));
	}

	// Get "translatedValue" from a slider
	float TranslatedValueOf(GameObject slider) {
		return slider.GetComponent<PositionToWeaponAttribute>().translatedValue;
	}

	/**
	 * Global function to activate Upgrades
	 */
	void activateUpgrade(int upgradeNum) {
		if (upgradeNum == 1) {
			enableLeftSlider = true;
		} else if (upgradeNum == 2) {
			enableRightSlider = true;
		} else if (upgradeNum == 3) {
			enableTopSlider = true;
		}
	}
}
