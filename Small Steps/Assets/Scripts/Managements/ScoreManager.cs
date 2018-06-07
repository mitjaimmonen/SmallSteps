using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    float currentScore;
    int totalSatsCaught;

    public float GetScore()
    {
        return currentScore;
    }

    public void AddToScore(float value, float timeMod)
    {
        currentScore += value + 8 * timeMod;
    }

    public void ClearScore()
    {
        currentScore = 0;
    }


    public void SaveScore()
    {
        //save score to player prefs!
    }

    public void ReadScore()
    {
        //read score from player prefs
    }

    public void CaughtSatellite()
    {
        totalSatsCaught++;
    }

}
