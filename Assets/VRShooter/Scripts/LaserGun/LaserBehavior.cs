using UnityEngine;
using System.Collections;

public class LaserBehavior : MonoBehaviour {

    public float lifeTime = 3F;
    private float currentLifeTime;
    private Vector3 rotationFactor = new Vector3(0, -20, 0);

	public bool canDestroyEverything = false;

	private GameObject gameObjectToDestroy;

	private bool setToDie = false;
	private float maxDyingTime = 60f;
	private float dyingTime;
	private Vector3 startVel;

    // Use this for initialization
    void Start()
    {
		startVel = this.GetComponent<Rigidbody>().velocity;
		dyingTime = maxDyingTime;
		gameObjectToDestroy = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotationFactor);
		transform.Translate(startVel);

		if (setToDie) {
			dyingTime -= 1f;

			if (dyingTime <= 0) {
				Destroy (gameObject);
				return;
			}

			float scale = (dyingTime / maxDyingTime);
			Vector3 scaleAsVector = new Vector3 (scale, scale, scale);
			this.transform.localScale = scaleAsVector;

			GetComponent<Light> ().range = scale;


			//transform.Translate(Vector3.Scale(startVel, scaleAsVector));
		}

		// Destroy object from last frame
		if (gameObjectToDestroy != null) {
			Destroy (gameObjectToDestroy);
			gameObjectToDestroy = null;
		}

		currentLifeTime += 1.0F * Time.deltaTime;
		if (currentLifeTime >= lifeTime)
		{
			//Debug.Log("fixed update " + currentLifeTime);
			Destroy(gameObject);
		}
    }


	void OnTriggerEnter(Collider col) {
		Debug.Log (col.tag);

		// Destroy collided, if it can be
		Transform currentParent = col.gameObject.transform;
		bool yesDestroy = false;
		while (currentParent != null) {
			if (currentParent.CompareTag ("World")) {
				yesDestroy = true;
				break;
			}
			currentParent = currentParent.transform.parent;
		}
		if (!yesDestroy) {
			return;
		}

		if (col.CompareTag ("Player")) {
			return;
		}

		setToDie = true;
		this.GetComponent<Collider> ().enabled = false;
		this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		this.transform.Find ("Laser").GetComponent<MeshRenderer> ().enabled = false;

		if (canDestroyEverything || col.gameObject.CompareTag ("Destroyable")) {
			Destroy (col.gameObject);
		}
	}
}
