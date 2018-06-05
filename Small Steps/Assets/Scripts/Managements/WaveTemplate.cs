using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTemplate {

    public int totalSattelitestoSpawn;
    //time to spawn satellites
    public float spawnTimer;
    //max time to finish the wave, not sure if needed? moybe for score
    public float maxWaveTimer;
    public int waveNumber;

    public WaveTemplate (int _satelittes, float _spawnTimer, float _waveTimer, int _waveNumber)
    {
        totalSattelitestoSpawn = _satelittes;
        spawnTimer = _spawnTimer;
        maxWaveTimer = _waveTimer;
        waveNumber = _waveNumber;
    }
}
