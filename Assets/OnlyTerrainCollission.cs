using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyTerrainCollission : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag ("TeleporterBall")) {
			Physics.IgnoreCollision (collision.gameObject.GetComponent<Collider> (), GetComponent<Collider> ());
		}
	}

}
