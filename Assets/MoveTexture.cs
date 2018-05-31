using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTexture : MonoBehaviour {

	public float scrollSpeed = 0.5f;
	public float glowSpeed = 2.0f;
	private float offset;

	// Use this for initialization
	void Start () {
		offset = 0f;
	}

	// Update is called once per frame
	void Update () {
		offset += (Time.deltaTime*scrollSpeed)/10.0f;
		GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (offset, 0));

		// Adjust glow
		float glow = (Mathf.Sin (offset * glowSpeed) * 0.7f) + 0.3f;
		GetComponent<Renderer> ().material.SetFloat ("_SeeThru", glow);
	}
}
