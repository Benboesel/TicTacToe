using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    public Cell[,] cells = new Cell[3,3];
    [SerializeField] private Player player;
    [SerializeField] private AI ai;
    private Player currentPlayersTurn;

    public void Restart()
    {

    }

    public void StartGame()
    {
        currentPlayersTurn = player;
    }

    public void QuitGame()
    {

    }

    public bool IsGameOver()
    {
        return false;
    }
    
    public void NextTurn()
    {
        if(currentPlayersTurn == player)
        {
            currentPlayersTurn = ai;
        }
        else
        {
            currentPlayersTurn = player;
        }
    }
}
