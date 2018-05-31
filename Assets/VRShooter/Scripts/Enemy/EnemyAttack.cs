using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{

    public float Damage = 10;
    private Animator animator;
    public AudioClip AttackSound;
    public AudioClip RoamingSound;

    //Time between two attacks
    public float attackRate = 1.367f;

	// Use this for initialization
	void Start ()
	{
	    animator = this.transform.parent.GetComponent<Animator>();
        InvokeRepeating("Roaming", 1.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
	    if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>().CurrentHealth <= 0)
	    {
	        animator.SetBool("PlayerDead", true);
            //this.transform.parent.GetComponent<NavMeshAgent>().Stop();
            this.transform.parent.GetComponent<NavMeshAgent>().SetDestination(this.transform.parent.position);
            CancelInvoke();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("PlayerInRange", true);
            CancelInvoke();
            InvokeRepeating("Attack", 1.367f, attackRate);
        }


    }
    void Roaming()
    {
        AudioSource.PlayClipAtPoint(RoamingSound, transform.position, 0.8f);
    }

    void Attack(/*Collider other*/)
    {
        //if (other.gameObject.CompareTag("Player")) { 
            AudioSource.PlayClipAtPoint(AttackSound, transform.position);
            GameObject.FindGameObjectWithTag("Player").SendMessage("Hit", Damage);
                
        //}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("PlayerInRange", false);
            CancelInvoke();
            InvokeRepeating("Roaming", 1.0f, 2.0f);
        }
    }

    void GameOver()
    {
        CancelInvoke();
    }
}
