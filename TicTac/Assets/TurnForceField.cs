using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnForceField : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GamePiece gamePiece = other.GetComponent<GamePiece>();
        if(gamePiece != null)
        {

        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        GamePiece gamePiece = other.GetComponent<GamePiece>();
        if (gamePiece != null)
        {

        }
    }
}
