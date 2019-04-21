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
    public GrabAction OnGrabbed;
    private List<Cell> hoveringCells = new List<Cell>();
    private Cell currentCellHoveringCell;
    [SerializeField] private Collider grabbableCollider;
    [SerializeField] private Transform explosionPrefab;

    protected override void Start()
    {
        base.Start();
        TicTacToe.Instance.OnTurnChange += OnTurnChange;
    }

    private void OnTurnChange(Player player)
    {
        if(player.GamePieceType != type)
        {
            if(currentCellHoveringCell != null)
            {
                currentCellHoveringCell.OnPieceExit(this);
                currentCellHoveringCell = null;
            }
        }
    }
    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        if(OnGrabbed != null)
        {
            OnGrabbed.Invoke(this);
        }
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        if(OnReleased != null)
        {
            OnReleased.Invoke(this);
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
        if(closestCell != currentCellHoveringCell)
        {
            if(currentCellHoveringCell != null)
            {
                currentCellHoveringCell.OnPieceExit(this);
            }
            currentCellHoveringCell = closestCell;
            if (currentCellHoveringCell != null)
            {
                currentCellHoveringCell.OnPieceEnter(this);
            }
        }
    }

    public void Update()
    {
        if(isGrabbed && TicTacToe.Instance.IsPiecesTurn(this))
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

    private void OnTriggerExit(Collider other)
    {
        Cell cell = other.GetComponent<Cell>();
        if (cell != null && hoveringCells.Contains(cell))
        {
            cell.OnPieceExit(this);
            hoveringCells.Remove(cell);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        CheckDropOnFloor(collision);
        CheckDropOnBoard(collision);
    }

    private void CheckDropOnBoard(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Board"))
        {
            if(!isGrabbed)
            {
                TicTacToe.Instance.TryStickPieceToBoard(this);
            }
        }
    }

    private void CheckDropOnFloor(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode(Quaternion.identity);
        }
    }

    public void Explode(Quaternion explosionRotation)
    {
        Transform explosion = Instantiate(explosionPrefab, null) as Transform;
        explosion.position = transform.position;
        explosion.rotation = explosionRotation;
        Destroy(this.gameObject);
    }

    public void Delete()
    {
        StartCoroutine(DeleteSequence());
    }

    private IEnumerator DeleteSequence()
    {
        float startTime = Time.time;
        float animationTime = .25f;
        Vector3 initialScale = transform.localScale;
        while (true)
        {
            float percentageDone = (Time.time - startTime) / animationTime;
            if (percentageDone > 1)
            {
                percentageDone = 1;
            }
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, percentageDone);
            if (percentageDone == 1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }

    public void SetGrabbable(bool isGrabbale)
    {
        IsGrabbale = isGrabbale;
    }

    public void SetIsKinematic(bool isKinematic)
    {
        GetComponent<Rigidbody>().isKinematic = isKinematic;
    }


}
