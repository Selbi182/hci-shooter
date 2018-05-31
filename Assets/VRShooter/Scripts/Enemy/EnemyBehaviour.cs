using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

	private Transform player;
	private NavMeshAgent navigation;
    private Animator animator;
    public AudioClip DeathSound;

    private int continueMovement;


	// Use this for initialization
	void Start ()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navigation = GetComponent<NavMeshAgent>();
	    animator = GetComponent<Animator>();
        //InvokeRepeating("Roaming", 1.0f, 2.0f);

        continueMovement = 0;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    navigation.SetDestination(player.position);
	}

    void FixedUpdate()
    {
        if (continueMovement > 0) {
            continueMovement--;
            if (continueMovement == 0)
            {
                navigation.Resume();
            }
        }
    }

    void Die()
    {
        navigation.Stop();

        continueMovement = 9999;
        
        animator.SetTrigger("EnemyDead");
        //AudioSource.PlayClipAtPoint(DeathSound, transform.position);
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.5f);
    }

    void Hit()
    {
        navigation.Stop();
        continueMovement = 34;
    }
}
