using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableTransparency : MonoBehaviour {

	public GameObject player;
	public float transparentDistanceMax;

	public float transperency;

	private Renderer rend;
	private Shader glow;

	// Use this for initialization
	void Start () {
		//rend = gameObject.GetComponent<Renderer> ();
		//glow = Shader.Find("TeleportAreaVisibleBright");
	}
	
	// Update is called once per frame
	void Update () {
		//rend.material.SetFloat ("SeeThru", transperency);
		/*
		// Get distance to player
		Vector3 posOfPlayer = player.transform.position;
		Vector3 posOfTable = this.transform.position;
		float distance = Vector3.Distance(posOfTable, posOfPlayer);

		// Adjust transparency based on distance
		Color col = this.gameObject.GetComponent<Renderer> ().material.color;
		//col.a = 0.1f + (Mathf.Min(0.9f, distance * 0.01f));
		col.a = transperency;
		this.gameObject.GetComponent<Renderer> ().material.color = col;

		Debug.Log (col.a);
		*/
	}
}
