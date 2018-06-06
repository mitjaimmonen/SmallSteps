using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHandler : MonoBehaviour
{
    public Text waveInfoText;
    public Text scoreText;
    public Text caughtSatellitesText;
    public Slider healthbarSlider;
    public LevelManager gameMaster;

    private SuperCharacterController player;
    private int oldIndex;
    private float oldHealth;
    private Image sliderBackground;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SuperCharacterController>();

    }
    private void Update()
    {

        // if (oldHealth != player.CurrentHealth)
        // {
        //     float healthPercentage = player.CurrentHealth / player.MaxHealth;
        //     healthbarSlider.value = healthPercentage;
        //     oldHealth = player.CurrentHealth;
        // }

        // scoreText.text = "Score: " + gameMaster.ScoreForDisplay().ToString("f0");
        // waveNumberText.text = "Wave: " + (gameMaster.WaveNumberForScore() + 1);


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
