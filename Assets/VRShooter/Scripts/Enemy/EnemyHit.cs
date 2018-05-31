using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour {

    public float Health;
    private Animator animator;
    public AudioClip HitSound;

    private bool isDying;

    // Use this for initialization
    void Start () {
        isDying = false;
        animator = this.transform.parent.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!isDying && other.gameObject.CompareTag("Projectile"))
        {
            this.transform.parent.SendMessage("Hit");
            animator.SetTrigger("EnemyHit");
            AudioSource.PlayClipAtPoint(HitSound, transform.position);
            Health = Health - GameObject.FindGameObjectWithTag("Weapon").GetComponent<RapidFire>().Damage;
            if (Health <= 0f)
            {
                this.transform.parent.SendMessage("Die");

                isDying = true;
            }
        }
    }
}
