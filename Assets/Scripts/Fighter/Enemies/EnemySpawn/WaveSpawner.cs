using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public List<SpawnEnemy> enemies = new List<SpawnEnemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Collider2D[] spawnTrigger;
    public Transform[] spawnLocations;
    public int spawnIndex;

    public int waveDuration;
    public bool pauseSpawner = true;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pauseSpawner)
            return;
        
        if (spawnTimer <= 0)
        {
            //spawn an enemy
            if (enemiesToSpawn.Count > 0)
            {
                spawnIndex = Random.Range(0, spawnLocations.Length);
                
                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocations[spawnIndex].position, Quaternion.identity); // spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // and remove it
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0; // if no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }

        if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
        {
            currWave++;
            GenerateWave();
        }
    }

    public void GenerateWave()
    {
        pauseSpawner = false;
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
    }
    public void PauseWave()
    {
        pauseSpawner = true;
    }

    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.

        // repeat... 

        //  -> if we have no points left, leave the loop

        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

}

[System.Serializable]
public class SpawnEnemy
{
    public GameObject enemyPrefab;
    public int cost;
}
