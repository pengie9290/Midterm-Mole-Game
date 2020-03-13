using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance = null;

    private bool _gameInProgress = false;
    public bool GameInProgress
    {
        get
        {
            return _gameInProgress;
        }

        set
        {
            if (!_gameInProgress && value)
            {
                Points = 0;
                AddPoints(0);
                CurrentTime = MaxTime;
            }
            _gameInProgress = value;
            if (_gameInProgress)
            {
                FinalScoreDisplay.text = "";
            }
            else
            {
                FinalScoreDisplay.text = "Final Score\n" + TotalScore;
            }
        }
    }

    public Text FinalScoreDisplay;
    public Text PointsDisplay;
    public Text TimeBonusDisplay;
        
    public float Points = 0f;
    public float MaxTime = 100f;
    public float CurrentTime = 100f;
    public float TimePointsMultiplier = 1;

    public int TimeBonus
    {
        get
        {
            return Mathf.CeilToInt(CurrentTime * TimePointsMultiplier);
        }
    }


    public float TotalScore
    {
        get
        {
            return Points + TimeBonus;
        }
    }




    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }

        GameInProgress = true;
    }

    void Update()
    {
        if (GameInProgress && CurrentTime > 0) Countdown();
    }

    public void AddPoints(float points)
    {
        Points += points;
        PointsDisplay.text = "Points\n" + Points;
        if (!GameInProgress) FinalScoreDisplay.text = "Final Score\n" + TotalScore;
    }

    void Countdown()
    {
        CurrentTime -= Time.deltaTime;
        TimeBonusDisplay.text = "Time Bonus\n" + TimeBonus;
    }
}
