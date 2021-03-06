﻿using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	private Score scoreEasy, scoreMedium, scoreHard, scoreVeryHard, score;
	private ScoreManagerView scoreManagerView;

	private Enums.GameDifficulty gameDifficulty = Enums.GameDifficulty.Easy;
	private Enums.GameState gameState = Enums.GameState.MainMenu;

	private void Awake()
	{
		InitializeScore();
		scoreManagerView = GetComponent<ScoreManagerView>();
	}

	private void OnEnable()
	{
		EventManager.OnChangedGameDifficulty.AddListener(ChangeScoreDifficulty);
		EventManager.OnPlayerScoredPoint.AddListener(AddScore);
		EventManager.OnSetGameState.AddListener(OnSetGameState);
	}

	private void OnDisable()
	{
		EventManager.OnChangedGameDifficulty.RemoveListener(ChangeScoreDifficulty);
		EventManager.OnPlayerScoredPoint.RemoveListener(AddScore);
		EventManager.OnSetGameState.RemoveListener(OnSetGameState);
	}
	
	public void InitializeScore()
	{
		scoreEasy = new Score();
		scoreMedium = new Score();
		scoreHard = new Score();
		scoreVeryHard = new Score();
		score = scoreEasy;
	}

	private void AddScore()
	{
		AddValueToCurrentScore(1);
		SetScore(score);
	}

	public void AddValueToCurrentScore(int value)
	{
		score.currentScore += value;
		if (score.currentScore < 0) score.currentScore = 0;
		score.CheckHighScore();
	}

	public int GetCurrentScore()
	{
		return this.score.currentScore;
	}

	private void ChangeScoreDifficulty(Enums.GameDifficulty newDifficulty)
	{
		gameDifficulty = newDifficulty;

		switch(gameDifficulty)
		{
			case Enums.GameDifficulty.Easy:
				score = scoreEasy;
				break;
			case Enums.GameDifficulty.Medium:
				score = scoreMedium;
				break;
			case Enums.GameDifficulty.Hard:
				score = scoreHard;
				break;
			case Enums.GameDifficulty.VeryHard:
				score = scoreVeryHard;
				break;
			default:
				Debug.LogError("Difficulty not Set Correctly");
				break;
		}

		SetScore(score);
	}

	private void OnSetGameState(Enums.GameState state)
	{
		gameState = state;

		if (state.Equals(Enums.GameState.Gameplay))
		{
			ResetCurrentScores();
		}

		SetScore(score);
	}

	private void ResetCurrentScores()
	{
		this.scoreEasy.currentScore =
		this.scoreMedium.currentScore =
		this.scoreHard.currentScore =
		this.scoreVeryHard.currentScore = 0;
	}

	private void SetScore(Score _score)
	{
		if(gameState.Equals(Enums.GameState.MainMenu))
		{
			scoreManagerView.SetCurrentHighScore(_score);
		} else
		{
			scoreManagerView.SetCurrentScore(_score);
		}
	}
}
