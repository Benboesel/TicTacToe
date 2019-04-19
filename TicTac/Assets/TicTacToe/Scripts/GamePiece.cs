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
    [SerializeField] private Collider grabbableCollider;

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
        }
    }

    private void HoverClosestCell()
    {
        float closestDistance = Mathf.Infinity;
        Cell closestCell = null;
        foreach(Cell cell in hoveringCells)
        {
            float distance = Vector3.Distance(cell.transform.position, this.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestCell = cell;
            }
        }
        foreach(Cell cell in hoveringCells)
        {
            if(cell == closestCell)
            {
                cell.OnPieceEnter(this);
            }
            else
            {
                cell.OnPieceExit(this);
            }
        }
    }

    public void Update()
    {
        if(isGrabbed)
        {
            HoverClosestCell();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell != null && !hoveringCells.Contains(cell))
        {
            hoveringCells.Add(cell);
        }
    }

    public void SetGrabbable(bool grabbale)
    {
        grabbableCollider.enabled = grabbale;
    }
    public void SetIsKinematic(bool isKinematic)
    {
        GetComponent<Rigidbody>().isKinematic = isKinematic;
    }
}
