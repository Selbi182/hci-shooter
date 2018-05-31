using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTarget : MonoBehaviour {

	public GameObject gun;

	void OnDestroy() {
		gun.GetComponent<RapidFire> ().maxAmmo = 1000;
		gun.GetComponent<RapidFire> ().currentAmmo = 100;
	}
}
