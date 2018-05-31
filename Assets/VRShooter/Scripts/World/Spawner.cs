using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    private Vector3[] spawnPositions = new Vector3[1];
    public GameObject Enemy;

    //Time between spawning enemies
    public float SpawnTime = 3f;

    // Maximum allowed enemies at once
    public int maxEnemies = 10;

    // Scaled factor for adjusted world space
    public Vector3 positionFactor;


	// Use this for initialization
	void Start ()
    {
        //Initiate all possible Spawn Points
        spawnPositions[0] = new Vector3(-21.6f, 0f, -4.9f);
        /*
        spawnPositions[0] = new Vector3(-76.5f, 0f, -35.5f);
        spawnPositions[1] = new Vector3(-44.5f, 0f, -35.5f);
        spawnPositions[2] = new Vector3(-9.5f, 0f, -35.5f);
        spawnPositions[3] = new Vector3(17.5f, 0f, -39.5f);
        spawnPositions[4] = new Vector3(60f, 0f, -41.5f);
        spawnPositions[5] = new Vector3(60f, 0f, -60f);
        spawnPositions[6] = new Vector3(60f, 0f, -80f);
        spawnPositions[7] = new Vector3(60f, 0f, -100f);
        spawnPositions[8] = new Vector3(60f, 0f, -120f);
        spawnPositions[9] = new Vector3(20f, 0f, -120f);
        spawnPositions[10] = new Vector3(-15f, 0f, -120f);
        spawnPositions[11] = new Vector3(-80f, 0f, -120f);
        spawnPositions[12] = new Vector3(-80f, 0f, -110f);
        spawnPositions[13] = new Vector3(-85.5f, 0f, -80f);
        spawnPositions[14] = new Vector3(-85.5f, 0f, -40f);
        spawnPositions[15] = new Vector3(-30f, 0f, -70f);
        spawnPositions[16] = new Vector3(87f, 0f, -65f);
        spawnPositions[17] = new Vector3(17.5f, 0f, -67.5f);
        spawnPositions[18] = new Vector3(17.5f, 0f, -100.5f);
        spawnPositions[19] = new Vector3(-15f, 0f, -100.5f);
        */

        //Debug purpose to test without VR
        //Ready();
    }

    // Update is called once per frame
    void Update () {

	}

    void Ready()
    {
        Destroy(GameObject.FindGameObjectWithTag("Table"));
        InvokeRepeating("Spawn", SpawnTime, SpawnTime);
    }

    void Spawn()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>().CurrentHealth <= 0f)
            return;

        // TODO
        //if (currentEnemyCount >= maxEnemies)
        //    return;

        int spawnPointIndex = Random.Range(0, spawnPositions.Length - 1);

        // Spawn an enemy at a random spawn position
        Vector3 spawnPosition = Vector3.Scale(positionFactor, spawnPositions[spawnPointIndex]);
        Instantiate(Enemy, spawnPosition, Quaternion.identity);
    }
}
