using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TextMeshPro aiScoreText;
    [SerializeField] private TextMeshPro playerScoreText;
    public int AIScore;
    public int PlayerScore;

    public void Start()
    {
        TicTacToe.Instance.OnAIWin += OnAIWin;
        TicTacToe.Instance.OnPlayerWin += OnPlayerWin;
        TicTacToe.Instance.OnSessionReset += ResetScores;
    }

    private void ResetScores()
    {
        SetAIScore(0);
        SetPlayerScore(0);
    }

    private void SetAIScore(int score)
    {
        AIScore = score;
        aiScoreText.SetText(AIScore.ToString());
    }

    private void SetPlayerScore(int score)
    {
        PlayerScore = score;
        playerScoreText.SetText(PlayerScore.ToString());
    }

    private void OnAIWin()
    {
        int score = AIScore + 1;
        SetAIScore(score);
    }

    private void OnPlayerWin()
    {
        int score = PlayerScore + 1;
        SetPlayerScore(score);
    }
}
