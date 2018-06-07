using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public int satsSpawnedThisWave;
    public int satsCaught = 0;
    private int satsSpawnedOverall;
    private LevelManager levelBoss;
    public GameObject[] satelitteToSpawn;

    public WaveTemplate currentWave;

    [Space(10)]
    [Header("Wave modifiers")]
    [SerializeField]
    private float satSpawnModifier = 2;
    [SerializeField]
    private float standardSpawnTime = 1;
    [SerializeField]
    private float standardWaveDuration = 10;
    [SerializeField]
    private float waveDurationModifier = 5;
    [SerializeField]
    private int maxSatsPerSpawn = 2;

    private float spawnCounter = 0;
    private float timerCounter = 0;


    public float WaveTimeLeft()
    {
        return Mathf.Max(0, currentWave.maxWaveTimer - timerCounter);
    }    

    public bool WithinTimer()
    {
        //checks if there is still time before next wave
        if (timerCounter < currentWave.maxWaveTimer)
            return true;
        else
            return false;
    }

    public bool SatelittesLeft()
    {
        if (satsCaught < satsSpawnedThisWave || satsSpawnedThisWave < currentWave.totalSattelitestoSpawn)       
            return true;
        
        else
            return false;
    }

    public int SatsToSpawn()
    {
        return currentWave.totalSattelitestoSpawn - satsSpawnedThisWave;
    }

    public void StartWaves()
    {
        levelBoss = gameObject.GetComponent<LevelManager>();
        EnterWave(0);
    }

    public void EnterWave(int waveNum)
    {
        satsCaught = 0;
        satsSpawnedThisWave = 0;
        timerCounter = 0;

        int sats = Mathf.RoundToInt(waveNum * satSpawnModifier + 3);
        float spawnTime = standardSpawnTime;
        float waveTimer = standardWaveDuration + waveDurationModifier * sats;

        currentWave = new WaveTemplate(sats, spawnTime, waveTimer, waveNum);

        Debug.Log("Wave " + currentWave.waveNumber + "Spawns: " + sats);

        if (currentWave != null)
        {
            Debug.Log("Entered wave " + waveNum);
        }
    }

    //public void ResetCounters()
    //{
    //    satsCaught = 0;
    //    satsSpawnedThisWave = 0;
    //    timerCounter = 0;
    //    spawnCounter = 0;
    //}

    public void ExecuteWave(GameObject planet, float radius)
    {
        timerCounter += Time.deltaTime;
        // Debug.Log("Spawn Counter: " + spawnCounter);

        if (SatsToSpawn() > 0)
        {
            spawnCounter += Time.deltaTime;
            if (spawnCounter > currentWave.spawnTimer)
            {
                int maxSatsToSpawn = Mathf.Min(SatsToSpawn(), maxSatsPerSpawn);
                int amountOfSats = Random.Range(1, maxSatsToSpawn);

                SpawnSats(amountOfSats,planet, radius);
            }
        }
        else
            spawnCounter = 0;
    }

    private void SpawnSats(int value, GameObject _planet, float radius)
    {
        for (int i = 0; i < value; i++)
        {
            int rand = Random.Range(0, satelitteToSpawn.Length-1);
            satelitteToSpawn[rand].GetComponent<Satellite>().planet = _planet;
            satelitteToSpawn[rand].GetComponent<Satellite>().levelManager = levelBoss;
            Instantiate(satelitteToSpawn[rand], Random.insideUnitSphere * radius, Quaternion.identity);
            satsSpawnedThisWave++;
            satsSpawnedOverall++;
            // Debug.Log("Spawned: " + satsSpawnedThisWave + ". To spawn: " + SatsToSpawn());
            spawnCounter = 0;
        }
    }

    public void ExitWave()
    {
        int nextWave = currentWave.waveNumber + 1;
        EnterWave(nextWave);
    }

}
