using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GamePiece : OVRGrabbable
{
    public enum Type
    {
        X,
        O
    }

    public Type type;

    public delegate void GrabAction(GamePiece gamePiece);
    public GrabAction OnReleased;
    private List<Cell> hoveringCells = new List<Cell>();

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        if(OnReleased != null)
        {
            OnReleased.Invoke(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell != null && hoveringCells.Contains(cell))
        {
            cell.OnPieceExit(this);
            hoveringCells.Remove(cell);
            if(hoveringCells.Count > 0)
            {
                hoveringCells[hoveringCells.Count -1 ].OnPieceEnter(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell != null && !hoveringCells.Contains(cell))
        {
            if(hoveringCells.Count > 0)
            {
                hoveringCells[hoveringCells.Count - 1].OnPieceExit(this);
            }
            cell.OnPieceEnter(this);
            hoveringCells.Add(cell);
        }
    }

}
