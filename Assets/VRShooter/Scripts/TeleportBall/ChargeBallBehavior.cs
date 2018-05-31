using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBallBehavior : MonoBehaviour {

    public float chargeFactor = 0f;

    private Vector3 rotationFactor = new Vector3(10, 20, 30);
    
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (chargeFactor > 0f)
        {
            gameObject.transform.Find("Material").transform.Rotate(rotationFactor * chargeFactor);
            gameObject.transform.Find("Material").GetComponent<MeshRenderer>().enabled = true;
            
        } else {
            gameObject.transform.Find("Material").GetComponent<MeshRenderer>().enabled = false;
        }

        if (chargeFactor >= 1f)
        {
            gameObject.transform.Find("FX").gameObject.SetActive(true);
        } else {
            gameObject.transform.Find("FX").gameObject.SetActive(false);
        }

    }
}
