using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceContainer_PrimaryPlayer : GamePieceContainer
{
    bool anyPiecesGrabbed = false;
    [SerializeField] private GameObject tutorialText;

    public override void AddPiece(GamePiece gamePiece)
    {
        base.AddPiece(gamePiece);
        if(anyPiecesGrabbed)
        {
            return;
        }
        gamePiece.OnGrabbed += OnGrab;
    }

    public override void RemovePiece(GamePiece gamePiece)
    {
        base.RemovePiece(gamePiece);
        gamePiece.OnGrabbed -= OnGrab;
    }

    public void OnGrab(GamePiece gamePiece)
    {
        if(!anyPiecesGrabbed)
        {
            anyPiecesGrabbed = true;
            Destroy(tutorialText);
        }
    }

}
