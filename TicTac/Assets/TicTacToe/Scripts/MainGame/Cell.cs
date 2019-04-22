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

    public State state;

    private List<GamePiece> hoveringPieces = new List<GamePiece>();
    [SerializeField] private MeshRenderer background;
    [SerializeField] private Color staticColor;
    [SerializeField] private Color hoverOColor;
    [SerializeField] private Color hoverXColor;
    [SerializeField] private ParticleSystem hoverParticleSystem;

    public GamePiece FilledGamePiece;

    public delegate void CellFilled(Cell cell);
    public CellFilled OnCellFilled;

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
        SetHighlightState(State.Empty);
        if(FilledGamePiece != null)
        {
            FilledGamePiece.Delete();
            FilledGamePiece = null;
        }
        hoveringPieces.Clear();
    }

    public void OnPieceEnter(GamePiece piece)
    {
        if (hoveringPieces.Contains(piece) || !IsEmpty())
            return;

        hoveringPieces.Add(piece);
        piece.OnReleased += OnGamePieceReleased;
        if(piece.type == GamePiece.Type.O)
        {
            SetHighlightState(State.O);
        }
        else
        {
            SetHighlightState(State.X);
        }
    }
    
    public void OnPieceExit(GamePiece piece)
    {
        if (!hoveringPieces.Contains(piece) || !IsEmpty())
            return;
        
        piece.OnReleased -= OnGamePieceReleased;
        hoveringPieces.Remove(piece);
        if (hoveringPieces.Count == 0)
        {
            SetHighlightState(State.Empty);
        }
    }

    private void OnGamePieceReleased(GamePiece gamePiece)
    {
        AddPieceToCell(gamePiece);
    }

    public void AddPieceToCell(GamePiece gamePiece)
    {
        if(!TicTacToe.Instance.IsPiecesTurn(gamePiece) || !IsEmpty())
        {
            gamePiece.Explode(Quaternion.identity);
            return;
        }
        if (gamePiece.type == GamePiece.Type.O)
        {
            SetCellState(State.O);
        }
        else
        {
            SetCellState(State.X);
        }
        StartCoroutine(MovePieceToCell(gamePiece));
        SetHighlightState(state);
        hoverParticleSystem.Stop();
        if (OnCellFilled != null)
        {
            OnCellFilled.Invoke(this);
        }
    }

    private IEnumerator MovePieceToCell(GamePiece gamePiece)
    {
        FilledGamePiece = gamePiece;
        gamePiece.PlacedOnBoard();
        gamePiece.transform.SetParent(this.transform);
        float startTime = Time.time;
        float animationTime = .25f;
        Vector3 initialPosition = gamePiece.transform.position;
        Quaternion initialRotation = gamePiece.transform.rotation;

        while(true)
        {
            if(gamePiece == null)
            {
                break;
            }
            float percentageDone = (Time.time - startTime) / animationTime;
            if(percentageDone > 1)
            {
                percentageDone = 1;
            }
            gamePiece.transform.position = Vector3.Lerp(initialPosition, transform.position, percentageDone);
            gamePiece.transform.rotation = Quaternion.Lerp(initialRotation, transform.rotation, percentageDone);
            if(percentageDone == 1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void SetCellState(State state)
    {
        this.state = state;
    }

    private void SetHighlightState(State highlightState)
    {
        if(highlightState == State.Empty)
        {
            hoverParticleSystem.Stop();
            background.material.color = staticColor;
        }
        if (highlightState == State.O)
        {
            hoverParticleSystem.Play();
            background.material.color = hoverOColor;
        }
        if (highlightState == State.X)
        {
            background.material.color = hoverXColor;
        }
    }

}
