using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Score;
    public GamePiece.Type GamePieceType;

    public void AddToScore()
    {
        Score++;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public virtual void StartTurn()
    {

    }

    public virtual void EndTurn()
    {

    }

}
