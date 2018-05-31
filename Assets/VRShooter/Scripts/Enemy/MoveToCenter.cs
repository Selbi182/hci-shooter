using UnityEngine;
using System.Collections;

public class MoveToCenter : MonoBehaviour {

    public float moveSpeed = 3F;
    public int rotationSpeed = 5;
    private float currentLifeTime;
    private Vector3 rotationFactor;

    // Use this for initialization
    void Start () {
        rotationFactor = new Vector3(0, rotationSpeed, 0);
        Vector3 pos = gameObject.transform.position * -moveSpeed;
        gameObject.transform.GetComponent<Rigidbody>().AddForce(pos);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationFactor);
    }

    void FixedUpdate()
    {
        currentLifeTime += 1.0F * Time.deltaTime;

        
        //if (currentLifeTime >= lifeTime)
        //{
            //Debug.Log("fixed update " + currentLifeTime);
        //    Destroy(gameObject);
        //}
    }
}
