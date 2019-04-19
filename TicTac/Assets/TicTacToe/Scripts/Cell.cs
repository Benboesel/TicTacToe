using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    
    public enum State
    {
        Empty,
        X,
        O
    }

    public enum HoveredStates
    {
        Unhovered,
        Hovered,
        Filled
    }

    private State state;

    private List<GamePiece> hoveringPieces = new List<GamePiece>();

    public State GetState()
    {
        return state;
    }

    public bool IsEmpty()
    {
        return state == State.Empty;
    }

    public void Clear()
    {
        state = State.Empty;
        //Delete gamepiece inside of itself
    }

    public void OnPieceEnter(GamePiece piece)
    {
        if (hoveringPieces.Contains(piece))
            return;

        hoveringPieces.Add(piece);
        piece.OnReleased += OnGamePieceReleased;
        if (IsEmpty())
        {
            SetHighlightState(HoveredStates.Hovered);
        }
        else
        {
            SetHighlightState(HoveredStates.Filled);
        }
    }
    
    public void OnPieceExit(GamePiece piece)
    {
        if (!hoveringPieces.Contains(piece))
            return;
        
        piece.OnReleased -= OnGamePieceReleased;
        hoveringPieces.Remove(piece);
        if (hoveringPieces.Count == 0)
        {
            SetHighlightState(HoveredStates.Unhovered);
        }
    }

    private void OnGamePieceReleased(GamePiece gamePiece)
    {
        if(gamePiece.type == GamePiece.Type.O)
        {
            SetCellState(State.O);
        }
        else
        {
            SetCellState(State.X);
        }
    }

    private void SetCellState(State state)
    {
        this.state = state;
    }

    private void SetHighlightState(HoveredStates highlightState)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if(highlightState == HoveredStates.Unhovered)
        {
            meshRenderer.material.color = Color.white;
        }
        if (highlightState == HoveredStates.Hovered)
        {

            meshRenderer.material.color = Color.green;
        }
        if(highlightState == HoveredStates.Filled)
        {
            meshRenderer.material.color = Color.red;
        }
    }

}
