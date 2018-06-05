using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public ScoreManager scoreManager;
    public WaveManager waveManager;
    public float planetRadius;

    public GameObject planet;
    public GameObject player;

    public float ScoreForDisplay()
    {
        return 0;
        //return scoreManager.GetScore();
    }

    public int WaveNumberForScore()
    {
        return waveManager.currentWave.waveNumber;
    }

    private void Awake()
    {
    }

    private void Start()
    {
        waveManager.StartWaves();
        //scoreManager.ClearScore();
    }

    private void Update()
    {
        ManageWaves();
    }

    void ManageWaves()
    {
         if (waveManager.SatelittesLeft())
        {
            waveManager.ExecuteWave(planet, planetRadius);
        }
        else
        {
           waveManager.ExitWave();
        }
    }

    public void OnSatelitteCaught()
    {
        //add to score
        //waveManager.satsCaught++;
    }

}
