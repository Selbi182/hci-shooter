using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableChildController : MonoBehaviour {

	public int currentTableIndex;

	// Use this for initialization
	void Start () {
		currentTableIndex = 0;
		updateActiveTables ();
	}

	// Global function to increase table index
	void increaseTableIndex() {
		currentTableIndex++;
		updateActiveTables ();
	}

	void updateActiveTables() {
		for (int i = 0; i < this.gameObject.transform.childCount; i++) {
			bool active = (i == currentTableIndex);
			this.gameObject.transform.GetChild (i).gameObject.SetActive (active);
		}
	}
}
