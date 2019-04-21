using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TicTacToe : Singleton<TicTacToe>
{
    public List<Cell> cells = new List<Cell>();
    [SerializeField] private Player player;
    [SerializeField] private AI ai;
    [SerializeField] private TouchableButton resetButton;
    private Player currentPlayersTurn;

    public Action OnSessionReset;
    public Action OnNewGame;
    public Action OnPlayerWin;
    public Action OnAIWin;
    public Action OnTie;
    public Action<Player> OnTurnChange;

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
        NewGame();
    }

    public void OnCellFilled(Cell cell)
    {
        if(IsGameOver())
        {
            GameOver();
        }
        else
        {
            NextTurn();
        }
    }

    private void GameOver()
    {
        if (IsTie())
        {
            Tie();
        }
        else if(currentPlayersTurn == player)
        {
            PlayerWon();
        }
        else
        {
            AIWon();
        }
        currentPlayersTurn = null;
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
        if(OnNewGame != null)
        {
            OnNewGame.Invoke();
        }
        SetTurn(player);
    }

    public void NextTurn()
    {
        currentPlayersTurn.EndTurn();
        if (currentPlayersTurn == player)
        {
            SetTurn(ai);
        }
        else
        {
            SetTurn(player);
        }
        currentPlayersTurn.StartTurn();
    }

    private void SetTurn(Player player)
    {
        currentPlayersTurn = player;
        if(OnTurnChange != null)
        {
            OnTurnChange.Invoke(player);
        }
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
        return GetEmptyCells().Count == 0;
    }
    
    private List<Cell> GetEmptyCells()
    {
        List<Cell> emptyCells = new List<Cell>();
        foreach (Cell cell in cells)
        {
            if (cell.GetState() == Cell.State.Empty)
            {
                emptyCells.Add(cell);
            }
        }
        return emptyCells;
    }

    private Cell GetClosestCell(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        Cell closestCell = null;
        foreach (Cell cell in GetEmptyCells())
        {
            float distance = Vector3.Distance(position, cell.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCell = cell;
            }
        }
        return closestCell;
    }

    public void TryStickPieceToBoard(GamePiece gamePiece)
    {
        if(IsPiecesTurn(gamePiece))
        {
            Cell closestCell = GetClosestCell(gamePiece.transform.position);
            if(closestCell != null && Vector3.Distance(closestCell.transform.position, gamePiece.transform.position) < .16f)
            {
                closestCell.AddPieceToCell(gamePiece);
            }
            else
            {
                gamePiece.Explode(Quaternion.identity);
            }
        }
        else
        {
           gamePiece.Explode(Quaternion.identity);
        }
    }

    public bool IsPiecesTurn(GamePiece gamePiece)
    {
        return currentPlayersTurn != null && gamePiece.type == currentPlayersTurn.GamePieceType;
    }

    private void Tie()
    {
        if (OnTie != null)
        {
            OnTie.Invoke();
        }
    }

    private void PlayerWon()
    {
        if (OnPlayerWin != null)
        {
            OnPlayerWin.Invoke();
        }
    }

    private void AIWon()
    {
        if (OnAIWin != null)
        {
            OnAIWin.Invoke();
        }
    }
}
