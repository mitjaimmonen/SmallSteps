using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHandler : MonoBehaviour
{
    public Text waveInfoText;
    public Text waveTimeText;
    public Text scoreText;
    public Text caughtSatellitesText;
    public Slider healthbarSlider;
    public LevelManager gameMaster;

    private PlayerMachine player;
    private int oldIndex;
    private float oldHealth;
    private Image sliderBackground;

    void Awake()
    {
        var tags = GameObject.FindGameObjectsWithTag("Player");
        foreach(var temp in tags)
        {
            player = temp.GetComponent<PlayerMachine>();
            if (player)
                break;
        }

    }
    private void Update()
    {

        if (oldHealth != player.CurrentHealth)
        {
            float healthPercentage = player.CurrentHealth / player.MaxHealth;
            healthbarSlider.value = healthPercentage;
            oldHealth = player.CurrentHealth;
        }

        scoreText.text = gameMaster.scoreManager.GetScore().ToString("f0");
        waveInfoText.text = "Wave: " + (gameMaster.WaveNumberForScore() + 1);
        waveTimeText.text = "Bonus: " + gameMaster.waveManager.WaveTimeLeft().ToString("f0");       
        caughtSatellitesText.text = gameMaster.waveManager.satsCaught.ToString() + "/" + gameMaster.waveManager.currentWave.totalSattelitestoSpawn.ToString();


        // if (gameMaster.scoreManager.GetScoreBonus() > 1)
        // {
        //     if (gameMaster.scoreManager.GetScoreBonus() % 1 == 0)
        //     {
        //         scoreTimerText.text = gameMaster.scoreManager.GetScoreTimer().ToString("f0");
        //         scoreBonusText.text = "x" + gameMaster.scoreManager.GetScoreBonus().ToString("f0");
        //     }
        //     else
        //     {
        //         scoreTimerText.text = gameMaster.scoreManager.GetScoreTimer().ToString("f0");
        //         scoreBonusText.text = "x" + gameMaster.scoreManager.GetScoreBonus().ToString("f1");
        //     }

        

        // else
        // {
        //     scoreTimerText.text = " ";
        //     scoreBonusText.text = " ";
        // }
    }

}
