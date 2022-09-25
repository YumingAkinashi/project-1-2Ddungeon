using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    WaveSpawner waveSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = GetComponentInParent<WaveSpawner>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isASectionSpawning && collision.gameObject.CompareTag("Player"))
        {
            waveSpawner.GenerateWave();
            GameManager.instance.isASectionSpawning = true;
            Debug.Log("spawner triggered");
        }
        else if(GameManager.instance.isASectionSpawning && collision.gameObject.CompareTag("Player"))
        {
            waveSpawner.PauseWave();
            GameManager.instance.isASectionSpawning = false;
            Debug.Log("spawner not triggered");
        }
    }
}
