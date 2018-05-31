using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizedCollission : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
       // Debug.Log(collision.transform.name);
    }
    /*
    void OnTriggerEnter(Collision col)
    {
        // Only allow collision on the map and nothing else. All map tiles are exactly two generations below "dungeonRoot".
        // (This is dirty, but it's the most performant option I know of.)
        Debug.Log(col.transform.name);

        if (!col.gameObject.transform.parent.parent.name.Equals("dungeonRoot"))
        {
            Physics.IgnoreCollision(col.gameObject.transform.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
    */
}
