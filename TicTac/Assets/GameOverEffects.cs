using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEffects : MonoBehaviour
{
    [SerializeField] private GameOverEffect playerWinPrefab;
    [SerializeField] private GameOverEffect tiePrefab;
    [SerializeField] private GameOverEffect aiWinPrefab;

    public void Start()
    {
        TicTacToe.Instance.OnPlayerWin += OnPlayerWin;
        TicTacToe.Instance.OnAIWin += OnAIWin;
        TicTacToe.Instance.OnTie += OnTie;
    }

    public void OnTie()
    {
        StartCoroutine(GameOver(tiePrefab));
    }

    public void OnPlayerWin()
    {
        StartCoroutine(GameOver(playerWinPrefab));
    }

    public void OnAIWin()
    {
        StartCoroutine(GameOver(aiWinPrefab));
    }

    private IEnumerator GameOver(GameOverEffect gameOverPrefab)
    {
        GameOverEffect effect = Instantiate(gameOverPrefab, this.transform) as GameOverEffect;
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = Quaternion.identity;
        //Color Sky
        //Color Table
        //Color Pieces
        //Show Text Prefab 
        //Play Fireworks


        //
        //Normal Sky Color
        //Normal Table Color
        //Turn off fireworks

        //New Game
        return null;
    }
}
