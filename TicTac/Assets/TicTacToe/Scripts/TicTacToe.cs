using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TicTacToe : Singleton<TicTacToe>
{
    public enum GameState
    {
        Inactive,
        Playing,
        PlayerWon,
        AIWon,
        Tie
    }
    
    public List<Cell> cells = new List<Cell>();
    [SerializeField] private Player player;
    [SerializeField] private AI ai;
    private Player currentPlayersTurn;
    [SerializeField] private TouchableButton resetButton;

    public Action OnSessionReset;
    public Action OnPlayerWin;
    public Action OnAIWin;

    private int[,] possibleLines = new int[,]
    {
        {0,1,2},
        {3,4,5},
        {6,7,8},
        {0,3,6},
        {1,4,7},
        {2,5,8},
        {0,4,8},
        {6,4,2},
    };

    public void Awake()
    {
        resetButton.OnButtonUp += ResetSession;
        foreach(Cell cell in cells)
        {
            cell.OnCellFilled += OnCellFilled;
        }
    }

    public void OnCellFilled(Cell cell)
    {
        if(IsGameOver())
        {
            if(IsTie())
            {
                Tie();
            }
            else
            {
                if(currentPlayersTurn == player)
                {
                    PlayerWon();
                }
                else
                {
                    AIWon();
                }
            }
            NewGame();
        }
        else
        {
            NextTurn();
        }
    }

    private void Tie()
    {
        
    }

    private void PlayerWon()
    {
        if(OnPlayerWin != null)
        {
            OnPlayerWin.Invoke();
        }
    }

    private void AIWon()
    {
        if(OnAIWin != null)
        {
            OnAIWin.Invoke();
        }
    }

    public void ResetSession()
    {
        if (OnSessionReset != null)
        {
            OnSessionReset.Invoke();
        }
        NewGame();
    }

    public void NewGame()
    {
        foreach(Cell cell in cells)
        {
            cell.Clear();
        }
        SetTurn(player);
    }

    public void StartGame()
    {
        currentPlayersTurn = player;
    }

    public void QuitGame()
    {

    }

    private void SetTurn(Player player)
    {

    }

    public bool IsGameOver()
    {
        return IsWinner() || IsTie();
    }

    private bool IsWinningLine(int a, int b, int c)
    {
        return cells[a].GetState() == cells[b].GetState() && cells[b].GetState() == cells[c].GetState() && cells[c].GetState() != Cell.State.Empty;
    }

    private bool IsWinner()
    {
        for (int i = 0; i <= possibleLines.GetLength(0) - 1; i++)
        {
            bool lineWon = IsWinningLine(possibleLines[i, 0], possibleLines[i, 1], possibleLines[i, 2]);
            if (lineWon)
            {
                return true;
            }
        }
        return false;
    }
    
    private bool IsTie()
    {
        return IsBoardFilled() && !IsWinner();
    }

    private bool IsBoardFilled()
    {
        foreach(Cell cell in cells)
        {
            if (cell.GetState() == Cell.State.Empty)
            {
                return false;
            }
        }
        return true;
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
